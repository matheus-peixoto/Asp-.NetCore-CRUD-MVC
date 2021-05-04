using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Treino3.Data;
using Treino3.Models;
using Treino3.Repositories.Interfaces;
using Treino3.Services.Exceptions;

namespace Treino3.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public DepartmentRepository(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Department> FindByIdAsync(int id) => await _context.Department.Include(dep => dep.Sellers).FirstOrDefaultAsync(dep => dep.Id == id);

        public async Task<List<Department>> FindAllAsync() => await _context.Department.OrderBy(dep => dep.Name).ToListAsync();

        public async Task<List<Department>> FindAllWithFilterAsync(Expression<Func<Department, bool>> filter) =>
            await _context.Department.Where(filter).ToListAsync();

        public async Task InsertAsync(Department tObj)
        {
            _context.Add(tObj);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            try
            {
                Department department = await _context.Department.FirstOrDefaultAsync(dep => dep.Id == id);
                _context.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("This department has sellers");
            }

        }

        public async Task UpdateAsync(Department tObj)
        {
            if (! await _context.Department.AnyAsync(dep => dep.Id == tObj.Id))
                throw new NotFoundException("Id not founded");

            try
            {
                _context.Update(tObj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<List<Department>> GetAllPaginatedAsync(int page = 1)
        {
            int pageSize = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            return await _context.Department.OrderBy(dep => dep.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<Department>> GetAllPaginatedWithFilterAsync(Expression<Func<Department, bool>> filter, int page = 1)
        {
            int pageSize = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            return await _context.Department.Where(filter).OrderBy(dep => dep.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> TotalItemsAsync()
        {
            return await _context.Department.CountAsync();
        }

        public async Task<int> TotalItemsWithFilterAsync(Expression<Func<Department, bool>> filter)
        {
            return await _context.Department.Where(filter).CountAsync();
        }
    }
}
