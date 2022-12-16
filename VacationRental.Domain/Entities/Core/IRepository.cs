using System.Collections.Generic;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Domain.Entities.Core
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IDictionary<int, T> GetAll();
    }
}
