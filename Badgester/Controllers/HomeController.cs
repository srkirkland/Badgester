using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Badgester.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verify(string id)
        {
            using (var client = new SmtpClient("smtp.ucdavis.edu"))
            {
                client.Send("srkirkland@ucdavis.edu", "srkirkland@ucdavis.edu", "Attempt to verify badge assertion",
                            "mozilla called for the verification");
            }
            
            return Content("Testing123");
        }
    }
}
