using gestionSite.API.DTOs.FAQDtos;
using gestionSite.API.DTOs.SiteDtos;
using gestionSite.API.Filters;
using gestionSite.API.Mappers;
using gestionSite.Core.Interfaces.FAQ;
using gestionSite.Core.Interfaces.SiteInterfaces;
using gestionSite.Core.Models;
using gestionSite.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService SiteService)
        {
            _siteService = SiteService;
        }

        [HttpGet("{adminId}")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetSiteInfosAsync(int adminId)
        {
            // Validate the adminId parameter
            if (adminId <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }
            // Retrieve FAQs using the service
            var site = await _siteService.GetSiteByAdminAsync(adminId);

            // Handle null or empty result
            if (site == null || !site.Any())
            {
                return NotFound($"No site infos found for admin with ID {adminId}.");
            }
            return Ok(site);
        }
        [HttpGet("name/{adminId}")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetSitenameAsync(int adminId)
        {
            // Validate the adminId parameter
            if (adminId <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }
            // Retrieve FAQs using the service
            Site site = await _siteService.GetSiteASync(adminId);

            // Handle null or empty result
            if (site == null )
            {
                return NotFound($"No site infos found for admin with ID {adminId}.");
            }
            return Ok(site);
        }

        [HttpPut]
        [ValidateModelAttribute]
        public async Task<ActionResult<FAQ>> UpdateSite([FromBody] UpdateSiteDto _updateSiteDto)
        {
            var site = SiteMapper.UpdateSiteDtoToSite(_updateSiteDto);
            try
            {
                var result = await _siteService.UpdateSiteAsync(site);
                if (result == null)
                {
                    return StatusCode(500,new { errorMessage="something happen whilee updating the site" });
                }
                if(result.Errors.Count > 0)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }catch (KeyNotFoundException ex)
            {
                return NotFound(new { errorMessage = ex.Message });
            }catch (Exception ex)
            {
                return StatusCode(500, new { errorMessage = "some error haapen while updating the site" });
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<FAQ>> AddFAQAsync(int Idadmin)
        {
           
            var result = await _siteService.AddSiteAsync(Idadmin);
            if (result == null)
            {
                return BadRequest("An error occurred while adding the site");
            }

            return Created("/api/Site/" + result.IdAdmin, result);
        }



    }
}
