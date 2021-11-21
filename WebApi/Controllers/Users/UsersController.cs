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
using WebApi.Validation;

namespace WebApi.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SqlContext _context;

        public UsersController(SqlContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUser>>> GetUsers()
        {
            var usersList = new List<ReadUser>();
           
           
                foreach (var user in await _context.Users.Include(x => x.Orders).ToListAsync())
                {
                   
                        usersList.Add(new ReadUser
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Password = user.Password,
                            

                        });
                    
                }
                
                

            
            return usersList;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (id.GetType() != typeof(int))
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"You must enter a number as Id" }));
            }

            var user = await _context.Users.Include(x => x.Orders).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return new OkObjectResult(new ReadUser
            {

                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
            });
             
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUser model)
        {

            if (id.GetType() != typeof(int))
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user != null)
                {

                    user.FirstName = !string.IsNullOrEmpty(model.FirstName) ? model.FirstName : user.FirstName;
                    user.LastName = !string.IsNullOrEmpty(model.LastName) ? model.LastName : user.LastName;
                    user.Email = !string.IsNullOrEmpty(model.Email) ? model.Email : user.Email;
                    user.Password = !string.IsNullOrEmpty(model.Password) ? model.Password : user.Password; 

                    _context.Entry(user).State = EntityState.Modified;

                }


                else
                {
                    return BadRequest();
                }



                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUser model)
        {
            if (!string.IsNullOrEmpty(model.Email) &&
                !string.IsNullOrEmpty(model.Password) &&
                !string.IsNullOrEmpty(model.FirstName) &&
                !string.IsNullOrEmpty(model.LastName))
            {
                var user = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    if (Validations.IsValidEmail(model.Email))
                    {
                        if (Validations.IsValidPassword(model.Password))
                        {
                            if (model.FirstName.Length > 2 && model.LastName.Length > 2)
                            {
                                var createdUser = new User
                                {
                                    FirstName = model.FirstName,
                                    LastName = model.LastName,
                                    Email = model.Email,
                                    Password = model.Password

                                };
                                _context.Users.Add(createdUser);
                                await _context.SaveChangesAsync();

                                return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
                            
                        } else return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"Firstname and Lastname must be atlease 2 characters long." }));
                    }
                        else return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character" }));
                    }

                    else return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"${model.Email} is not a valid email address." }));
                }
                else return new ConflictResult();
            }
            else return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"All fields must contains values." }));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(JsonConvert.SerializeObject(new { message = "Deleted" }));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
