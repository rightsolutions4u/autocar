using System;
using System.Collections.Generic;
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
    public class OrderMastersController : ControllerBase
    {
        private readonly SiteContext _context;
        

        public OrderMastersController(SiteContext context)
        {
            _context = context;
        }
        

        // GET: api/OrderMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderMaster>>> Getordoermaster()
        {
            return await _context.ordermaster.ToListAsync();
        }

        // GET: api/OrderMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderMaster>> GetOrderMaster(int id)
        {
            var orderMaster = await _context.ordermaster.FindAsync(id);

            if (orderMaster == null)
            {
                return NotFound();
            }

            return orderMaster;
        }

        // PUT: api/OrderMasters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderMaster(int id, OrderMaster orderMaster)
        {
            if (id != orderMaster.INVNUM)
            {
                return BadRequest();
            }

            _context.Entry(orderMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderMasterExists(id))
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

        // POST: api/OrderMasters
        [HttpPost]
        public async Task<ActionResult<OrderMaster>> PostOrderMaster(OrderMaster orderMaster)
        {
            //var AutosBuyersController = new AutosBuyersController();
            //Boolean Buyer = AutosBuyersController.AutosBuyerExists(orderMaster.BUYRID);
            //if (Buyer)
            //{
                _context.ordermaster.Add(orderMaster);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (OrderMasterExists(orderMaster.INVNUM))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetOrderMaster", new { id = orderMaster.INVNUM }, orderMaster);
            //}
            //return NotFound();
        }
        // DELETE: api/OrderMasters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderMaster>> DeleteOrderMaster(int id)
        {
            var orderMaster = await _context.ordermaster.FindAsync(id);
            if (orderMaster == null)
            {
                return NotFound();
            }

            _context.ordermaster.Remove(orderMaster);
            await _context.SaveChangesAsync();

            return orderMaster;
        }

        private bool OrderMasterExists(int id)
        {
            return _context.ordermaster.Any(e => e.INVNUM == id);
        }
    }
}
