using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Utilities
{
    public static class BusinessRules
    {
        public static async Task<IResult?> RunAsync(params Func<Task<IResult>>[] logics)
        {
            foreach (var logic in logics)
            {
                IResult result = await logic();
                if (!result.Success)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
