namespace MagicVilla_Web.Models.Dto.Identity;

public class LoginResponseDTO
{
    public UserDTO User { get; set; }
    public string Token { get; set; }
}