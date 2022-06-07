namespace Shared.DataTransferObjects
{
    public record CompanyForUpdateDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);
    public record CompanyForPatchDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationDto> Employees);
    public record EmployeeDto(Guid Id, string Name, int Age, String Position, Guid CompanyId);
    public record EmployeeForPatchDto(string Name, int Age, string Position);

}
