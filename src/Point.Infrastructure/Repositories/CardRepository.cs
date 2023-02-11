namespace Point.Infrastructure.Repositories;

// TODO: move to EF Core
public class CardRepository : ICardRepository
{
    private readonly string _connectionString;

    public CardRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:CardDbConnection"];
    }

    public async Task<Guid> Create(Card card, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var inputValues = new Dictionary<string, object>
        {
            {"@CardNumber", card.CardNumber},
            {"@QRCode", card.QRCode},
            {"@UserId", card.UserId},
            {"@CardTemplateId", card.CardTemplateId}
        };


        var parameters = new DynamicParameters();
        parameters.AddDynamicParams(inputValues);
        parameters.Add("@CreatedId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        var command = new CommandDefinition("spCard_createCard",
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        await connection.ExecuteAsync(command);

        return parameters.Get<Guid>("@CreatedId");
    }

    public async Task<List<Card>> ListByUserId(Guid id, ApiPageFilter filter, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = @"SELECT c.IsActive, c.CountSteps, c.CardNumber, ct.Title, ct.Description, ct.MaxBonuses, ci.Name
                        FROM Cards AS c
                            JOIN CardTemplates AS ct
                            ON c.CardTemplateId = ct.Id
                            JOIN CardImages AS ci
                            ON ci.Id = ct.BackgroundImageId
                        WHERE c.UserId = @UserId
                        LIMIT @Count OFFSET @Offset";

        var command = new CommandDefinition(sql, parameters: new
        {
            UserId = id,
            Count = filter.Count,
            Offset = filter.Offset
        }, cancellationToken: ct);

        var result = await connection.QueryAsync<Card, CardTemplate, CardImage, Card>(command,
        (card, template, image) =>
            {
                card.CardTemplate = template;
                card.CardTemplate.BackgroundImage = image;
                return card;
            }, splitOn: "Title, Name");

        return result.ToList();
    }

    public async Task<Card?> ById(Guid id, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = @"SELECT c.IsActive, c.CountSteps, c.CardNumber, ct.Title, ct.Description, ct.MaxBonuses, ci.Name
                        FROM Cards AS c
                            JOIN CardTemplates AS ct
                            ON c.CardTemplateId = ct.Id
                            JOIN CardImages AS ci
                            ON ci.Id = ct.BackgroundImageId
                        WHERE c.Id = @Id";

        var command = new CommandDefinition(sql, parameters: new { Id = id }, cancellationToken: ct);

        return (await connection.QueryAsync<Card, CardTemplate, CardImage, Card>(command,
                (card, template, image) =>
                {
                    card.CardTemplate = template;
                    card.CardTemplate.BackgroundImage = image;
                    return card;
                }, splitOn: "Title, Name")).FirstOrDefault();
    }

    public async Task<Card?> ByCardNumber(string number, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = @"SELECT c.IsActive, c.CountSteps, c.CardNumber, ct.Title, ct.Description, ct.MaxBonuses, ci.Name
                        FROM Cards AS c
                            JOIN CardTemplates AS ct
                            ON c.CardTemplateId = ct.Id
                            JOIN CardImages AS ci
                            ON ci.Id = ct.BackgroundImageId
                        WHERE c.CardNumber = @CardNumber";

        var command = new CommandDefinition(sql, parameters: new { CardNumber = number }, cancellationToken: ct);

        return (await connection.QueryAsync<Card, CardTemplate, CardImage, Card>(command,
                (card, template, image) =>
                {
                    card.CardTemplate = template;
                    card.CardTemplate.BackgroundImage = image;
                    return card;
                }, splitOn: "Title, Name")).FirstOrDefault();
    }

    public async Task<int> Update(Card card, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = "UPDATE Cards " +
                           "SET CountSteps = @CountSteps, QRCode = @QRCode, " +
                           "IsActive = @IsActive, CardTemplateId = @CardTemplateId " +
                           "WHERE Id = @Id";

        var command = new CommandDefinition(sql, parameters: card, cancellationToken: ct);

        return await connection.ExecuteAsync(command);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = "DELETE FROM Cards WHERE Id = @Id";

        var command = new CommandDefinition(sql, parameters: new { Id = id }, cancellationToken: ct);
        return await connection.ExecuteAsync(command);
    }
}