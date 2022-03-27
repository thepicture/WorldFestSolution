using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IInviteOfFestivalDataStore
    {
        Task<bool> ToggleParticipateAsync(int festivalId);
    }
}
