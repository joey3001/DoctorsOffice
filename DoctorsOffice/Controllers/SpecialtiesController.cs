using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorsOffice.Controllers
{
  public class SpecialtiesController : Controller
  {
    private readonly DoctorsOfficeContext _db; 
    public SpecialtiesController(DoctorsOfficeContext db)
    {
      _db = db; 
    }
    public ActionResult Index()
    {
      return View(_db.Specialty.ToList()); 
    }
    public ActionResult Details (int id)
    {
      var thisSpecialty = _db.Specialty
        .Include(specialty => specialty.Doctors)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(specialty => specialty.SpecialtyId == id); 
      return View(thisSpecialty); 
    }
    public ActionResult Create() //create new doctor for specialty
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Specialty specialty, int DoctorId) //create new doctor for specialty
    {
      _db.Specialty.Add(specialty);
      if (DoctorId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Edit(int id)
    {
      var thisSpecialty = _db.Specialty.FirstOrDefault(SpecialtyController => SpecialtyController.SpecialtyId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View(thisSpecialty);
    }
    [HttpPost]
    public ActionResult Edit(Specialty specialty, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId }); 
      }
      _db.Entry(specialty).State = EntityState.Modified; 
      _db.SaveChanges(); 
      return RedirectToAction("Index"); 
    }

    public ActionResult AddDoctor(int id) //add existing doctor to specialty 
    {
      var thisSpecialty = _db.Specialty.FirstOrDefault(SpecialtyController => SpecialtyController.SpecialtyId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name", "Specialty");
      return View(thisSpecialty);
    }
    
    [HttpPost]
    public ActionResult AddDoctor(Specialty specialty, int DoctorId)
    {
      if (DoctorId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() { DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisSpecialty = _db.Specialty.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      return View(thisSpecialty);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisSpecialty = _db.Specialty.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      _db.Specialty.Remove(thisSpecialty);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteDoctor(int joinId)
    {
      var joinEntry = _db.DoctorSpecialty.FirstOrDefault(entry => entry.DoctorSpecialtyId == joinId);
      _db.DoctorSpecialty.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}