using System.Net.Http;
using System.Net.Http.Json;

namespace CatchLog.Wpf.Services;

public static class ApiService
{
    public static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("http://localhost:5000/api/")
    };
}