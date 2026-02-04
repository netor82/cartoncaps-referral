using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.User;
using Bogus;

namespace CartonCaps.Referrals.Services.Clients
{
    /// <summary>
    /// Mock implementation of the User API Service
    /// </summary>
    internal class UserClient : IUserClient
    {
        public User GetUser(long id)
        {
            var userFaker = new Faker<User>()
                .RuleFor(dest => dest.FirstName, f => f.Name.FirstName())
                .RuleFor(dest => dest.LastName, f => f.Name.LastName())
                .RuleFor(dest => dest.ReferralCode, f => f.Random.AlphaNumeric(6));

            var result = userFaker.Generate();
            result.Id = id;
            return result;
        }
    }
}
