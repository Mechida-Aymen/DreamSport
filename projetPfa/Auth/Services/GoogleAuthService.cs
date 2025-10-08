using Auth.Dtos;
using Auth.Interfaces;
using Google.Apis.Auth;

namespace Auth.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private static readonly String ClientId = "39053290852-ol06nn3fdpl6cbs2eobh3toc44dbb8kr.apps.googleusercontent.com";

        public async Task<FacebookUserDto> ValidateGoogleTokenAsync(string token)
        {
            try
            {
                // Ensure ClientId and token are not null or empty
                if (string.IsNullOrEmpty(ClientId))
                {
                    throw new ArgumentNullException("ClientId cannot be null or empty.");
                }

                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Token cannot be null or empty.");
                }

                Console.WriteLine($"Token: {token}");
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { ClientId } // Replace with your actual Client ID
                };

                // Validate and decode the token
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

                // Parse payload into a DTO
                FacebookUserDto googleUser = new FacebookUserDto
                {
                    FacebookId = payload.Subject,   // Unique Google User ID
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    PictureUrl = payload.Picture,
                    Gender = null // Google API doesn't provide gender
                };

                return googleUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google token validation failed: {ex.Message}");
                return null;
            }
        }
    }
}
