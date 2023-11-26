﻿using WebFrontend.Models;
using WebFrontend.Service.IService;
using Newtonsoft.Json;
using static WebFrontend.Utility.SD;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace WebFrontend.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("DemoMicroServiceAPI");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage message = new();
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
                switch (requestDto.ApiType)
                {
                    default:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                }

                HttpResponseMessage? apiResponse = null;
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                }
            }
            catch (Exception ex)
            {
                return new() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
