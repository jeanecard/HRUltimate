using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace EmployeeCompanyWebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            //CreateMap<Company, CompanyDto>()
            //.ForCtorParam("FullAddress",
            //opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<EmployeeForPatchDto, Employee>().ReverseMap();

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CompanyForUpdateDto, Company>().ReverseMap();
            CreateMap<CompanyForPatchDto, Company>().ReverseMap();
        }
    }
}
