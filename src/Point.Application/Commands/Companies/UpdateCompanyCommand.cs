namespace Point.Application.Commands.Companies
{
    public class UpdateCompanyCommand : AddCompanyCommand
    {
        public UpdateCompanyCommand(CompanyDto input, Guid ownerId) : base(input, ownerId)
        {
        }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, OperationResult<Guid>>
    {
        private readonly IRepository<Company> _repository;

        public UpdateCompanyCommandHandler(IRepository<Company> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<Guid>> Handle(UpdateCompanyCommand request,
            CancellationToken ct)
        {
            var company = await _repository.GetByIdAsync(request.OwnerId, ct);
            if (company is null)
                return OperationResult<Guid>.Failure(ValidationErrors.DoesNotExist("Company"));

            company.Update(request.Input.Name, request.Input.Instagram, 
                request.Input.Telegram, request.Input.SupportNumber);

            _repository.Update(company);

            return OperationResult<Guid>.Success(company.Id);
        }
    }
}
