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
    public class EfRepairJobImageDal
      : EfEntityRepositoryBase<RepairJobImage, DataContext>, IRepairJobImageDal
    {
        public EfRepairJobImageDal(DataContext context) : base(context)
        {
        }
    }
}
