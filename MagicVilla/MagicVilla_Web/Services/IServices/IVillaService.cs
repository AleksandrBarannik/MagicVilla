using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaService
{
    public Task<T> GetAllAsync<T>(string token);
    public Task<T> GetAsync<T>(int id, string token);
    public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token);
    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token);
    public Task<T> DeleteAsync<T>(int id, string token);
}