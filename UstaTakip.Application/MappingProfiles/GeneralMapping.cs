using AutoMapper;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Application.DTOs.InsurancePayments;
using UstaTakip.Application.DTOs.InsurancePolicys;
using UstaTakip.Application.DTOs.RepairJobImage;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Application.DTOs.VehicleImages;
using UstaTakip.Application.DTOs.Vehicles;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.MappingProfiles
{
    public class GeneralMapping : Profile
    {


        public GeneralMapping()
        {
           


            CreateMap<UserCreateDto, User>()
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<User, UserForLoginDto>().ReverseMap();
            CreateMap<User, UserForRegisterDto>().ReverseMap();
            CreateMap<User, UserListDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();


            CreateMap<OperationClaim, OperationClaimCreateDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimListDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimUpdateDto>().ReverseMap();



            CreateMap<UserOperationClaim, UserOperationClaimCreateDto>().ReverseMap();
            CreateMap<UserOperationClaim, UserOperationClaimListDto>().ReverseMap();

            // Customer
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();
            CreateMap<Customer, CustomerUpdateDto>();
            CreateMap<Customer, CustomerListDto>()
    .ForMember(dest => dest.DisplayName, opt =>
        opt.MapFrom(src =>
            src.Type == CustomerType.Individual
                ? src.FullName
                : src.CompanyName
        )
    );
            CreateMap<RepairJobImage, RepairJobImageListDto>();


            // Vehicle
            CreateMap<Vehicle, VehicleCreateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleUpdateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleListDto>()
     .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
     .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.VehicleImages));



            // RepairJob
            CreateMap<RepairJob, RepairJobCreateDto>().ReverseMap();
            CreateMap<RepairJob, RepairJobUpdateDto>().ReverseMap();
            CreateMap<RepairJob, RepairJobListDto>()
             .ForMember(d => d.VehiclePlate, o => o.MapFrom(s => s.Vehicle.Plate));

            CreateMap<VehicleImage, VehicleImageListDto>().ReverseMap();
            CreateMap<VehicleImage, VehicleImageCreateDto>().ReverseMap();

            // InsurancePolicy Mappings
            CreateMap<InsurancePolicy, InsurancePolicyCreateDto>().ReverseMap();
            CreateMap<InsurancePolicy, InsurancePolicyUpdateDto>().ReverseMap();
            CreateMap<InsurancePolicy, InsurancePolicyListDto>().ReverseMap();

            // InsurancePayment Mappings
            CreateMap<InsurancePayment, InsurancePaymentCreateDto>().ReverseMap();
            CreateMap<InsurancePayment, InsurancePaymentUpdateDto>().ReverseMap();
            CreateMap<InsurancePayment, InsurancePaymentListDto>().ReverseMap();

        }
    }
}
