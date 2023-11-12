using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberService
{
    public Task<T> GetAllAsync<T>();
    public Task<T> GetAsync<T>(int id);
    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto);
    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto);
    public Task<T> DeleteAsync<T>(int id);
}