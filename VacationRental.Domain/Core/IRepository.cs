using System.Collections.Generic;

namespace VacationRental.Domain.Core
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IDictionary<int, T> GetAll();
    }
}
