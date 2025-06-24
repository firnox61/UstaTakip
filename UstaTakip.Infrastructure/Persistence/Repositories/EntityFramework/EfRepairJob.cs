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
    public class EfRepairJob : EfEntityRepositoryBase<RepairJob, DataContext>, IRepairJobDal
    {
        public EfRepairJob(DataContext context) : base(context)
        {
        }
    }
}
