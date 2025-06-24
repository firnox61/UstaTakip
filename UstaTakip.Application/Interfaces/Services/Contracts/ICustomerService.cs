using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Customers;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerListDto>>> GetAllAsync();
        Task<IDataResult<CustomerListDto>> GetByIdAsync(Guid id);
        Task<IResult> AddAsync(CustomerCreateDto dto);
        Task<IResult> UpdateAsync(CustomerUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
    }

}
