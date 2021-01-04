using Microsoft.EntityFrameworkCore;

namespace DoctorsOffice.Models
{
    public class DoctorsOfficeContext : DbContext
    {
        public DoctorsOfficeContext(DbContextOptions options) : base(options) { }
    }
}