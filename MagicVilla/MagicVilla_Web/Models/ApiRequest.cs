using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models;

//Request data from MagicVilla_VillaAPI( from server)
public class ApiRequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string Token { get; set; }
}