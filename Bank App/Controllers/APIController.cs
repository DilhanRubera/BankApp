using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.Data;
using System.Reflection;
using BankDataModels;
using BankDataWebService;
using BankDataWebService.Controllers;

namespace Bank_App.Controllers
{
    [Route("api/[controller]")]
    public class APIController : Controller
    {
        private readonly UsersController _usersController;


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            if (string.IsNullOrEmpty(userLoginRequest.Username))
            {
                return BadRequest("Username cannot be empty");
            }

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("getUserByUsername",Method.Get);
            restRequest.AddJsonBody(userLoginRequest);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                User user = JsonConvert.DeserializeObject<User>(restResponse.Content);
                Console.WriteLine(user.Name + user.PhoneNo);
                return new ObjectResult(user) { StatusCode =200};
            }

            return new ObjectResult(null) { StatusCode =404};  

          //  var user = await _usersController.GetUserByUsername(userLoginRequest);
            //Console.WriteLine(user);

        /*    if (user != null)
            {
                return Ok(user); // Send the full User object
            }
            else
            {
                return NotFound(new { message = "User not found" });
            }*/

        }
    }
}
