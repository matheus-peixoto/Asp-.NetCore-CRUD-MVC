using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Treino3.Repositories.Interfaces
{
    public interface ICrud<T> where T : class
    {
        Task<T> FindByIdAsync(int id);
        Task<List<T>> FindAllAsync();
        Task<List<T>> FindAllWithFilterAsync(Expression<Func<T, bool>> filter);
        Task InsertAsync(T tObj);
        Task DeleteAsync(int id);
        Task UpdateAsync(T tObj);
    }
}
