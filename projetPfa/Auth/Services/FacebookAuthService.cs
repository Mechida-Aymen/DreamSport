using Auth.Dtos;
using Auth.Interfaces;
using Facebook;
using Microsoft.Extensions.Options;

namespace Auth.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly string _facebookAppId;
        private readonly string _facebookAppSecret;

        // Inject your Facebook App credentials from the configuration
        public FacebookAuthService(IConfiguration configuration)
        {
            _facebookAppId = configuration["FacebookAuth:AppId"];
            _facebookAppSecret = configuration["FacebookAuth:AppSecret"];
        }

        public async Task<FacebookUserDto> ValidateFacebookTokenAsync(string facebookToken)
        {
            try
            {
                var fbClient = new FacebookClient(facebookToken);

                // Make the request to get the user info
                dynamic user = await fbClient.GetTaskAsync("me?fields=id,first_name,last_name,email,gender,picture.type(large)");

                var xd = new FacebookUserDto
                {
                    FacebookId = user.id,
                    FirstName = user.first_name,
                    LastName = user.last_name,
                    Email = user.email,
                    Gender = user.gender,
                    PictureUrl = user.picture?.data?.url // Only the URL of the profile picture
                };
                return xd;
            }
            catch (Exception)
            {
                // Token validation failed
                return null;
            }
        }

    }
}
