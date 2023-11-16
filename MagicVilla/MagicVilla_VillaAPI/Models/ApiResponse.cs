using System.Net;

namespace MagicVilla_VillaAPI.Models;

public class ApiResponse
{
    public ApiResponse()
    {
        ErrorMessages = new List<string>();
    }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessages { get; set; }
    public object Result { get; set; }

    public void FillBadRequestDefault(string _message)
    {
        StatusCode = HttpStatusCode.BadRequest;
        IsSuccess = false;
        ErrorMessages.Add(_message);
    }

    public void FillGoodRequestDefault<T>(T resultRequest )
    {
        StatusCode = HttpStatusCode.OK;
        IsSuccess = true;
        Result = resultRequest;
    }
}