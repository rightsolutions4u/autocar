using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using autocarrs.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace autocarrs.Controllers
{
    public class LoadController : Controller
    {
        private autocarrsContext db = new autocarrsContext();
        
        // GET: Load
        public ActionResult Index()
        {
            return View();
        }
        public class UserCookie
        {
            public string Value1;
            public DateTime Expiry;
        }
        public UserCookie GetCookie()
        {
            UserCookie u = new UserCookie();
            if (Request.Cookies.AllKeys.Contains("UserId"))
            {
                HttpCookie cookie = Request.Cookies["UserId"];
                u.Value1 = cookie.Value;
                u.Expiry = cookie.Expires;
            }
          
            return u;
        }

        [HttpGet]
        public async Task<ActionResult> GetLoad()
        {
            Load load = new Load();
            var AutosVehiclesController = new AutosVehiclesController();
            var CarMakesController = new CarMakesController();
            load.AutosVehicleF = (IEnumerable<AutosVehicle>)await AutosVehiclesController.GetFeaturedAutos();
            load.CarMake = (IEnumerable<CarMake>)await CarMakesController.GetCarMake();
            UserCookie uc = GetCookie();
            ViewBag.Value = uc.Value1;
            ViewBag.Expiry = uc.Expiry;
            return View("Featured_cars", load);
        }
        

    }
}