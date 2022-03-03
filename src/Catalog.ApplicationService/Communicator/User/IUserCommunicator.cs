using Catalog.ApplicationService.Communicator.User.Model;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.User
{
    public interface IUserCommunicator
    {
        Task<GetUserResponse> GetUser(GetUserRequest request);
        bool IsUp();
    }
}