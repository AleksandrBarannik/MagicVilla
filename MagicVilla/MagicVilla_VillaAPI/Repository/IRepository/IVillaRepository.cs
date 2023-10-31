using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface IVillaRepository
{
    Task<List<Villa>> GetAllAsync(Expression<Func<Villa,bool>> filter = null);
    //for get 1 villa & track - Отслеживать или нет
    Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null,bool tracked=true);
    Task CreateAsync(Villa entity);
    
    Task UpdateAsync(Villa entity);
    Task RemoveAsync(Villa entity);
    Task SaveAsync();
    
}