using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTApp.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<string>> SelectListByConditionAsync(Expression<Func<T, string>> predicate);
        Task<T> GetItemByConditionAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task RemoveAsync(string chatTitle);
        Task UpdateAsync(T newEntity);
    }
}
