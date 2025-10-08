using gestionReservation.API.DTOs;
using gestionReservation.API.Exceptions;
using gestionReservation.API.Mappers;
using gestionReservation.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestionReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet("{AdminId}")]
        public async Task<ActionResult<List<ReturnedListReservationsDTO>>> GetReservations([FromQuery] GetReservationsDTO filter)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsAsync(filter);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AjouterReservationAsync([FromBody] AddReservationDto reservation)
        {
            try
            {
                Reservation res = ReservationMapper.AddDTOtoModel(reservation);
                var result = await _reservationService.AjouterReservationAsync(res);
                return Ok(result); // Return HTTP 200 with the created reservation
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    reservation = reservation
                }); // HTTP 400
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new
                {
                    message = ex.Message,
                    reservation = reservation
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message,
                    reservation = reservation
                }); // HTTP 404
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message,
                    reservation = reservation
                }); // HTTP 500
            }
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateReservationStatus([FromBody] UpdateStatusDTO dto)
        {
            try
            {
                Reservation reservation = await _reservationService.ReservationStatusUpdateAsync(dto);
                return Ok(reservation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //---------------
        [HttpGet("upcoming/{idTerrain}/{AdminId}")]
        public async Task<ActionResult<List<ReservationDto>>> GetUpcomingReservationsByTerrainAsync(int idTerrain)
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(7);

            var result = await _reservationService.GetReservationsAsync(startDate, endDate, idTerrain);
            return Ok(result);
        }

        [HttpGet("requests-list/{AdminId}")]
        public async Task<IActionResult> GetRequestsListAsync(int AdminId)
        {
            try
            {
                IEnumerable<Reservation> listRequests = await _reservationService.GetRequestsListAsync(AdminId);
                if(listRequests == null)
                {
                    return NotFound(new {errorMessage = "No requests Found"});
                }
                return Ok(listRequests);
            }catch(Exception ex)
            {
                return StatusCode(500, "An error happen while performing your request");
            }
        }
    }

}
