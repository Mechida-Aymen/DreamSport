using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Infrastructure.Data.Repositories;
using gestionEquipe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Shared.Messaging.Services;

namespace gestionEquipe.Core.Services
{
    public class EquipeService : IEquipeService

    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ISiteService _siteService;
        private readonly IMembersRepository _membersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EquipeService(IEquipeRepository equipeRepository,ISiteService siteService,IMembersRepository membersRepository,IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) {
            _equipeRepository = equipeRepository;
            _siteService = siteService;
            _membersRepository = membersRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<AddedEquipeDTO> AddEquipeAsync(Equipe _equipe)
        {
     
            int c = await _membersRepository.CountTeamsForMemberAsync(_equipe.CaptainId);
            
            var ReturningEquipe = EquipeMapper.ModelToAdded(_equipe);
            if(c > 1)
            {
                ReturningEquipe.Errors.Add("Count", "This user had more than one team");
            }
            if(await _equipeRepository.ExistWithName(_equipe.Name, _equipe.AdminId))
            {
                ReturningEquipe.Errors.Add("Name", "A team with this name already exist");
            }
            var sports = await _siteService.GetSportsAsync(_equipe.AdminId);
            if (!sports.Select(s => s.Id).Contains(_equipe.SportId)) 
            {
                ReturningEquipe.Errors.Add("Sport", "Sport with this id dont exist");
            }
            

            if (ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }
            
            
             await AddEquipeWithMemberAsync(_equipe);

            var _producer = new RabbitMQProducerService("Create chat equipe");
            _producer.Publish(new { teamId = _equipe.Id, AdminId = _equipe.AdminId });

            return ReturningEquipe;

        }
        public async Task<EquipeDto> GetEquipe(int IdEquipe)
        {
            var equipe = await _equipeRepository.GetEquipeById(IdEquipe);
            
            
            if (equipe == null) throw new KeyNotFoundException("Team not found");

            var membres = await _membersRepository.GetTeamMembersAsync(equipe.Id);

            var memberDtos = membres.Select(m => new MembreDto
            {
                UserId = m.UserId,
                EquipeId = m.EquipeId
            }).ToList();

            return new EquipeDto
            {
               Id = equipe.Id ,
               Name =equipe.Name ,
               Description=equipe.Description ,
               Avatar=equipe.Avatar ,
               CaptainId=equipe.CaptainId ,
               Membres= memberDtos,
               SportId=equipe.SportId

            };

        }


        private async Task<Equipe> AddEquipeWithMemberAsync(Equipe equipe)
        {
            // Start the transaction if necessary
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Add both the Equipe and the Member
                equipe = await _equipeRepository.AddEquipeAsync(equipe);
                Members member = new Members
                {
                    UserId = equipe.CaptainId,
                    EquipeId = equipe.Id,
                };
                await _membersRepository.AddMemberAsync(member);

                await _unitOfWork.SaveChangesAsync();  // Save changes

                await _unitOfWork.CommitAsync();  // Commit transaction
                return equipe;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();  // Rollback if any errors occur
                throw;
            }
        }

        public async Task SupprimerEquipeAvecMembresAsync(int equipeId)
        {
            var equipe = await _equipeRepository.GetEquipeById(equipeId);
            if (equipe == null) throw new KeyNotFoundException("Team not found");
            await _equipeRepository.SupprimerEquipeAvecMembresAsync(equipeId);
        }

        

