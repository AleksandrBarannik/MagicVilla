namespace MagicVilla_VillaAPI.Models.Dto.Identity;

public class LoginResponseDTO
{
    public LocalUser User { get; set; }
    public string Token { get; set; }
}