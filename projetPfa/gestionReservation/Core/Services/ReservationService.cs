using gestionReservation.API.DTOs;
using gestionReservation.Core.Interfaces;
using gestionReservation.Core.Models;
using gestionReservation.API.Exceptions;
using System.Net.Http;
using System.Text.Json;
using gestionReservation.Infrastructure.Data.Repositories;
using gestionReservation.API.Mappers;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ISiteService _siteService;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    public ReservationService(IReservationRepository reservationRepository,IMailService mailService, IStatusRepository statusRepository, IUserService userService, ISiteService siteService)
    {
        _reservationRepository = reservationRepository;
        _statusRepository = statusRepository;
        _userService = userService; 
        _mailService = mailService;
        _siteService = siteService;
    }

    public async Task<AddReservationDto> AjouterReservationAsync(Reservation reservation)
    {
        if(reservation.DateRes < DateTime.Now)
        {
            throw new BadRequestException("The reservation date must be today or in the future.");
        }
        var user = await _userService.FetchUserAsync(reservation.IdUtilisateur ,reservation.IdAdmin);
        if(user == null )
        {
            throw new KeyNotFoundException("The user dont exist in our sytem");
        }
        if(user.IsReservationBlocked)
        {
            throw new UnauthorizedAccessException("You cant Reserve please talk to the support");
        }
        var terrain = await _siteService.FetchTerrainAsync(reservation.IdTerrain,reservation.IdAdmin);
        if(terrain == null)
        {
            throw new KeyNotFoundException("The terrain dont exist in our sytem");
        }
        if(terrain.terrainStatus.Libelle!= "Field ready for reservation")
        {
            throw new BadRequestException("This terrain cant be reserved at the moment");
        }
        if (await _reservationRepository.GetReservationsCountByTerrainAndDateAsync(reservation.IdTerrain, reservation.DateRes) > 0)
        {
            throw new BadRequestException("This terrain is reserved at the date provided");
        }
        Status st = await _statusRepository.GetStatusByLibelle("Pending");
        int idStatus = st.Id;
        reservation.IdStatus = idStatus;
        reservation.Status = st;

        await _reservationRepository.AddAsync(reservation);//add reservation
        
        var dto = ReservationMapper.ModelToAddDTO(reservation);
        return dto;
    }
    public async Task<List<ReturnedListReservationsDTO>> GetReservationsAsync( GetReservationsDTO filter)
    {
     

        var reservations = await _reservationRepository.GetReservationsAsync(filter.StartDate, filter.EndDate);
        List < ReturnedListReservationsDTO > reservationdto = ReservationMapper.ReservationsToReservationsDTOs(reservations);
        return reservationdto;
    }


    public async Task<Reservation> ReservationStatusUpdateAsync(UpdateStatusDTO dto)
    {
        Reservation reservation = await _reservationRepository.GetByIdAsync(dto.Id);
        if (reservation == null)
        {
            throw new KeyNotFoundException("The reservation not found ");
        }
        UserDTO user =await _userService.FetchUserAsync(reservation.IdUtilisateur, reservation.IdAdmin);
        if (user == null)
        {
            throw new KeyNotFoundException("The user not found ");
        }
        reservation.IdEmploye = dto.EmployeeId;
        if (dto.Status == "Confirmed")
        {
            if (!await _userService.ResetConteurResAnnulerAsync(reservation.IdUtilisateur, reservation.IdAdmin))
            {
                throw new Exception("Failed to reset compteur");
            }
            Status st = await _statusRepository.GetStatusByLibelle("Confirmed");
            reservation.IdStatus = st.Id;
        }
        else if (dto.Status == "Canceled")
        {
            if (!await _userService.CheckAndIncrementReservationAnnuleAsync(reservation.IdUtilisateur, reservation.IdAdmin))
            {
                throw new Exception("Failed to update the user compteur");
            }
            Status st = await _statusRepository.GetStatusByLibelle("Canceled");
            reservation.IdStatus = st.Id;
        }
        else
        {
            throw new BadRequestException("We dont have a status named : " + dto.Status);
        }
        Reservation res = await _reservationRepository.UpdateReservationAsync(reservation);
        if(res != null && dto.Status == "Confirmed")
        {
            SiteDto site = await _siteService.GetSiteInfosAsync(reservation.IdAdmin);
            if(site != null)
            {
                EmailRequest mail = new EmailRequest();
                mail.SendReservationConfirmationToUser(user.Email, user.Nom + "" + user.Prenom, site.Name, reservation.DateRes);
                await _mailService.SendMailAsync(mail, reservation.IdAdmin);
            }
        }
        return res;

    }
    //---------------
    public async Task<List<ReservationDto>> GetReservationsAsync(DateTime startDate, DateTime endDate, int idTerrain)
    {
        var reservations = await _reservationRepository.GetReservationsAsync(startDate, endDate, idTerrain);

        var reservationDtos = new List<ReservationDto>();

        foreach (var reservation in reservations)
        {
            var terrain = await _siteService.FetchTerrainAsync(reservation.IdTerrain, reservation.IdAdmin);
            if (terrain == null)
            {
                throw new KeyNotFoundException($"The terrain with ID {reservation.IdTerrain} does not exist in our system.");
            }

        
                reservationDtos.Add(new ReservationDto
                {
                    Id = reservation.Id,
                    DateRes = reservation.DateRes,
                    IdUtilisateur = reservation.IdUtilisateur,
                    IdTerrain = reservation.IdTerrain,
                    IdEmploye = reservation.IdEmploye,
                    IdAdmin = reservation.IdAdmin,
                    StatusLibelle = reservation.Status?.Libelle // Inclure le libellé du statut
                });
            

         
        }

        return reservationDtos;
    }

    public async Task<IEnumerable<Reservation>> GetRequestsListAsync(int AdminId){
        DateTime startDate = DateTime.Now;  
        DateTime endDate = startDate.AddDays(7);
        return await _reservationRepository.GetRequestsListAsync(AdminId, startDate, endDate); 

    }

}
