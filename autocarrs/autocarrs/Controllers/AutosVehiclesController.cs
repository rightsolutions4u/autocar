using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using autocarrs.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;
using System.Dynamic;
using System.Net.Http.Headers;

namespace autocarrs.Controllers
{
    public class AutosVehiclesController : Controller

    {
       
        private autocarrsContext db = new autocarrsContext();
        

        // GET: AutosVehicles
        public async Task<ActionResult> Index()
        {
            var autosVehicles = db.AutosVehiclesCrollers.Include(a => a.CarBody).Include(a => a.CarCategory).Include(a => a.CarMake).Include(a => a.CarModel);
            return View(await autosVehicles.ToListAsync());
        }

        //GET: SearchCars
        [HttpPost]
        public async Task<ActionResult> SearchCars(FormCollection data)
        {
            var qry = "PowerFrom=" + data["PowerFrom"] + "&PowerTo=" + data["PowerTo"] + "&FromMil=" + data["FromMil"] + "&ToMil=" + data["ToMil"];
            Dictionary<string, string> form = data.AllKeys.ToDictionary(k => k, v => data[v]);
            
            AutosVehicle autosVehicle = new AutosVehicle();
            var myContent = JsonConvert.SerializeObject(form);
           try
            {
                var data1 = new StringContent( myContent + qry,  Encoding.UTF8, "application/json");
                 var url = "https://localhost:44363/api/AutosVehicles/SearchCars";
                var client = new HttpClient();
                var response = await client.PostAsync(url, data1);
                var AutosSearch = response.Content.ReadAsStringAsync().Result;
                AutosVehicle[] a = JsonConvert.DeserializeObject<AutosVehicle[]>(AutosSearch);
                ViewBag.AutosVehicle = a;
                ViewBag.MakeId = data["MakeId"];
                ViewBag.ModlId = data["ModlId"];
                ViewBag.BodyId = data["BodyId"];
                ViewBag.AuYear = data["AuYear"];
                ViewBag.Transmission = data["Transmission"];
                ViewBag.SellPri = data["SellPri"];
                ViewBag.FuelType = data["FuelType"];
                ViewBag.from = data["from"];
                ViewBag.to = data["to"];
                ViewBag.fourbyfour = data["fourbyfour"];
                ViewBag.vatdeduction = data["vatdeduction"];
                ViewBag.FromMil = data["FromMil"];
                ViewBag.ToMil = data["ToMil"];
                ViewBag.PowerFrom = data["PowerFrom"];
                ViewBag.PowerTo = data["PowerTo"];
                ViewBag.FromYear = data["FromYear"];
                ViewBag.ToYear = data["ToYear"];
                ViewBag.PowerFrom = data["PowerFrom"];
                ViewBag.PowerTo = data["PowerTo"];
                ViewBag.Volume = data["Volume"];
                ViewBag.Engine = data["Engine"];
                ViewBag.Seater = data["Seater"];
                ViewBag.Cosumption = data["Cosumption"];
                ViewBag.NoOfDoors = data["NoOfDoors"];
                ViewBag.Acolor = data["Acolor"];

                return View("MainView", a);
            }
             catch (Exception )
            {
                Error err = new Error();
                err.ErrorMessage = "Sorry we found no cars with these filters";
                ViewBag.Error = err;
                ViewBag.AutosVehicle = null;
                return View("Error", err);
            }

        }

