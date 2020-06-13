using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedModels;

namespace Services
{
    public interface IUserService
    {
        Task<Guid> Register(User user);

        Task<User> GetUserByWallet(string walletName);

        Task<int> UpdateApplicationStatus(string id, int status);

        Task<Guid> CreateApplication(Application application);

        Task<IEnumerable<Application>> GetAllApplications();

        Task<IEnumerable<Application>> GetApplicationsByUser(string userId);
    }
}
