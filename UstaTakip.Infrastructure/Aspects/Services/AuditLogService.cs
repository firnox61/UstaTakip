using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Services;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Aspects.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly DataContext _context;

        public AuditLogService(DataContext context)
        {
            _context = context;
        }

        public void Log(string userEmail, string controller, string method)
        {
            var log = new AuditLog
            {
                UserEmail = userEmail,
                Controller = controller,
                Method = method,
                Action = $"{method} executed",
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }
    }

}
