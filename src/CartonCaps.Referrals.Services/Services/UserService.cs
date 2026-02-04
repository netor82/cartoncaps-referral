using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.Shared;
using CartonCaps.Referrals.Services.Models.User;

namespace CartonCaps.Referrals.Services.Services
{
    public class UserService(
        IUserClient userClient,
        IDeferredDeepLinkClient deferredDeepLinkClient) : IUserService
    {
        public GenericResult<User> GetUser(long id)
        {
            return new GenericResult<User>(userClient.GetUser(id));
        }

        public GenericResult<List<User>> GetUsersByIds(long[] userIds)
        {
            List<User> result = [.. 
                userIds
                .Select(GetUser)
                .Where(x => x.Success)
                .Select(x => x.Data!)];

            return new GenericResult<List<User>>(result);
        }

        public GenericResult<string> GetUserReferralLink(long userId)
        {
            var user = userClient.GetUser(userId);
            var deepLinkResult = deferredDeepLinkClient.GetDeferredLink(user.ReferralCode);
            
            if (string.IsNullOrEmpty(deepLinkResult))
            {
                var result = new GenericResult<string>();
                result.SetError("Failed to create deferred deep link", ErrorCode.GenericError);
                return result;
            }
            return new GenericResult<string>(deepLinkResult);
        }
    }
}
