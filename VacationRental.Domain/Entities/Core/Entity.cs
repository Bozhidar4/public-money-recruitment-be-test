namespace VacationRental.Domain.Entities.Core
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }
    }
}
