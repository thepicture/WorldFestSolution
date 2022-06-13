using System.Net.Http;
using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IHttpContextFactory
    {
        Task<HttpClient> GetInstance();
    }
}
