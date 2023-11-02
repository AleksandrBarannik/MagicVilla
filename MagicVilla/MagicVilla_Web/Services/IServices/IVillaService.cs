using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaService
{
    public Task<T> GetAllAsync<T>();
    public Task<T> GetAsync<T>(int id);
    public Task<T> CreateAsync<T>(VillaCreateDTO dto);
    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto);
    public Task<T> DeleteAsync<T>(int id);
}