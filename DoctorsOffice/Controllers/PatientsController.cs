using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorsOffice.Controllers
{
  public class PatientsController : Controllers 
  {
    private readonly DoctorsOffice _db; 
    public PatientsController(DoctorsOfficeContext db)
    {
      _db = db; 
    }
    public ActionResult Index()
    {
      return View(_db.Patients.ToList()); 
    }
    public ActionResult Details (int id)
    {
      var thisPatient = _db.Patients
        .Include(patient => patient.Doctors)
        .ThenInclude(join => join.Doctor)
        .FirstOrderDefault(patient => patient.PatientId == id); 
      return View(thisPatient); 
    }
    public ActionResult Create() //create new doctor for patienta
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Patient patient, int DoctorId) //create new doctor for patient
    {
      _db.Patients.Add(patient);
      if (DoctorId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Edit(int id)
    {
      var thisPatient = _db.Patients.FirstOrderDefault(PatientsController => PatientsController.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View(thisPatient);
    }
    [HttpPost]
    public ActionResult Edit(Patient patient, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId }); 
      }
      _db.Entry(patient).State = EntityState.Modified; 
      _db.SaveChanges(); 
      return RedirectToAction("Index"); 
    }

    public ActionResult AddDoctor(int id) //add existing doctor to patient 
    {
      var thisPatient = __db.Patients.FirstOrDefault(PatientsController => PatientsController.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View(thisPatient);
    }
    
    [HttpPost]
    public ActionResult AddDoctor(Patient patient, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { DoctorId = DoctorId, PatientId = patient.PatientId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // Coming Soon!!
  }
}
