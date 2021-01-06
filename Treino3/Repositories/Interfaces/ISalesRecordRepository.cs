using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Treino3.Models;

namespace Treino3.Repositories.Interfaces
{
    public interface ISalesRecordRepository : IPagination<SalesRecord>
    {
        Task<List<SalesRecord>> FinByDateAsync(DateTime? minDate, DateTime? maxDate);
        Task<List<SalesRecord>> GetAllPaginatedByDateAsync(DateTime? minDate, DateTime? maxDate, int page);
        Task<int> TotalItemsByDateAsync(DateTime? minDate, DateTime? maxDate);
    }
}
