using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataModels;
using BankDataWebService.Data;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly DBManager _context;
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(DBManager context, ILogger<AdminsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        [HttpGet]
        [Route("getAdminByUsername/{Name}")]
        public async Task<IActionResult> GetUserByUsername(string Name)
        {
            _logger.LogInformation($"Request received for user with username in DATAWEBSERVICE: {Name}" + Name);
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.Name == Name);
            if (user == null)
            {
                return new ObjectResult("User not found")
                {
                    StatusCode = 404
                };
            }

            Console.WriteLine(user.Name + user.PhoneNo);
            Admin userObj = new Admin();
            userObj.Name = user.Name;
            userObj.AdminId = user.AdminId;
            //userObj.UserId = user.UserId;
            userObj.PhoneNo = user.PhoneNo;
            userObj.Address = user.Address;
            userObj.Email = user.Email;
            userObj.ProfilePicdBytes = user.ProfilePicdBytes;
            //userObj.AccountList = user.AccountList;
            userObj.Password = user.Password;
            Console.WriteLine(userObj.Name + userObj.PhoneNo + userObj.Address);
            return new ObjectResult(userObj) { StatusCode = 200 };
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.AdminId }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("updateAdmin/{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, AdminUpdateRequest adminUpdateRequest)
        {
            var existingUser = await _context.Admins.FindAsync(id);
            if (existingUser == null)
            {
                _logger.LogInformation($"User with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"User with id {id} found CAAAAAME HERE");

            // Only update the fields that are provided
            if (adminUpdateRequest.username != null)
                existingUser.Name = adminUpdateRequest.username;
            if (adminUpdateRequest.email != null)
                existingUser.Email = adminUpdateRequest.email;
            if (adminUpdateRequest.phoneNo != 0)
                existingUser.PhoneNo = adminUpdateRequest.phoneNo;
            if (adminUpdateRequest.password != null)
                existingUser.Password = adminUpdateRequest.password;
            if (adminUpdateRequest.address != null)
                existingUser.Address = adminUpdateRequest.address;

            if (!string.IsNullOrEmpty(adminUpdateRequest.profilePic))
            {
                existingUser.ProfilePicdBytes = Convert.FromBase64String(adminUpdateRequest.profilePic); // Convert base64 to byte[]
                Console.WriteLine(existingUser.ProfilePicdBytes);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var user = await _context.Admins.FindAsync(id);
            Admin adminObj = new Admin();
            adminObj.Name = user.Name;
            adminObj.AdminId = user.AdminId;
            adminObj.PhoneNo = user.PhoneNo;
            adminObj.Address = user.Address;
            adminObj.Email = user.Email;
            adminObj.Password = user.Password;
            adminObj.ProfilePicdBytes = user.ProfilePicdBytes;
            return new ObjectResult(adminObj) { StatusCode = 200 };

        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }      
    }
}
