using gestionSite.API.DTOs.FAQDtos;
using gestionSite.API.Mappers;
using gestionSite.Core.Interfaces.FAQ;
using gestionSite.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionSite.API.Filters;

namespace gestionSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet("{adminId}")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQsAsync(int adminId)
        {
            // Validate the adminId parameter
            if (adminId <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }
            // Retrieve FAQs using the service
            var faqs = await _faqService.GetFAQsByAdminAsync(adminId);

            // Handle null or empty result
            if (faqs == null || !faqs.Any())
            {
                return NotFound($"No FAQs found for admin with ID {adminId}.");
            }
            return Ok(faqs);
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<FAQ>> AddFAQAsync([FromBody] AddFAQDto AddFaq)
        {
            var faq = FAQMapper.AddFAQDtoToFAQ(AddFaq);
            var result = await _faqService.AddFAQAsync(faq);
            if (result == null)
            {
                return BadRequest(error:"This question already exists");
            }
            return Ok(result);
            //return Created("/api/faq/" + result.IdAdmin, result);
        }

        [HttpPut]
        [ValidateModelAttribute]
        public async Task<ActionResult<FAQ>> UpdateFAQAsync([FromBody] UpdateFAQDto UpdateFaq)
        {
            var faq = FAQMapper.UpdateFAQDtoToFAQ(UpdateFaq);
            var result = await _faqService.UpdateFAQAsync(faq);
            if (result == null)
            {
                return BadRequest("Failed to update FAQ");
            }

            return Ok(result);
        }

        [HttpDelete("{faqId}/{AdminId}")]
        public async Task<ActionResult<FAQ>> DeleteFAQAsync(int faqId)
        {
            if (faqId < 0)
            {
                return BadRequest("the Id must be positive");
            }
            var result = await _faqService.DeleteFAQAsync(faqId);
            if (result == null)
            {
                return NotFound("FAQ not found");
            }

            return Ok(result);
        }
    }
}
