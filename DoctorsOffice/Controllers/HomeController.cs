using Microsoft.AspNetCore.Mvc;

namespace DoctorsOffice.Controllers
{
  public class HomeController : Controllers 
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View(); 
    }
  }
}