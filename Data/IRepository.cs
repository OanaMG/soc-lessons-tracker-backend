using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T> 
{
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllByUserAsync(string token);
    Task<List<T>> GetBySearchAndUserAsync(string token, string search);
    Task<T> GetByIdAsync(string id);
    Task<T> GetByDateAndUserAsync(string token, string date);
    Task<T> InsertAsync (T t);
    Task UpdateAsync (string id, T t);
    Task DeleteAsync(string id);
}
