using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using gestionEmployer.Core.Interfaces;
using System.Linq.Expressions;

namespace gestionEmployer.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<Employer> EmployerByEmailAsync(string email, int adminId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.AdminId == adminId && e.Email == email);
        }
        public async Task<Employer> EmployerByCINAsync(string cin, int adminId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.AdminId == adminId && e.CIN == cin);
        }
        public async Task<Employer> EmployerByUsernameAsync(string username, int adminId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.AdminId == adminId && e.Username == username);
        }
        public async Task<Employer> EmployerByPhoneAsync(string phone, int adminId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.AdminId == adminId && e.PhoneNumber == phone);
        }

        public  async Task<Employer?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employers.FindAsync(id);
            return employee;
        }

        //Méthode de récuperation de tous les employees by idAdmin
        public async Task<IEnumerable<Employer>> GetEmployesByAdminIdAsync(int idAdmin)
        {
            return await _context.Employers.Where(e => e.AdminId == idAdmin).ToListAsync();
        }

        //Methode Existe qui se surcharge dans le service 
        public bool Exists(Expression<Func<Employer, bool>> predicate)
        {
            return _context.Employers.Any(predicate);
        }

        public async Task<Employer> AddEmployeeAsync(Employer employee)
        {
             _context.Employers.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }



        public async Task<Employer?> UpdateEmployeeAsync(Employer employee)
        {
            var trackedEntity = _context.Employers.Local.FirstOrDefault(e => e.Id == employee.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached; // Detach the old entity
            }

            _context.Employers.Attach(employee);
            _context.Entry(employee).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return employee;
        }


        public async Task<Employer?> DeleteEmployeeAsync(int id)
        {
            var employee = _context.Employers.Find(id);
            if (employee != null)
            {
                _context.Employers.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return employee;
        }

        public async Task<List<Employer>> SearchEmployeesAsync(string searchTerm)
        {
            var terms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return await _context.Employers
                .Where(u =>
                    terms.All(term =>
                        u.Username.Contains(term) ||
                        u.Nom.Contains(term) ||
                        u.Prenom.Contains(term) 
                        
                    )
                )
                .ToListAsync();
        }


    }
}
