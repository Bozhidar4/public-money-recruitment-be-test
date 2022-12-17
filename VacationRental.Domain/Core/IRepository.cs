using System.Collections.Generic;
using System.Threading.Tasks;

namespace VacationRental.Domain.Core
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IDictionary<int, T>> GetAll();
    }
}
