using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    //public record CompanyForCreationDto(string Name, string Address, string Country);
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; init; }
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
        public string? Address { get; init; }
        public string? Country { get; init; }
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }

    }

}
