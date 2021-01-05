using Microsoft.EntityFrameworkCore;

namespace DoctorsOffice.Models 
{
  public class DoctorsOfficeContext : DbContext
  {
    public virtual DbSet<Doctor> Doctors { get; set; }
    public Dbset<Patient> Patients { get; set; }
    public DbSet<DoctorPatient> DoctorPatient { get; set; }
    public DoctorsOfficeContext(DbContextOptions options) : base(options) { }
  }
}