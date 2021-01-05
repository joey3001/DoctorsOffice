using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorsOffice.Controllers
{
  public class DoctorsController : Controller
  {
    private readonly DoctorsOfficeContext _db;

    public DoctorsController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Doctor> model = _db.Doctors.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Doctor doctor)
    {
      _db.Doctors.Add(doctor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    } 

    public ActionResult Details(int id)
    {
      var thisDoctor = _db.Doctors
        .Include(doctor => doctor.Patients)
        .ThenInclude(join => join.Patient)
        .FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }
    public ActionResult Edit(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctors => doctors.DoctorId == id );
      ViewBag.PatientId = new SelectList(_db.Patients, "PatientId", "Name", "Birthday");
      return View(thisDoctor);
    }
    
    [HttpPost]
    public ActionResult Edit(Doctor doctor, int PatientId)
    {
      if (PatientId !=0)
    {
      _db.DoctorPatient.Add(new DoctorPatient() { PatientId = PatientId, DoctorId = doctor.DoctorId });
    }
        _db.Entry(doctor).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    public ActionResult AddPatient(int id) //add existing patient to doctor, this is automatically a get. entity defaults to a get unless you tell it otherwise
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(DoctorsController => DoctorsController.DoctorId == id);
      ViewBag.PatientId = new SelectList(_db.Patients, "PatientId", "Name", "Birthday");
      return View(thisDoctor);
    }

    [HttpPost] //Whenever you hit submit on a form entity will look for the post command that corresponds with that view 
    public ActionResult AddPatient(Doctor doctor, int PatientId)
    {
      if (PatientId != 0)
      {
        _db.DoctorPatient.Add(new DoctorPatient() { PatientId = PatientId, DoctorId = doctor.DoctorId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }
    
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      _db.Doctors.Remove(thisDoctor);
      _db.SaveChanges(); 
      return RedirectToAction("Index"); 
    }
  }
}