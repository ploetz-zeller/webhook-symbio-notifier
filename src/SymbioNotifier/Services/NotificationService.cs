using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace SymbioNotifier.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(StringValues message);
        void Subscribe(ClaimsPrincipal user);
        void Unsubscribe(ClaimsPrincipal user);
        bool IsSubscribed(ClaimsPrincipal user);
        IEnumerable<string> GetCollectedMessages(ClaimsPrincipal user);
    }

    public class DefaultNotificationService : INotificationService
    {
        private readonly IUserProfileService _userProfileService;

        public DefaultNotificationService(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public Task SendNotificationAsync(StringValues message)
        {
            return Task.Run(() =>
            {
                foreach (var user in _userProfileService.AllUsers.Where(u => u.IsSubscribed).ToList())
                {
                    user.CollectedMessages.Add(message);
                }
            });
        }

        void INotificationService.Subscribe(ClaimsPrincipal user)
        {
            var upn = GetUpn(user);
            var internalUser = _userProfileService?.GetUser(upn);
            if (internalUser != null)
                internalUser.IsSubscribed = true;

            _userProfileService?.SaveChanges();
        }

        public void Unsubscribe(ClaimsPrincipal user)
        {
            var upn = GetUpn(user);
            var internalUser = _userProfileService?.GetUser(upn);
            if (internalUser != null)
                internalUser.IsSubscribed = false;

            _userProfileService?.SaveChanges();
        }

        public bool IsSubscribed(ClaimsPrincipal user)
        {
            var upn = GetUpn(user);
            var internalUser = _userProfileService?.GetUser(upn);
            return internalUser?.IsSubscribed ?? false;
        }

        public IEnumerable<string> GetCollectedMessages(ClaimsPrincipal user)
        {
            var upn = GetUpn(user);
            var internalUser = _userProfileService?.GetUser(upn);
            return internalUser?.CollectedMessages ?? Enumerable.Empty<string>();
        }

        private string GetUpn(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
        }
    }
}
