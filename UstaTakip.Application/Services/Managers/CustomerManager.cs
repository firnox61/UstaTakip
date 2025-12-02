using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.Customers;
using UstaTakip.Application.Validators.Vehicles;
using UstaTakip.Core.Aspects.Caching;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IMapper mapper)
        {
            _customerDal = customerDal;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(CustomerCreateDtoValidator))]
        [TransactionScopeAspect]
        [CacheRemoveAspect("ICustomerService.Get*")]
        public async Task<IResult> AddAsync(CustomerCreateDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            await _customerDal.AddAsync(customer);
            return new SuccessResult("Müşteri başarıyla eklendi.");
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("ICustomerService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _customerDal.DeleteAsync(new Customer { Id = id });
            return new SuccessResult("Müşteri silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<CustomerListDto>>> GetAllAsync()
        {
            var customers = await _customerDal.GetAllAsync();
            var dto = _mapper.Map<List<CustomerListDto>>(customers);
            return new SuccessDataResult<List<CustomerListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<CustomerListDto>> GetByIdAsync(Guid id)
        {
            var customer = await _customerDal.GetAsync(x => x.Id == id);
            if (customer == null)
                return new ErrorDataResult<CustomerListDto>("Müşteri bulunamadı.");

            var dto = _mapper.Map<CustomerListDto>(customer);
            return new SuccessDataResult<CustomerListDto>(dto);
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("ICustomerService.Get*")]
        public async Task<IResult> UpdateAsync(CustomerUpdateDto dto)
        {
            var existing = await _customerDal.GetAsync(x => x.Id == dto.Id);
            if (existing == null)
                return new ErrorResult("Müşteri bulunamadı.");

            _mapper.Map(dto, existing);

            await _customerDal.UpdateAsync(existing);
            return new SuccessResult("Müşteri güncellendi.");
        }

    }


}
