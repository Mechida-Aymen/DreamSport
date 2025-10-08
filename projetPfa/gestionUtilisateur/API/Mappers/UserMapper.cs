using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.API.Mappers
{
    public class UserMapper
    {
        public static User AddUserToUser(AddUserManualyDTO dto)  //to pass data from controller to service layer
        {

            User user = new User();

            user.Nom = dto.Nom;
            user.Prenom = dto.Prenom;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.PhoneNumber = dto.PhoneNumber;
            user.Username = dto.Username;
            if (dto.Birthday!=null)
            {
                user.Birthday = dto.Birthday;

            }
            user.Genre = dto.Genre;
            user.IdAdmin = dto.AdminId;

            return user;
        }

        public static ReturnAddedUserManualy UserToAddedUser(User user) {
            ReturnAddedUserManualy usr = new  ReturnAddedUserManualy
            {
                Nom = user.Nom,
                Prenom = user.Prenom,
                AdminId = user.IdAdmin,
                Birthday = user.Birthday,
            };
            if (user.Email != null)
            {
                usr.Email = user.Email;
            }
            if (user.Password != null)
            {
                usr.Password = user.Password;
            }
            if (user.PhoneNumber != null)
            {
                usr.PhoneNumber = user.PhoneNumber;
            }
            if (user.Username != null)
            {
                usr.Username = user.Username;
            }
            if (user.Genre != null)
            {
                usr.Genre = user.Genre;
            }
            return usr;
        }
        public static void UpdateUser(User user, UpdateUserDto dto)
        {
            if (dto.Nom != null)
            {
                user.Nom = dto.Nom;
            }

            if (dto.Prenom != null)
            {
                user.Prenom = dto.Prenom;
            }

            if (dto.Email != null)
            {
                user.Email = dto.Email;
            }

            if (dto.Password != null)
            {
                user.Password = dto.Password;
            }
            if (dto.Birthday != null)
            {
                user.Birthday = dto.Birthday;
            }

            if (dto.PhoneNumber != null)
            {
                user.PhoneNumber = dto.PhoneNumber;
            }

            if (dto.Username != null)
            {
                user.Username = dto.Username;
            }
            if (dto.bio != null) { 
                user.Bio=dto.bio;
            }

        }
        public static void UpdateSportData(User user, UpdateSportDataDTO dto)
        {
            if (dto.Bio != null)
            {
                user.Bio = dto.Bio;
            }

            if (dto.ImageUrl != null)
            {
                user.ImageUrl = dto.ImageUrl;
            }

        }

        //Recup password
        public static User RecupererPasswod(  RecupererPasswordDTO dto)
        {
            return new User {

                Email = dto.Email,
                IdAdmin = dto.AdminId,
            };
        }

        public static ReturnForgotPasswordDTO returnUpdatedPasswordDTO(User user)
        {
            return new ReturnForgotPasswordDTO
            {
                Email = user.Email,
                AdminId = user.IdAdmin
            };
        }


        public static ReturnedLoginDto ModelToLoginDto(User user)
        {
            return new ReturnedLoginDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Image = user.ImageUrl
            };
        }

        public static User FacebookToUser(FacebookUserDto dto)
        {
            
            string gender = "male";
            if (dto.Gender == null)
            {

            }
            else if (dto.Gender.Equals("female"))
            {
                 gender = "female";
            }
            User user = new User
            {
                Nom = dto.LastName,
                Prenom = dto.FirstName,
                Genre = gender,
                Email = dto.Email,
                ImageUrl = dto.PictureUrl,
                IdAdmin = dto.AdminId
            };

            if (dto.type == "facebook")
            {
                user.FacebookId = dto.FacebookId;
            }
            else
            {
                user.GoogleId = dto.FacebookId;
            }
            return user;
        }

        public static paginationUser modelTopagination(User user)
        {
            return new paginationUser
            {
                id = user.Id,
                firstName = user.Prenom,
                lastName = user.Nom,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                imageUrl = user.ImageUrl,
                isBlocked = user.IsReservationBlocked,
                username = user.Username,
            };
        }

        public static User updateToUser(UpdateUser dto)
        {
            return new User
            {
                Id = dto.Id,
                Nom = dto.lastname,
                Prenom = dto.firstname,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Username = dto.Username,
                ImageUrl= dto.ImageUrl, 
                IdAdmin = dto.AdminId,
                Bio=dto.bio,
            };
        }

        public static ReturnUpdated modelToUpdated(User user)
        {
            return new ReturnUpdated
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username= user.Username,
                imageUrl= user.ImageUrl,
                AdminId = user.IdAdmin,
                bio=user.Bio,
                
            };
        }
        
        public static void updateInsideUser(User user, User userr)
        {
            if(userr.Nom != null)
            {
                user.Nom = userr.Nom;
            }
            if (userr.Prenom != null)
            {
                user.Prenom = userr.Prenom;
            }
            if (userr.Email != null)
            {
                user.Email = userr.Email;
            }
            if (userr.PhoneNumber != null)
            {
                user.PhoneNumber = userr.PhoneNumber;
            }
            if (userr.Username != null)
            {
                user.Username = userr.Username;
            }
            if (userr.ImageUrl != null)
            {
                user.ImageUrl = userr.ImageUrl;
            }
            if (userr.Bio != null)
            {
                user.Bio = userr.Bio;
            }
        }
    }
}
