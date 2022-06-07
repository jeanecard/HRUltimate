namespace Shared.DataTransferObjects
{
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public String? Name { get; init; }
        public String? FullAddress { get; init; }

    }

}
