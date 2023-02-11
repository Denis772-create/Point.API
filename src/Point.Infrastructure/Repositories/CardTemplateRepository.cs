namespace Point.Infrastructure.Repositories;

// TODO: move to EF Core
public class CardTemplateRepository : ICardTemplateRepository
{
    private readonly string _connectionString;

    public CardTemplateRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:CardDbConnection"];
    }


    public async Task<Guid> Create(CardTemplate template, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var inputValues = new Dictionary<string, object>
        {
            {"@Title", template.Title},
            {"@Description", template.Description},
            {"@MaxBonuses", template.MaxBonuses},
            {"@IsFreeFirstPoint", template.IsFreeFirstPoint},
            {"@CompanyId", template.CompanyId},
            {"@BackgroundImageId", template.BackgroundImage != null
                                   && template.BackgroundImage.Id != Guid.Empty ?
                template.BackgroundImage.Id: template.BackgroundImageId}
        };
        var parameters = new DynamicParameters();
        parameters.AddDynamicParams(inputValues);
        parameters.Add("@CreatedId",
            dbType: DbType.Guid,
            direction: ParameterDirection.Output);

        var command = new CommandDefinition("spCardTemplates_createCardTemplate",
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        await connection.ExecuteAsync(command);

        return parameters.Get<Guid>("@CreatedId");
    }

    public async Task<Guid> CreateBackgroundImage(CardImage image, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var inputValues = new Dictionary<string, object>
        {
            {"@Name", image.Name},
            {"@IsCommon", image.IsCommon}
        };
        var parameters = new DynamicParameters();
        parameters.AddDynamicParams(inputValues);
        parameters.Add("@CreatedId",
            dbType: DbType.Guid,
            direction: ParameterDirection.Output);

        var command = new CommandDefinition("spCardImages_createCardImage",
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        await connection.ExecuteAsync(command);

        return parameters.Get<Guid>("@CreatedId");
    }

    public Task Update(CardTemplate card, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Card> ById(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        const string sql = "DELETE FROM  CardTemplates WHERE Id = @Id";

        var command = new CommandDefinition(sql, parameters: new { Id = id }, cancellationToken: ct);
        return await connection.ExecuteAsync(command);
    }
}