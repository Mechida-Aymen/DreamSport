using gestionReservation.API.DTOs;
using gestionReservation.Core.Models;

namespace gestionReservation.API.Mappers
{
    public class ReservationMapper
    {
        public static AddReservationDto ModelToAddDTO(Reservation reservation)
        {
            return new AddReservationDto
            {
                DateRes = reservation.DateRes,
                IdUtilisateur = reservation.IdUtilisateur,
                IdTerrain = reservation.IdTerrain,
                AdminId = reservation.IdAdmin,
            };
        }

        public static Reservation AddDTOtoModel(AddReservationDto dto)
        {
            return new Reservation
            {
                DateRes = dto.DateRes,
                IdAdmin = dto.AdminId,
                IdTerrain = dto.IdTerrain,
                IdUtilisateur = dto.IdUtilisateur,

            };
        }
        public static Reservation GetReservationDTOToReservation(GetReservationsDTO dto)
        {
            return new Reservation
            {
                DateRes = dto.StartDate ?? DateTime.Now, // Met la date actuelle si StartDate est null
            };
        }
        public static List<ReturnedListReservationsDTO> ReservationsToReservationsDTOs(List<Reservation> reservations)
        {
            return reservations.Select(reservation => new ReturnedListReservationsDTO
            {
                Id = reservation.Id,
                DateRes = reservation.DateRes,
                IdEmploye = reservation.IdEmploye,
                IdTerrain = reservation.IdTerrain,
                IdStatus = reservation.IdStatus,
                IdAdmin = reservation.IdAdmin
            }).ToList();
        }

    }
}
