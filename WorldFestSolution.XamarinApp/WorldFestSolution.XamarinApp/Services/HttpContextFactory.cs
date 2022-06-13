using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public class HttpContextFactory : IHttpContextFactory
    {
        public Task<HttpClient> GetInstance()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback +=
                GetAlwaysTrueValidationCallback();

            HttpClient client = new HttpClient(handler);

            return Task.FromResult(client);
        }

        private static Func<HttpRequestMessage, X509Certificate2,
            X509Chain, SslPolicyErrors, bool> GetAlwaysTrueValidationCallback()
        {
            return (_, __, ___, ____) => false;
        }
    }
}
