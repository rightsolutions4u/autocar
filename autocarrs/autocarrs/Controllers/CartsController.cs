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
using System.Net.Http.Headers;

namespace autocarrs.Controllers
{
    public class CartsController : Controller
    {
        private autocarrsContext db = new autocarrsContext();

        // GET: Carts
        public async Task<ActionResult> Index()
        {
            var carts = db.Carts.Include(c => c.AutosVehicle);
            return View(await carts.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CartId,AutoId,BuyrID,SellPri,Status,Endate")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
        // GET: Carts/Delete/5
        public async Task<ActionResult> Delete(int? id, string BuyrId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = new Cart();
            var client = new HttpClient();
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource DeleteCart using HttpClient  
                UriBuilder builder = new UriBuilder("https://localhost:44363/api/Carts/DeleteCart?");
                builder.Query = "id=" + id + "&BuyrId=" + BuyrId; 
                HttpResponseMessage Res = await client.GetAsync(builder.Uri);
                if (Res.IsSuccessStatusCode)
                {
                var cart1 = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                Cart[] c = JsonConvert.DeserializeObject<Cart[]>(cart1);
                ViewBag.Cart = c;
                return View("Cart", c);
                }
                else
                {
                    Error err = new Error();
                    err.ErrorMessage = "Sorry no cart found on this id " + id;
                    ViewBag.Error = err;
                    ViewBag.Cart = null;
                    return View("Error", err);
                }
        }
        // GET: Carts/MyCart
        public async Task<ActionResult> MyCart(string BuyrId)
        {
            BuyrId = "Shazia";
            Cart cart = new Cart();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource DeleteCart using HttpClient  
            UriBuilder builder = new UriBuilder("https://localhost:44363/api/Carts/GetCartofBuyer?");
            builder.Query = "BuyrId=" +  BuyrId;
            HttpResponseMessage Res = await client.GetAsync(builder.Uri);
            if (Res.IsSuccessStatusCode)
            {
                var cart1 = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                Cart[] c = JsonConvert.DeserializeObject<Cart[]>(cart1);
                ViewBag.Cart = c;
                return View("Cart", c);
            }
            else
            {
                Error err = new Error();
                err.ErrorMessage = "Sorry there are no items in your cart  " + BuyrId;
                ViewBag.Error = err;
                ViewBag.Cart = null;
                return View("Error", err);
            }
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            db.Carts.Remove(cart);
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
