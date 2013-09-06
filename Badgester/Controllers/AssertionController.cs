using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Badgester.Controllers
{
    /// <summary>
    /// https://github.com/mozilla/openbadges/wiki/Assertions
    /// </summary>
    public class AssertionController : Controller
    {
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
                    salt = "deadsea",
                    identity = "sha256$c7ea4bf7bb3c4f9b1dd1126f1867e0182ceb6c27fea521a2836eb675d5d32640" //srkirkland@ucdavis.edu
                };

            var verify = new {type = "hosted", url = "https://example.org/beths-robotics-badge.json"};

            var obj = new
                {
                    uid = id, recipient,
                    image = "https://example.org/beths-robot-badge.png",
                    evidence = "https://example.org/beths-robot-work.html",
                    issuedOn = 1359217910,
                    badge = "https://example.org/robotics-badge.json",
                    verify
                };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
