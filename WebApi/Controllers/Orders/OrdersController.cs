using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SqlContext _context;

        public OrdersController(SqlContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderRead>>> GetOrders()
        {
            var ordersList = new List<OrderRead>();


            foreach (var order in await _context.Orders.Include(x => x.OrderDetails).ToListAsync())
            {

                ordersList.Add(new OrderRead
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Amount = order.Amount,
                    OrderDetails = order.OrderDetails,


                });

            }

            return ordersList;

           
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (id.GetType() != typeof(int))
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"You must enter a number as Id" }));
            }
            var order = await _context.Orders.Include(x=>x.OrderDetails).Where(x=> x.Id == id).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order model)
        {
            if (id.GetType() != typeof(int))
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"You must enter a number as Id" }));
            }
             var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                    return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"A order with id: {id} do not exists." }));
                }
          
            order.Amount = model.OrderDetails.Sum(x => x.Quantity * x.Price);

            foreach (var item in model.OrderDetails)
            {
                var orderDetailExist = await _context.OrderDetails.FindAsync(item.Id);
                if (orderDetailExist == null)
                {
                     return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"A item with id: {item.Id} do not exists in order." }));
                   
                }

                orderDetailExist.OrderId = id;
                orderDetailExist.ProductId = item.ProductId != 0 ? item.ProductId : orderDetailExist.ProductId; 
                orderDetailExist.Quantity = item.Quantity != 0 ? item.Quantity : orderDetailExist.Quantity;
                orderDetailExist.Price = item.Price != 0 ? item.Price : orderDetailExist.Price; 
                
                _context.OrderDetails.Update(orderDetailExist);
                await _context.SaveChangesAsync();
            }
           
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(JsonConvert.SerializeObject(new { message = "Updated" }));
           
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]

        public async Task<ActionResult<OrderDetail>> PostOrder(List<CartItem> model, int userId)
        {
            var order = new Order()
            {
                UserId = userId,
                Amount = model.Sum(x=>x.Quantity * x.Price)
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach(var item in model) { 
            var orderDetail = new OrderDetail()
            {
                Quantity = item.Quantity,
                ProductId = item.ProductId,
                OrderId = order.Id,
                Price = item.Price
            };
            await _context.OrderDetails.AddAsync(orderDetail);
                }

            await _context.SaveChangesAsync();
            //return Ok();
            return CreatedAtAction("GetOrder", new { id = order.Id }, /*orderDetail*/ new Order {
                Id = order.Id,
                UserId = order.UserId,
                Amount = order.Amount,
                OrderDetails = order.OrderDetails

            });
        }







        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(JsonConvert.SerializeObject(new { message = "Deleted" }));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
