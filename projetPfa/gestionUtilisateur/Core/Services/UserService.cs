using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http;
using System.Text.Json;

namespace gestionUtilisateur.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IAuthService authService;

        public UserService(IUserRepository userRepository, IMailService mailService, IAuthService authService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            this.authService = authService;
        }

        public async Task<ReturnAddedUserManualy> AddUserManualyAsync(User _user)
        {
            var Errors = new Dictionary<string, string>();
            if (_user.PhoneNumber != null)
            {
                if (await _userRepository.DoesUserWithPhoneExist(_user.PhoneNumber, _user.IdAdmin))
                {
                    Errors.Add("PhoneNumber", "User with that Number already exist");
                }
            }
            if(_user.Email != null)
            {
                if (await _userRepository.DoesUserWithEmailExist(_user.Email, _user.IdAdmin))
                {
                    Errors.Add("Email", "User with that Email already exist");
                }
            }
            if(_user.Username != null)
            {
                if (await _userRepository.DoesUserWithUsernameExist(_user.Username, _user.IdAdmin))
                {
                    Errors.Add("Username", "User with that Username already exist");
                }
            }
            if(_user.FacebookId != null)
            {
                if (await _userRepository.DoesUserWithFacebookExist(_user.FacebookId, _user.IdAdmin)!=null)
                {
                    Errors.Add("Facebook", "User with that FacebookId already exist");
                }
            }
            if(_user.GoogleId != null)
            {
                if (await _userRepository.DoesUserWithGoogleExist(_user.GoogleId, _user.IdAdmin) != null)
                {
                    Errors.Add("Google", "User with that GoogleId already exist");
                }
            }
            if (Errors.Count == 0)
            {
                if (_user.ImageUrl == null)
                {
                    _user.ImageUrl = "https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg";
                }
                var user = await _userRepository.AddUserManualyAsync(_user);
                var AddedUsers = UserMapper.UserToAddedUser(user);
                if (user == null)
                {
                    AddedUsers.errors.Add("Server", "Some error happen while adding you, please try again later");
                    return AddedUsers;
                }
                return AddedUsers;

            }
            var AddedUser = UserMapper.UserToAddedUser(_user);
            AddedUser.errors = Errors;
            return AddedUser;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            UserMapper.UpdateUser(user, dto);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> UpdateSportDataAsync(int userId, UpdateSportDataDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            UserMapper.UpdateSportData(user, dto);

            await _userRepository.UpdateAsync(user);

            return true;
            //ajouter CDN
        }

        public async Task<ReturnForgotPasswordDTO> RecupererPasswodAsync(RecupererPasswordDTO dto)
        {
            // Recherche l'utilisateur par email
            var user = await _userRepository.GetByEmailAsync(dto.Email, dto.AdminId);
            if (user == null)
            {
                user = UserMapper.RecupererPasswod(dto);
                var Returnto = UserMapper.returnUpdatedPasswordDTO(user);
                Returnto.error = "Aucun utilisateur trouvé avec cet email";
                return Returnto;
            }
            var ReturnDto = UserMapper.returnUpdatedPasswordDTO(user);

            // Générer un nouveau mot de passe
            var nouveauMotDePasse = GenererNouveauMotDePasse();

            // Mise à jour du mot de passe dans l'objet utilisateur
            user.Password = nouveauMotDePasse;

            // Mise à jour dans la base de données
            await _userRepository.UpdateAsync(user);

            EmailRequest emailRequest= new EmailRequest(user.Email, nouveauMotDePasse, user.Nom + " " + user.Prenom);
            await _mailService.MailRecoverkey(emailRequest,user.IdAdmin);
            // Retourner true après une mise à jour réussie
            return ReturnDto;
        }

        private string GenererNouveauMotDePasse()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Vérification si l'utilisateur est bloqué
        public async Task<User> GetUserAsync(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            return user;
        }

        public async Task<bool> ResetConteurResAnnulerAsync(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.ConteurReservationAnnule = 0;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> CheckAndIncrementReservationAnnuleAsync(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("The user not found");

            // Vérifier si l'utilisateur est déjà bloqué
            if (user.IsReservationBlocked)
                return false;

            // Incrémenter le compteur d'annulations
            user.ConteurReservationAnnule++;
            if (user.ConteurReservationAnnule >= 2)
            {
                user.IsReservationBlocked = true;
                user.DateBlockedReservation = DateTime.UtcNow;
            }
            await _userRepository.UpdateAsync(user);
            return true;
        }
    
        public async Task<ReturnedLoginDto> Login(LoginDto login)
        {
            User user = await _userRepository.GetByEmailAsync(login.Email, login.AdminId);
            if(user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            if(user.Tentatives >= 2)
            {
                return null;
            }
            if(!user.Password.Equals(login.Password))
            {
                user.Tentatives = user.Tentatives + 1;
                await _userRepository.UpdateAsync(user);
                throw new UnauthorizedAccessException();
            }
            user.Tentatives = 0;
            await _userRepository.UpdateAsync(user);
            return UserMapper.ModelToLoginDto(user);

        }

        public async Task<ReturnedLoginDto> FacebookLoginAsync(string Id, int adminId)
        {
            User user =await _userRepository.DoesUserWithFacebookExist(Id, adminId);
            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            return UserMapper.ModelToLoginDto(user);
        }


        //----------- search user

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm,int id ,int AdminId)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm,id ,AdminId);

            // Mapper manuellement les entités vers les DTOs
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Nom = u.Nom,
                Prenom = u.Prenom,
                Username = u.Username,
                Email = u.Email,
                ImageUrl = u.ImageUrl,
                Bio = u.Bio
            }).ToList();
        }

        public async Task<ReturnedLoginDto> GoogleLoginAsync(string Id, int adminId)
        {
            User user = await _userRepository.DoesUserWithGoogleExist(Id, adminId);
            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            return UserMapper.ModelToLoginDto(user);
        }

        public async Task<ReturnedLoginDto> GetUserConfAsync(int Id)
        {
            User user = await _userRepository.GetByIdAsync(Id);
            if(user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return UserMapper.ModelToLoginDto(user);
        }

        public async Task<PaginatedResponse<paginationUser>> GetUsersPaginatedAsync(int skip, int limit, int adminId, bool? isBlocked = null, string searchTerm = null)
        {
            return await _userRepository.GetUsersAsync(skip, limit, adminId, isBlocked, searchTerm);
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isBlocked)
        {
            // Additional business logic can be added here
            var result = await _userRepository.UpdateUserStatusAsync(userId, isBlocked);

            return result;
        }

        public async Task<ReturnUpdated> updateUserAsync(User updatingUser)
        {
            User existingUser = await _userRepository.GetByIdAsync(updatingUser.Id);
            if(existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            ReturnUpdated dto = UserMapper.modelToUpdated(existingUser);
            if (updatingUser.Username != null && !updatingUser.Username.Equals(existingUser.Username))
            {
                if (await _userRepository.DoesUserWithUsernameExist(updatingUser.Username, updatingUser.IdAdmin))
                {
                    dto.Errors.Add("Username", "Username already exist.");
                }
            }
            if (updatingUser.PhoneNumber != null && !updatingUser.PhoneNumber.Equals(existingUser.PhoneNumber))
            {
                if (await _userRepository.DoesUserWithPhoneExist(updatingUser.PhoneNumber, updatingUser.IdAdmin))
                {
                    dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
                }
            }
            if (updatingUser.Email != null && !updatingUser.Email.Equals(existingUser.Email))
            {
                if (await _userRepository.DoesUserWithEmailExist(updatingUser.Email, updatingUser.IdAdmin))
                {
                    dto.Errors.Add("Email", "Email already exist.");
                }
            }
            if (dto.Errors.Count() == 0)
            {
                UserMapper.updateInsideUser(existingUser, updatingUser);
                // Sauvegarder les changements dans la base de données
                 await _userRepository.UpdateAsync(existingUser);
                ReturnedLoginDto auth = new ReturnedLoginDto
                {
                    Id = existingUser.Id,
                    Nom = updatingUser.Nom,
                    Prenom = updatingUser.Prenom,
                    Image = updatingUser.ImageUrl
                };
                await authService.UpdateTokenAsync(auth, existingUser.IdAdmin);
            }

            return dto;
        }
    }
}
