using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    public interface IApiClient
    {
        HttpClient Client { get; } 
        string BaseUrl { get; }
    }
}
