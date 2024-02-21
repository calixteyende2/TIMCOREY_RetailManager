using System.Net.Http;
using System.Threading.Tasks;
using TRMWPFUserInterface.Models;

namespace TRMWPFUserInterface.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}