using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Badgester.Controllers
{
    /// <summary>
    /// https://github.com/mozilla/openbadges/wiki/Assertions
    /// </summary>
    public class AssertionController : Controller
    {
        private const string BadgeImagePath = "~/Content/badge.png";

        private static readonly int IssueTime = CurrentUnixTime();
            //DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

        /// <summary>
        /// returns information about a specific badge awarded to a specific user
        /// </summary>
        /// <param name="id">locally unique ID of badge</param>
        /// <returns></returns>
        public ActionResult UserBadge(string id)
        {
            var recipient = new
                {
                    type = "email",
                    hashed = true,
                    identity = "sha256$" + HashString("srkirkland@ucdavis.edu"),
                };

            var verify = new {type = "hosted", url = AbsoluteUrl("UserBadge", id: id)};

            var obj = new
                {
                    uid = id,
                    recipient,
                    image = AbsoluteContentUrl(BadgeImagePath),
                    evidence = "http://asi.ucdavis.edu",
                    issuedOn = IssueTime,
                    badge = AbsoluteUrl("Badge", id: id),
                    verify
                };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns info about a badge itself
        /// </summary>
        /// <param name="id">locally unique id of the badge</param>
        /// <returns></returns>
        public ActionResult Badge(string id)
        {
            //          alignment": [
            //  { "name": "CCSS.ELA-Literacy.RST.11-12.3",
            //    "url": "http://www.corestandards.org/ELA-Literacy/RST/11-12/3",
            //    "description": "Follow precisely a complex multistep procedure when carrying out experiments, taking measurements, or performing technical tasks; analyze the specific results based on explanations in the text."
            //  },
            //  { "name": "CCSS.ELA-Literacy.RST.11-12.9",
            //    "url": "http://www.corestandards.org/ELA-Literacy/RST/11-12/9",
            //    "description": " Synthesize information from a range of sources (e.g., texts, experiments, simulations) into a coherent understanding of a process, phenomenon, or concept, resolving conflicting information when possible."
            //  }
            //]

            var obj = new
                {
                    name = "Awesome Fake Badgester Badge",
                    description = "For doing awesome things with robots that people think is pretty great.",
                    image = AbsoluteContentUrl(BadgeImagePath),
                    criteria = AbsoluteUrl("Badge", id: id),
                    tags = new[] {"ucdbadges", "awesome"},
                    issuer = AbsoluteUrl("Organization"),
                };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return information about who issued a badge
        /// </summary>
        /// <returns></returns>
        public ActionResult Organization()
        {
            var obj = new
                {
                    name = "UC Davis FAKE Issuer",
                    image = "http://asi.ucdavis.edu/img/logo-anr.jpg",
                    url = "http://asi.ucdavis.edu/front-page",
                    email = "fake@ucdavis.edu",
                };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        private static int CurrentUnixTime()
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan diff = DateTime.Now - origin;

            return (int)Math.Floor(diff.TotalSeconds);
        }

        private string HashString(string original)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(original);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));
        }

        private string AbsoluteContentUrl(string relativePath)
        {
            return AbsoluteUrl("Index", "Home") + Url.Content(relativePath);
        }

        private string AbsoluteUrl(string action, string controller = null, string id = null)
        {
            return Url.Action(action, controller, new {id}, ControllerContext.HttpContext.Request.Url.Scheme);
        }
    }
}
