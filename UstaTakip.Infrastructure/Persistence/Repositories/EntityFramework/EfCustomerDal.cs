using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, DataContext>, ICustomerDal
    {
        public EfCustomerDal(DataContext context) : base(context)
        {
        }
    }
}
