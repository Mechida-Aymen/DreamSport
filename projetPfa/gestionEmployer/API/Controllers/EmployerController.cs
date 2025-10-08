using gestionEmployer.API.DTOs.DTOs;
using gestionEmployer.API.Filters;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using gestionEmployer.API.Mappers;
using gestionEmployer.API.DTOs.EmployeeDTO;

namespace gestionEmployer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPasswordService _passwordService;


        public EmployeeController(IEmployeeService employeeService, IPasswordService passwordService)
        {
            _employeeService = employeeService;
            _passwordService = passwordService;

        }


        // GET: api/Employee/{id}
        [HttpGet("get/{id}/{AdminId}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cant be < 0");
            }
            Employer employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            GetEmployeeDTO dto = EmployeeMapper.ModelToGetEmployee(employee);
            return Ok(dto);
        }

        //GET ALL EMPLOYEES by idAdmin
        [HttpGet("admin/{AdminId}")]
        public async Task<ActionResult<IEnumerable<ReturnAddedEmployee>>> GetEmployesByAdminId(int AdminId)
        {
            var employes = await _employeeService.GetEmployesByAdminIdAsync(AdminId);
            if (employes == null || employes.Count()==0)
            {
                return NotFound($"Aucun employé trouvé pour l'ID Admin : {AdminId}");
            }

            return Ok(employes);
        }



        // POST: api/Employee
        [HttpPost]
        [ValidationModels]
        public async Task<ActionResult<ReturnAddedEmployee>> AddEmployee([FromBody] AddEmployeeDTO employee)
        {
            
            Employer employerr = EmployeeMapper.AddEmployeeDTOToEmployer(employee);

            var employeeAjoute = await _employeeService.AddEmployeeAsync(employerr);
            if (employeeAjoute.errors.Count()>0)
            {
                return BadRequest(employeeAjoute);
            }
            return Ok(employeeAjoute);
        }

       
        // PUT: api/Employee/{id}
        [HttpPut]
        [ValidationModels]
        public async Task <IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDTO employee)
        {
            Employer employer = EmployeeMapper.UpdateEmployeeDTOToEmployer(employee);

            try
            {
                ReturnUpdatedEmpDto emp = await _employeeService.UpdateEmployeeAsync(employer);
                if(emp.Errors.Count()>0)
                {
                    return BadRequest(emp);
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}/{AdminId}")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeService.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }


        [HttpPost("validate")]
        public async Task<IActionResult> ValidateEmployer([FromBody] EmployerLoginDto dto)
        {
            try
            {
                SendLoginEmployeeDto employee = await _employeeService.ValidateLogin(dto);
                if (employee == null)
                {
                    return Unauthorized("Email or password are incorrect");
                }
                return Ok(employee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }

        }

        [HttpGet("search/{searchTerm}/{AdminId}")]
        public async Task<ActionResult<IEnumerable<GetEmployeeDTO>>> SearchUsersAsync(string searchTerm)
        {
            var result = await _employeeService.SearchEmployeesAsync(searchTerm);
            if(result == null)
            {
                return NotFound("No employee found with this pattern");
            }
            return Ok(result);
        }

        //change password
        [HttpPut ("changePassword")]
        public async Task<IActionResult> ChangePassword( [FromBody] ChangePasswordDto ChangePasswordDto )
        {
            try
            {
                // Vérification de l'ancien mot de passe
                var isValid = await _passwordService.VerifyOldPassword(ChangePasswordDto.AdminId, ChangePasswordDto.EmployerId, ChangePasswordDto.OldPassword);

                if (!isValid)
                    return BadRequest();

                // Changement du mot de passe
                await _passwordService.ChangePassword(ChangePasswordDto.AdminId, ChangePasswordDto.EmployerId, ChangePasswordDto.NewPassword);

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpPut("recover-password")]
        public async Task<IActionResult> RecoverPasswordAsync([FromBody] recoverPass dto)
        {
            // Appel au service
            var userDto = await _employeeService.RecupererPasswodAsync(dto);

            if (userDto.error != null)
            {
                return BadRequest(userDto);
            }
            return Ok(userDto);
        }

        

    }
}
