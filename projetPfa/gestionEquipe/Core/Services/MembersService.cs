using gestionEquipe.API.DTOs;
using gestionEquipe.API.Exceptions;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Infrastructure.ExternServices.ExternDTO;

namespace gestionEquipe.Core.Services
{
    public class MembersService : IMembersService 
    {
        private readonly IMembersRepository _membersRepository;
        private readonly IEquipeRepository _equipeRepository;
        private readonly ISiteService _siteService;

        public MembersService(IMembersRepository membersRepository, IEquipeRepository equipeRepository, ISiteService siteService)
        {
            _membersRepository = membersRepository;
            _equipeRepository = equipeRepository;
            _siteService = siteService;
        }

        public async Task<Members> KickMemberAsync(Members member)
        {
            if (await _equipeRepository.ExistWithIdAsync(member.EquipeId)==null)
            {
                throw new KeyNotFoundException("Team not found.");
            }

            if(!await _membersRepository.ExistInTeamAsync(member))
            {
                throw new KeyNotFoundException("Member not found in this team.");
            }

            
            var deletedMember = await _membersRepository.KickMemberAsync(member);
            return deletedMember;
        }    

        public async Task<Members> AjouterMemberAsync(Members member)
        {
            Equipe equipe = await _equipeRepository.ExistWithIdAsync(member.EquipeId);
            if (equipe == null)
            {
                throw new KeyNotFoundException("Team not found.");
            }

            if (await _membersRepository.ExistInTeamAsync(member))
            {
                throw new BadRequestException("Member is already part of this team.");
            }
            int CountTeams = await _membersRepository.CountTeamsForMemberAsync(member.UserId);
            if (CountTeams >=2 )
            {
                throw new InvalidOperationException("Member is already in another team.");
            }
            List<SportCategorieDTO> sports = await _siteService.GetSportsAsync(equipe.AdminId);
            int MaxVolume = sports.FirstOrDefault(s => s.Id == equipe.SportId).NombreMax;
            if(await _membersRepository.CountTeamMembersAsync(member.EquipeId) >= MaxVolume)
            {
                throw new InvalidOperationException("This team is full");
            }
            return await _membersRepository.AddMemberSaveAsync(member);
        }     
    }
}