        public async Task<UpdatedEquipeDTO> UpdateEquipeAsync(Equipe _equipe) {

            var ReturningEquipe = EquipeMapper.EquipetoUpdatedEquipeDTO(_equipe);
            Equipe existingEquipe = await _equipeRepository.GetEquipeById(_equipe.Id);

            if (existingEquipe == null)
            {
                ReturningEquipe.Errors.Add("Count", "Team not exist");

                return ReturningEquipe;
            }

            // Update the properties of the existing equipe

            if (_equipe.Name != null)
            {
                existingEquipe.Name = _equipe.Name;
            }

            if (_equipe.Description != null)
            {
                existingEquipe.Description = _equipe.Description;
            }

            if (_equipe.Avatar != null)
            {
                existingEquipe.Avatar = _equipe.Avatar;
            }

            if (_equipe.CaptainId > 0)
            {
                existingEquipe.CaptainId = _equipe.CaptainId;
            }

            if (ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }

            
             var UpdatedEquipe = await _equipeRepository.UpdateEquipeAsync(existingEquipe);

            return EquipeMapper.EquipetoUpdatedEquipeDTO(UpdatedEquipe);



        }
        public async Task<UpdatedEquipeDTO> TransferCaptaincyAsync(Equipe _equipe)
        {
            var ReturningEquipe = EquipeMapper.EquipeChangedCapitain(_equipe);

            if (!await _membersRepository.ExistInTeamWithIdAsync(_equipe.CaptainId,_equipe.Id))
            {
                ReturningEquipe.Errors.Add("Count", "This user not exist in this team");
            }
            var equipe = await _equipeRepository.GetEquipeById(_equipe.Id);

            if (equipe == null)
            {
                ReturningEquipe.Errors.Add("Count", "This team not exist");
            }

            if (ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }

            equipe.CaptainId = _equipe.CaptainId;

            var UpdatedEquipe = await UpdateEquipeAsync(equipe);

            

            return UpdatedEquipe;



        }

        public async Task<bool> CapitainQuitteAsync(int capitaineId, int equipeId)
        {
            Equipe equipe = await _equipeRepository.GetEquipeById(equipeId);
            // Vérifier si l'utilisateur est bien le capitaine de l'équipe
            bool estCapitaine = await _equipeRepository.IsCaptainAsync(capitaineId, equipeId);
            if (!estCapitaine)
                throw new UnauthorizedAccessException("User is blocked and cannot perform this action.");

            // Récupérer la liste des membres de l'équipe
            List<Members> membresIds = await _membersRepository.GetTeamMembersAsync(equipeId);
            if (membresIds == null || membresIds.Count == 1)
                throw new InvalidOperationException("No members available to select a captain.");

            membresIds = membresIds.Where(e => e.UserId != capitaineId).ToList();

            // Sélectionner aléatoirement un nouveau capitaine
            Random random = new Random();
            int nouveauCapitaineId = membresIds[random.Next(membresIds.Count)].UserId; 

            equipe.CaptainId = nouveauCapitaineId;
            // Mettre à jour l'équipe avec le nouveau capitaine
            Equipe updateSuccess = await _equipeRepository.UpdateEquipeAsync(equipe);
            if (updateSuccess == null)
                throw new Exception("Erreur lors de la mise à jour du capitaine.");

            // Supprimer l'ancien capitaine de l'équipe
            Members ancienCapitaine = new Members { UserId = capitaineId, EquipeId = equipeId, Equipe = null };
            await _membersRepository.KickkMemberAsync(ancienCapitaine);

            return true;
        }

        public async Task<IEnumerable<int>> GetTeamAsync(int teamId)
        {
            if(await _equipeRepository.ExistWithIdAsync(teamId) == null)
            {
                throw new KeyNotFoundException("Team dont exist");
            }
            IEnumerable<Members> list = await _membersRepository.GetTeamMembersAsync(teamId);
            List<int> memberIds = list.Select(m => m.UserId).ToList();
            return memberIds;
        }

        //----------------
        public async Task<UserTeamMembershipResponseDto> CheckUserTeamMembershipAsync(int userId, int adminId)
        {
            var membre = await _equipeRepository.GetUserTeamMembershipAsync(userId, adminId);



            if (membre == null)
            {
                return new UserTeamMembershipResponseDto
                {
                    IsMember = false,
                    EquipeId = null,
                    EquipeNom = null,
                    IsCapitaine = false,
                    UserId= userId,
                };
            }

            var membres = await _membersRepository.GetTeamMembersAsync(membre.EquipeId);

            var memberDtos = membres.Select(m => new MembreDto
            {
                UserId = m.UserId,
                EquipeId = m.EquipeId 
            }).ToList();

            if (membre.Equipe.CaptainId==userId)
            {
                return new UserTeamMembershipResponseDto
                {
                    IsMember = true,
                    EquipeId = membre.EquipeId,
                    EquipeNom = membre.Equipe.Name,
                    IsCapitaine = true,
                    UserId = userId,
                    Members= memberDtos,


                };
            }
            return new UserTeamMembershipResponseDto
            {
                IsMember = true,
                EquipeId = membre.EquipeId,
                EquipeNom = membre.Equipe.Name,
                IsCapitaine = false,
                UserId = userId,


            };
        }

    }
}
