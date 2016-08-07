using System;
using System.Net.Http;

namespace AsposeWeb.DesktopClient
{
    class AsposeApiHttpClient
    {   
            public static HttpClient GetClient()
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(WebApiClientConstants.WebApiServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                return client;
           
        }
    }
}
