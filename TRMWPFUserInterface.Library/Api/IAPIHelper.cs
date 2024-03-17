using System.Net.Http;
using System.Threading.Tasks;
using TRMWPFUserInterface.Models;

namespace TRMWPFUserInterface.Helpers
{
    public interface IAPIHelper
    {
        HttpClient APIClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}