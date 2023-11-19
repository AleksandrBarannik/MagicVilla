using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberService
{
    public Task<T> GetAllAsync<T>(string token);
    public Task<T> GetAsync<T>(int id, string token);
    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token);
    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token);
    public Task<T> DeleteAsync<T>(int id, string token);
}