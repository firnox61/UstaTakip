using AutoMapper;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.DTOs.Users;
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
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            CreateMap<Customer, CustomerListDto>().ReverseMap();

            // Vehicle
            CreateMap<Vehicle, VehicleCreateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleUpdateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleListDto>().ReverseMap();

            // RepairJob
            CreateMap<RepairJob, RepairJobCreateDto>().ReverseMap();
            CreateMap<RepairJob, RepairJobUpdateDto>().ReverseMap();
            CreateMap<RepairJob, RepairJobListDto>().ReverseMap();



        }
    }
}
