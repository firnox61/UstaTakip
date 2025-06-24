using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
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

        public async Task<IResult> AddAsync(CustomerCreateDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            await _customerDal.AddAsync(customer);
            return new SuccessResult("Müşteri başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _customerDal.DeleteAsync(new Customer { Id = id });
            return new SuccessResult("Müşteri silindi.");
        }

        public async Task<IDataResult<List<CustomerListDto>>> GetAllAsync()
        {
            var customers = await _customerDal.GetAllAsync();
            var dto = _mapper.Map<List<CustomerListDto>>(customers);
            return new SuccessDataResult<List<CustomerListDto>>(dto);
        }

        public async Task<IDataResult<CustomerListDto>> GetByIdAsync(Guid id)
        {
            var customer = await _customerDal.GetAsync(x => x.Id == id);
            if (customer == null)
                return new ErrorDataResult<CustomerListDto>("Müşteri bulunamadı.");

            var dto = _mapper.Map<CustomerListDto>(customer);
            return new SuccessDataResult<CustomerListDto>(dto);
        }

        public async Task<IResult> UpdateAsync(CustomerUpdateDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            await _customerDal.UpdateAsync(customer);
            return new SuccessResult("Müşteri güncellendi.");
        }
    }

}
