using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly SiteContext _context;

        public CartsController(SiteContext context)
        {
            _context = context;
        }
        // GET: api/Carts
        [HttpGet("GetTotalOrder")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetTotalOrder(int cartId, string BuyrId)
        {
           
            try 
            {
                var cart = _context.Cart.FirstOrDefault(s => s.CartId.Equals(cartId));
                cart.Status = "U";
                _context.Entry(cart).Property("Status").IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " + "see your system administrator.");
            }
            return await _context.Cart     
                 .Where(a => a.Status == "U" && a.BuyrID == BuyrId )
                 .Include(vehicle => vehicle.AutosVehicle)
                     .ThenInclude(make => make.CarMake)
                 .Include(vehicle => vehicle.AutosVehicle)
                      .ThenInclude(model => model.CarModel)
                 .Include(vehicle => vehicle.AutosVehicle)
                      .ThenInclude(carbody => carbody.CarBody)
                 .ToListAsync();
             
        }
        // GET: api/Carts
        [HttpGet("GetCartofBuyer")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartofBuyer(string BuyrId)
        {
            return await _context.Cart.Where(a => /*a.Status == "T" && */a.BuyrID == BuyrId)
                                            .Include(vehicle => vehicle.AutosVehicle)
                                                .ThenInclude(make => make.CarMake)
                                             .Include(vehicle => vehicle.AutosVehicle)
                                                 .ThenInclude(model => model.CarModel)
                                              .Include(vehicle => vehicle.AutosVehicle)
                                                 .ThenInclude(carbody => carbody.CarBody)
                                            .ToListAsync();
                                            
        }
        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }
            return cart;
          }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carts
        [HttpPost("PostCart")]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/
        [HttpDelete("DeleteCart")] 
        public async Task<ActionResult<IEnumerable<Cart>>> DeleteCart(int id, string BuyrId)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return await GetCartofBuyer(BuyrId); ;
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }
    }
}
