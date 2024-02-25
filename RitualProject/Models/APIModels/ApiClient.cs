using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    public class ApiClient : IApiClient
    {
        public HttpClient Client { get; } = new HttpClient();
        public string BaseUrl { get; } = "https://localhost:7138";
    }
}