        // GET: GetFeaturedAutos
        [HttpGet]
        public async Task<ActionResult> GetFeaturedAutos()
        {
            AutosVehicle autosVehicle = new AutosVehicle();
            
            try
            {
                var url = "https://localhost:44363/api/AutosVehicles/GetFeatuedAutos";
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                var AutosFeatured = response.Content.ReadAsStringAsync().Result;
                try {
                    AutosVehicle[] a = JsonConvert.DeserializeObject<AutosVehicle[]>(AutosFeatured);
                    ViewBag.AutosVehicle = a;
                    return View("Featured_cars", a);
                }
                catch {
                    Error err = new Error();
                    err.ErrorMessage = "All our cars are Featured cars";
                    ViewBag.Error = err;
                    ViewBag.AutosVehicle = null;
                    return View("Error", err);
                }
            }
            catch
            {
                Error err = new Error();
                err.ErrorMessage = "All our cars are Featured cars";
                ViewBag.Error = err;
                ViewBag.AutosVehicle = null;
                return View("Error", err);
            }
        }
        // GET: SearchCarsBrands
        [HttpGet]
        public async Task<ActionResult> SearchCarsBrands(string brand)
        {
           AutosVehicle autosVehicle = new AutosVehicle();
           using (var client = new HttpClient())
            {
                //Passing service base url  
               client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource PostSiteUsers using HttpClient  
                UriBuilder builder = new UriBuilder("https://localhost:44363/api/AutosVehicles/SearchCarsBrands?");
                builder.Query = "Brand=" + brand ;
                HttpResponseMessage Res = await client.GetAsync(builder.Uri);
                  if (Res.IsSuccessStatusCode)
                    {
                    var Brand = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    AutosVehicle[] a = JsonConvert.DeserializeObject<AutosVehicle[]>(Brand);
                    ViewBag.AutosVehicle = a;
                    ViewBag.mystring= brand;
                    ViewBag.Error = null;
                    return View("MainView", a);
                    //return Json(a);
                    }
                else
                    {
                    Error err = new Error();
                    err.ErrorMessage = "Sorry no cars found for"+ brand + "Brand";
                    ViewBag.Error = err;
                    ViewBag.AutosVehicle = null;
                    return View("Error", err);
                    }
            }
        }
        // GET: AutosVehicles/GetAutoById/5
        [HttpGet]
        public async Task<ActionResult> GetAutoById(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutosVehicle autosVehicle = new AutosVehicle();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource PostSiteUsers using HttpClient  
                UriBuilder builder = new UriBuilder("https://localhost:44363/api/AutosVehicles/GetAutoById?");
                builder.Query = "id=" + id;
                HttpResponseMessage Res = await client.GetAsync(builder.Uri);
                if (Res.IsSuccessStatusCode)
                {
                    var auto = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    AutosVehicle a = JsonConvert.DeserializeObject<AutosVehicle>(auto);
                    ViewBag.AutosVehicle = a;
                    ViewBag.mystring = id;
                    ViewBag.Error = null;
                    return View("CarDetails", a);
                    
                }
                else
                {
                    Error err = new Error();
                    err.ErrorMessage = "Sorry no car found on this id " + id ;
                    ViewBag.Error = err;
                    ViewBag.AutosVehicle = null;
                    return View("Error", err);
                }
            }
        }

        // GET: AutosVehicles/PostToCart
        [HttpGet]
        public async Task<ActionResult> PostToCart(int id, int SellPri, char CarMake, char CarModel, char Category, char Color)
        {
            if (Request.Cookies.AllKeys.Contains("UserId"))
            {
                HttpCookie cookie = Request.Cookies["UserId"];
                ViewBag.CookieMessage = cookie.Value;
            }
            if (ViewBag.CookieMessage == null) //create a new one
            {
                    HttpCookie cookie_new = new HttpCookie("UserId");
                    //random buyer generator
                    Random random = new Random();
                    int length = 8;
                    var Buyer = "";
                    for (var i = 0; i < length; i++)
                    {
                    Buyer += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
                    }
                    cookie_new.Value = Buyer;
                    cookie_new.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(cookie_new);
                    ViewBag.CookieMessage = cookie_new.Value;
            }
           Cart cart = new Cart
            {
                CartId = 1,
                BuyrID = ViewBag.CookieMessage,
                AutoId = id,
                SellPri = SellPri,
                Status = "T",
                Endate = DateTime.Now
            };
            string output = JsonConvert.SerializeObject(cart);
            try
            {
                var data = new StringContent(output, Encoding.UTF8, "application/json");
                var url = "https://localhost:44363/api/Carts/PostCart";
                var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                var Cart = response.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Cart>(Cart); // a is the newly posted auto in cart
                //var url1 = "https://localhost:44363/api/Carts/GetCartofBuyer";
                var client1 = new HttpClient();
                UriBuilder builder = new UriBuilder("https://localhost:44363/api/Carts/GetCartofBuyer?");
                builder.Query = "BuyrId=" + ViewBag.CookieMessage;
                HttpResponseMessage response1 = await client1.GetAsync(builder.Uri);
                //var response1 = await client.GetAsync(url1);
                var CartTemp = response1.Content.ReadAsStringAsync().Result;
                Cart[] a1 = JsonConvert.DeserializeObject<Cart[]>(CartTemp);
                ViewBag.Cart = a1;
                ViewBag.CarMake = CarMake;
                ViewBag.CarModel = CarModel;
                ViewBag.Category = Category;
                ViewBag.Color = Color;

                return View("Cart", a1);
            }
            catch
            {
                return View("Error", "Your Cart is empty");
            }
        }
// GET: AutosVehicles/Create
public ActionResult Create()
        {
            ViewBag.BodyId = new SelectList(db.CarBodies, "BodyId", "BDDesc");
            ViewBag.CatgId = new SelectList(db.CarCategories, "CatgId", "CgDesc");
            ViewBag.MakeId = new SelectList(db.CarMakes, "MakeId", "MkDesc");
            ViewBag.ModlId = new SelectList(db.CarModels, "ModlId", "MdDesc");
            return View();
        }

