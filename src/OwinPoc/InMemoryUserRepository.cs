using System.Security.Principal;

namespace OwinPoc
{
    public class InMemoryUserRepository : IUserRepository
    {
        private IIdentity identity;

        public InMemoryUserRepository(IIdentity identity)
        {
            this.identity = identity;
        }

        public string GetUserName(int userId)
        {
            return $"user_{userId} {identity?.Name}";
        }
    }
}