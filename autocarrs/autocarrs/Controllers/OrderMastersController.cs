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
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace autocarrs.Controllers
{
    public class OrderMastersController : Controller
    {
        private autocarrsContext db = new autocarrsContext();

        // GET: OrderMasters
        public async Task<ActionResult> Index()
        {
            return View(await db.OrderMasters.ToListAsync());
        }
        // GET: OrderMasters
        public async Task<ActionResult> AddOrder(int AutoId, int SellPri)
        {
            var Val = "";
            if (Request.Cookies.AllKeys.Contains("UserId"))
            {
                HttpCookie cookie = Request.Cookies["UserId"];
                 Val = cookie.Value;
                //if (cookie.Expires < DateTime.Now)
                //{
                //    Val = null;
                //}

            }
            if (Val == null) //not login / registered or login expired or 
            {
                return HttpNotFound();

            }
            else
            {
                OrderMaster ordermaster = new OrderMaster
                {
                    TRDATE = DateTime.Now,
                    BUYRID = Val,
                    AMOUNT = SellPri,
                    PartialPayment = 0
                };
                string output = JsonConvert.SerializeObject(ordermaster);
                try
                {
                    var data = new StringContent(output, Encoding.UTF8, "application/json"); var url = "https://localhost:44363/api/OrderMasters/PostOrderMaster";
                    var client = new HttpClient();
                    var response = await client.PostAsync(url, data);
                    var OrderMaster = response.Content.ReadAsStringAsync().Result;
                    var a = JsonConvert.DeserializeObject<SiteUsers>(OrderMaster);
                    ViewBag.OrderMaster = a;
                    return View("Cart", a);

                }
                catch
                {
                    return View();
                }
            }
        }

        // GET: OrderMasters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderMaster orderMaster = await db.OrderMasters.FindAsync(id);
            if (orderMaster == null)
            {
                return HttpNotFound();
            }
            return View(orderMaster);
        }

        // GET: OrderMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "INVNUM,TRDATE,BUYRID,AMOUNT,PartialPayment")] OrderMaster orderMaster)
        {
            if (ModelState.IsValid)
            {
                db.OrderMasters.Add(orderMaster);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(orderMaster);
        }

        // GET: OrderMasters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderMaster orderMaster = await db.OrderMasters.FindAsync(id);
            if (orderMaster == null)
            {
                return HttpNotFound();
            }
            return View(orderMaster);
        }

        // POST: OrderMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "INVNUM,TRDATE,BUYRID,AMOUNT,PartialPayment")] OrderMaster orderMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderMaster);
        }

        // GET: OrderMasters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderMaster orderMaster = await db.OrderMasters.FindAsync(id);
            if (orderMaster == null)
            {
                return HttpNotFound();
            }
            return View(orderMaster);
        }

        // POST: OrderMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OrderMaster orderMaster = await db.OrderMasters.FindAsync(id);
            db.OrderMasters.Remove(orderMaster);
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