        // POST: AutosVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoId,Acolor,Street,statNm,cityNM,ZipCod,cnName,MILEAG,sellpri,Auyear,transmission,fueltype,power,valume,Engine,Seater,Cosumption,NoOfDoors,IsSold,IsReserved,IsFeataured,IsTrendy,MakeId,ModlId,BodyId,CatgId,sellID")] AutosVehicle autosVehicle)
        {
            if (ModelState.IsValid)
            {
                db.AutosVehiclesCrollers.Add(autosVehicle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BodyId = new SelectList(db.CarBodies, "BodyId", "BDDesc", autosVehicle.BodyId);
            ViewBag.CatgId = new SelectList(db.CarCategories, "CatgId", "CgDesc", autosVehicle.CatgId);
            ViewBag.MakeId = new SelectList(db.CarMakes, "MakeId", "MkDesc", autosVehicle.MakeId);
            ViewBag.ModlId = new SelectList(db.CarModels, "ModlId", "MdDesc", autosVehicle.ModlId);
            return View(autosVehicle);
        }

        // GET: AutosVehicles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutosVehicle autosVehicle = await db.AutosVehiclesCrollers.FindAsync(id);
            if (autosVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.BodyId = new SelectList(db.CarBodies, "BodyId", "BDDesc", autosVehicle.BodyId);
            ViewBag.CatgId = new SelectList(db.CarCategories, "CatgId", "CgDesc", autosVehicle.CatgId);
            ViewBag.MakeId = new SelectList(db.CarMakes, "MakeId", "MkDesc", autosVehicle.MakeId);
            ViewBag.ModlId = new SelectList(db.CarModels, "ModlId", "MdDesc", autosVehicle.ModlId);
            return View(autosVehicle);
        }

        // POST: AutosVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoId,Acolor,Street,statNm,cityNM,ZipCod,cnName,MILEAG,sellpri,Auyear,transmission,fueltype,power,valume,Engine,Seater,Cosumption,NoOfDoors,IsSold,IsReserved,IsFeataured,IsTrendy,MakeId,ModlId,BodyId,CatgId,sellID")] AutosVehicle autosVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autosVehicle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BodyId = new SelectList(db.CarBodies, "BodyId", "BDDesc", autosVehicle.BodyId);
            ViewBag.CatgId = new SelectList(db.CarCategories, "CatgId", "CgDesc", autosVehicle.CatgId);
            ViewBag.MakeId = new SelectList(db.CarMakes, "MakeId", "MkDesc", autosVehicle.MakeId);
            ViewBag.ModlId = new SelectList(db.CarModels, "ModlId", "MdDesc", autosVehicle.ModlId);
            return View(autosVehicle);
        }

        // GET: AutosVehicles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutosVehicle autosVehicle = await db.AutosVehiclesCrollers.FindAsync(id);
            if (autosVehicle == null)
            {
                return HttpNotFound();
            }
            return View(autosVehicle);
        }

        // POST: AutosVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AutosVehicle autosVehicle = await db.AutosVehiclesCrollers.FindAsync(id);
            db.AutosVehiclesCrollers.Remove(autosVehicle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
