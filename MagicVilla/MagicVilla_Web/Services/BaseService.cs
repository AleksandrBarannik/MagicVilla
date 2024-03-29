﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

// BaseService - base from all different service (Villa,VillaNumber etc)
namespace MagicVilla_Web.Services;

public class BaseService:IBaseService
{
    public ApiResponse responseModel { get; set; }
    public IHttpClientFactory httpClient { get; set; }

    protected const string version = "v1";

    public BaseService(IHttpClientFactory httpClient)
    {
        this.responseModel = new();
        this.httpClient = httpClient;
    }
     
    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept","application/json");
            message.RequestUri = new Uri(apiRequest.Url);

            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                Encoding.UTF8,"application/json");
            }

            switch (apiRequest.ApiType)
            {
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage apiResponse = null;

            if (!string.IsNullOrEmpty(apiRequest.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }
            
            apiResponse = await client.SendAsync(message);
            
            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            try
            {
                //For change flag IsSucsess  and Error (VillaNumber Exsist)
                ApiResponse ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiContent);

                if (ApiResponse!=null && (apiResponse.StatusCode == HttpStatusCode.BadRequest
                    || apiResponse.StatusCode == HttpStatusCode.NotFound))
                {
                    ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                    ApiResponse.IsSuccess = false;
                    
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }
            }
            
            catch (Exception e)
            {
                // If ApiResponse type is not ApiResponse => exceptionResponse
                var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return exceptionResponse;
            }
            
            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        
        catch(Exception e)
        {
            var dto = new ApiResponse()
            {
                ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
        }
    }
}