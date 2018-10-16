using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Models;

namespace University.Controllers
{
    public class SearchController : Controller
    {
        UserContext db = new UserContext();
        // GET: Search
        public ActionResult Index(string searching)
        {
            var users = from s in db.Users select s;
            if(!String.IsNullOrEmpty(searching))
            {
                users = users.Where(s => s.FirstName.Contains(searching));

            }
            return View(users.ToList());
        }
    }
}