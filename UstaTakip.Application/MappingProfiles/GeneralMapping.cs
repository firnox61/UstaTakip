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
          .ForMember(dest => dest.CustomerName,
              opt => opt.MapFrom(src =>
                  src.Customer.Type == CustomerType.Individual
                      ? src.Customer.FullName
                      : src.Customer.CompanyName
              ))
          .ForMember(dest => dest.Images,
              opt => opt.MapFrom(src => src.VehicleImages));






            CreateMap<VehicleImage, VehicleImageListDto>().ReverseMap();
            CreateMap<VehicleImage, VehicleImageCreateDto>().ReverseMap();

            // ===============================
            // RepairJob Mappings
            // ===============================

            CreateMap<RepairJob, RepairJobListDto>()
                .ForMember(dest => dest.VehiclePlate,
                    opt => opt.MapFrom(src => src.Vehicle.Plate))
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.InsurancePolicy != null
                        ? src.InsurancePolicy.CompanyName
                        : null))
                .ForMember(dest => dest.PolicyNumber,
                    opt => opt.MapFrom(src => src.InsurancePolicy != null
                        ? src.InsurancePolicy.PolicyNumber
                        : null))
                .ForMember(dest => dest.AgencyCode,
                    opt => opt.MapFrom(src => src.InsurancePolicy != null
                        ? src.InsurancePolicy.AgencyCode
                        : null));
            CreateMap<RepairJob, RepairJobUpdateDto>();

            CreateMap<RepairJobCreateDto, RepairJob>();
            CreateMap<RepairJobUpdateDto, RepairJob>();


            // ===============================
            // InsurancePolicy Mappings
            // ===============================

            CreateMap<InsurancePolicy, InsurancePolicyListDto>()
                .ForMember(dest => dest.VehiclePlate,
                    opt => opt.MapFrom(src => src.Vehicle.Plate));

            CreateMap<InsurancePolicyCreateDto, InsurancePolicy>();
            CreateMap<InsurancePolicyUpdateDto, InsurancePolicy>();


            // ===============================
            // InsurancePayment Mappings
            // ===============================

            // InsurancePayment → ListDto
            CreateMap<InsurancePayment, InsurancePaymentListDto>()
       .ForMember(dest => dest.RepairJobDescription,
           opt => opt.MapFrom(src => src.RepairJob.Description))
       .ForMember(dest => dest.CompanyName,
           opt => opt.MapFrom(src => src.InsurancePolicy.CompanyName))
       .ForMember(dest => dest.PolicyNumber,
           opt => opt.MapFrom(src => src.InsurancePolicy.PolicyNumber))
       .ForMember(dest => dest.AgencyCode,
           opt => opt.MapFrom(src => src.InsurancePolicy.AgencyCode))
       .ForMember(dest => dest.VehiclePlate,
           opt => opt.MapFrom(src => src.RepairJob.Vehicle.Plate)); // ← EKLENDİ


            // CreateDto → Entity
            CreateMap<InsurancePaymentCreateDto, InsurancePayment>();

            // UpdateDto → Entity
            CreateMap<InsurancePaymentUpdateDto, InsurancePayment>();



            // ===============================
            // MonthlyRepairJobStatsDto (OPSİYONEL)
            // ===============================
            // Eğer EF manuel dolduruyorsa gerek yok ama eklersen geleceğe uyumlu olur.

            // CreateMap<MonthlyRepairJobStatsDto, MonthlyRepairJobStatsDto>();

        }
    }
}
