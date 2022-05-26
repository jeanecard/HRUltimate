namespace Shared.DataTransferObjects
{
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public String Name { get; init; }
        public String FullAddress { get; init; }

    }
    //public record CompanyForCreationDto(string Name, string Address, string Country);
    public record CompanyForCreationDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);
    public record CompanyForUpdateDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);
    public record CompanyForPatchDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);



    public record EmployeeDto(Guid Id, string Name, int Age, String Position, Guid CompanyId);
    public record EmployeeForCreationDto(string Name, int Age, string Position);
    public record EmployeeForUpdateDto(string Name, int Age, string Position);
    public record EmployeeForPatchDto(string Name, int Age, string Position);

}
