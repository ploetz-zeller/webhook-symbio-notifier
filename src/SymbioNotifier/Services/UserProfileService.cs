using SymbioNotifier.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SymbioNotifier.Services
{
    public interface IUserProfileService
    {
        IEnumerable<User> AllUsers { get; }
        User GetUser(string upn);
        void SaveChanges();
    }

    public class MemoryUserProfileService : IUserProfileService
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();

        public IEnumerable<User> AllUsers => _users.Values;

        public User GetUser(string upn)
        {
            if (!_users.ContainsKey(upn))
                _users[upn] = new User();

            return _users[upn];
        }

        public void SaveChanges()
        {
            // only needed in persisting services
        }
    }
}
