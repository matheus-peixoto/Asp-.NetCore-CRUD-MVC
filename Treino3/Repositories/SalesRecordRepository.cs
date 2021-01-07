using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treino3.Data;
using Treino3.Models;
using Microsoft.EntityFrameworkCore;
using Treino3.Repositories.Interfaces;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace Treino3.Repositories
{
    public class SalesRecordRepository : ISalesRecordRepository
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public SalesRecordRepository(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<SalesRecord>> FinByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> querie = _context.SalesRecord.Include(sale => sale.Seller).ThenInclude(seller => seller.Department).AsQueryable();

            if (minDate.HasValue)
                querie = querie.Where(sale => sale.Date >= minDate);

            if (maxDate.HasValue)
                querie = querie.Where(sale => sale.Date <= maxDate);

            return await querie.OrderBy(sl => sl.Date).ToListAsync();
        }

        public async Task<List<SalesRecord>> GetAllPaginatedAsync(int page = 1)
        {
            int pageSize = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            return await _context.SalesRecord.OrderBy(dep => dep.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<SalesRecord>> GetAllPaginatedWithFilterAsync(Expression<Func<SalesRecord, bool>> filter, int page = 1)
        {
            int pageSize = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            return await _context.SalesRecord.Where(filter).OrderBy(dep => dep.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<SalesRecord>> GetAllPaginatedByDateAsync(DateTime? minDate, DateTime? maxDate, int page)
        {
            IQueryable<SalesRecord> querie = _context.SalesRecord.Include(sale => sale.Seller).ThenInclude(seller => seller.Department).AsQueryable();

            if (minDate.HasValue)
                querie = querie.Where(sale => sale.Date >= minDate);

            if (maxDate.HasValue)
                querie = querie.Where(sale => sale.Date <= maxDate);

            int pageSize = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            return await querie.OrderBy(sale => sale.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        

        public async Task<int> TotalItemsAsync()
        {
            return await _context.SalesRecord.CountAsync();
        }

        public async Task<int> TotalItemsWithFilterAsync(Expression<Func<SalesRecord, bool>> filter)
        {
            return await _context.SalesRecord.Where(filter).CountAsync();
        }

        public async Task<int> TotalItemsByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> querie = _context.SalesRecord.AsQueryable();

            if (minDate.HasValue)
                querie = querie.Where(sale => sale.Date >= minDate);

            if (maxDate.HasValue)
                querie = querie.Where(sale => sale.Date <= maxDate);
            return await querie.CountAsync();
        }
    }
}
