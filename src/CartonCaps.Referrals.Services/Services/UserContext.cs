using CartonCaps.Referrals.Services.Interfaces;

namespace CartonCaps.Referrals.Services.Services
{
    public class UserContext : IUserContext
    {
        private long userId;
        public long UserId { get => userId; set => userId = value; }

        public bool IsAuthenticated => userId > 0;
    }
}
