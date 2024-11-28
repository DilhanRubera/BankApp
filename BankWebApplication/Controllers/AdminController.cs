using Microsoft.AspNetCore.Mvc;
using BankWebApplication.Models;
using BankDataModels;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace BankWebApplication.Controllers
{
    [Route("api")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("adminLogin")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest adminLoginRequest)
        {
            _logger.LogInformation($"Login request received for user: {adminLoginRequest.Username}");


            if (adminLoginRequest == null)
            {

                return BadRequest(new { message = "Request body is null" });
            }

            if (string.IsNullOrEmpty(adminLoginRequest.Username))
            {
                return BadRequest(new { message = "Username cannot be empty" });
            }

            RestClient restClient = new RestClient("http://localhost:5046");
            string adminName = adminLoginRequest.Username;
            RestRequest restRequest = new RestRequest($"api/Admins/getAdminByUsername/{adminName}", Method.Get);
            _logger.LogInformation($"Requesting admin with username: {adminLoginRequest.Username}");
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);
            _logger.LogInformation($"Response received: {restResponse.Content}");

            if (restResponse.IsSuccessful)
            {
                Admin admin = JsonConvert.DeserializeObject<Admin>(restResponse.Content);
                _logger.LogInformation($"Admin found: {admin.Name}");
                if (admin != null && admin.Password == adminLoginRequest.Password)
                {
                    await LogAction(adminLoginRequest.Username, "Login", "Admin", "Admin logged in successfully");
                    return new ObjectResult(admin) { StatusCode = 200 };
                }
                else
                {
                    return Unauthorized(new { message = "Invalid password" });
                }
            }

            return new ObjectResult(null) { StatusCode = 404 };

        }

        [HttpPost]
        [Route("updateAdminProfile")]
        public async Task<IActionResult> Update([FromBody] AdminUpdateRequest adminUpdateRequest)
        {

            _logger.LogInformation($"Update request received for user: {adminUpdateRequest.username}");

            if (adminUpdateRequest == null)
            {
                return BadRequest(new { message = "Request body is null" });
            }

            if (string.IsNullOrEmpty(adminUpdateRequest.username))
            {
                return BadRequest(new { message = "Username cannot be empty" });
            }

            if (adminUpdateRequest.phoneNo == 0)
            {
                return BadRequest(new { message = "Phone number cannot be empty" });
            }

            if (string.IsNullOrEmpty(adminUpdateRequest.address))
            {
                return BadRequest(new { message = "Address cannot be empty" });
            }

            /*Admin updatedAdmin = new Admin();
            updatedAdmin.AdminId = adminUpdateRequest.adminId;
            updatedAdmin.Name = adminUpdateRequest.username;
            updatedAdmin.Password = adminUpdateRequest.password;
            updatedAdmin.PhoneNo = adminUpdateRequest.phoneNo;
            updatedAdmin.Address = adminUpdateRequest.address;
            updatedAdmin.Email = adminUpdateRequest.email;
            updatedAdmin.ProfilePic = adminUpdateRequest.profilePic;*/

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Admins/updateAdmin/{adminUpdateRequest.adminId}", Method.Put);
            restRequest.AddJsonBody(adminUpdateRequest);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                Admin admin = JsonConvert.DeserializeObject<Admin>(restResponse.Content);
                await LogAction(adminUpdateRequest.username, "Update Profile", "Admin", "Admin profile updated");
                return new ObjectResult(admin) { StatusCode = 200 };
                //return Ok(new { message = "User details updated successfully." });
            }

            return new ObjectResult(null) { StatusCode = 404 };

        }

        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users", Method.Post);
            restRequest.AddJsonBody(user);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                User newUser = JsonConvert.DeserializeObject<User>(restResponse.Content);
                await LogAction("AdminUsername", "Create User", "User", $"User with ID {newUser.UserId} created successfully");
                return new ObjectResult(newUser) { StatusCode = 200 };
            }

            if (restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(new { message = restResponse.Content });
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users", Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                List<User> users = JsonConvert.DeserializeObject<List<User>>(restResponse.Content);
                return new ObjectResult(users) { StatusCode = 200 };
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers([FromQuery] string search, [FromQuery] string option = "")
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            string endpoint = $"api/Users/search?search={search}&option={option}";
            RestRequest restRequest = new RestRequest(endpoint, Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                List<User> users = JsonConvert.DeserializeObject<List<User>>(restResponse.Content);
                return new ObjectResult(users) { StatusCode = 200 };
            }
            
            return BadRequest(new { message = "Could not find user" });                     
        }

        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Transactions", Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(restResponse.Content);
                return new ObjectResult(transactions) { StatusCode = 200 };
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpGet]
        [Route("SearchTransaction/{searchId}")]
        public async Task<IActionResult> SearchTransaction(int searchId)
        {
            _logger.LogInformation($"SearchTransaction: Searching for transaction with ID: {searchId}");
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Transactions/{searchId}", Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                Transaction transaction = JsonConvert.DeserializeObject<Transaction>(restResponse.Content);
                return new ObjectResult(transaction) { StatusCode = 200 };
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpPost]
        [Route("createAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            // Log the account object
            string accountJson = JsonConvert.SerializeObject(account);
            _logger.LogInformation($"CreateAccount: Received account object: {accountJson}");
            if (account == null)
            {
                _logger.LogError("UpdateAccount: Account object is null.");
                return BadRequest(new { message = "Account object is null" });
            }

            int id = account.AccountHolderId;
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Users/addAccountForUser/{id}", Method.Put);
            restRequest.AddJsonBody(account);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            // Log the request details
            _logger.LogInformation($"CreateAccount: Sending request to URL: {restRequest.Resource}");
            _logger.LogInformation($"CreateAccount: Request body: {JsonConvert.SerializeObject(account)}");

            if (restResponse.IsSuccessful)
            {
                // Check if the response content is a success message or an Account object
                if (restResponse.Content.Contains("account added"))
                {
                    await LogAction("AdminUsername", "Create Account", "Account", "Account added successfully");
                    _logger.LogInformation("CreateAccount: Account added successfully.");
                    return Ok(new { message = "Account added successfully" });
                }
                else
                {
                    Account newAccount = JsonConvert.DeserializeObject<Account>(restResponse.Content);
                    _logger.LogInformation($"CreateAccount: Account created successfully: {JsonConvert.SerializeObject(newAccount)}");
                    return new ObjectResult(newAccount) { StatusCode = 200 };
                }
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpPut]
        [Route("updateAccount/{accountId}")]
        public async Task<IActionResult> UpdateAccount(int accountId, [FromBody] Account account)
        {
            if (account == null)
            {
                _logger.LogError("UpdateAccount: Account object is null.");
                return BadRequest(new { message = "Account object is null" });
            }

            if (accountId != account.AccountId)
            {
                _logger.LogError("UpdateAccount: Account ID in the URL does not match the account ID in the body.");
                return BadRequest(new { message = "Account ID in the URL does not match the account ID in the body" });
            }

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Accounts/{account.AccountId}", Method.Put);
            restRequest.AddJsonBody(account);
            string requestBody = JsonConvert.SerializeObject(account);
            _logger.LogInformation($"Request body for updating account: {requestBody}");
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                Account updatedAccount = JsonConvert.DeserializeObject<Account>(restResponse.Content);
                return new ObjectResult(updatedAccount) { StatusCode = 200 };
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpPut]
        [Route("activateAccount/{id}")]
        public async Task<IActionResult> ActivateAccount(int id)
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Accounts/{id}/activate", Method.Put);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                await LogAction("AdminUsername", "Activate Account", "Account", $"Account with ID {id} activated");
                return NoContent();
            }

            return new ObjectResult(null) { StatusCode = restResponse.StatusCode == System.Net.HttpStatusCode.NotFound ? 404 : 400 };
        }

        [HttpPut]
        [Route("deactivateAccount/{id}")]
        public async Task<IActionResult> DeactivateAccount(int id)
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest($"api/Accounts/{id}/deactivate", Method.Put);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                await LogAction("AdminUsername", "Deactivate Account", "Account", $"Account with ID {id} deactivated");
                return NoContent();
            }

            return new ObjectResult(null) { StatusCode = restResponse.StatusCode == System.Net.HttpStatusCode.NotFound ? 404 : 400 };
        }

        [HttpGet]
        [Route("getAuditLogs")]
        public async Task<IActionResult> GetAuditLogs()
        {
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/AuditLogs", Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                var auditLogs = JsonConvert.DeserializeObject<List<AuditLog>>(restResponse.Content);
                return Json(auditLogs);
            }
            else
            {
                return Json(new List<AuditLog>());
            }
        }

        public async Task LogAction(string adminUsername, string act, string affectedResrc, string details)
        {
            var auditLog = new AuditLog
            {
                AdminUsername = adminUsername,
                Action = act,
                Timestamp = DateTime.UtcNow,
                AffectedResource = affectedResrc,
                Details = details
            };

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/AuditLogs", Method.Post);
            restRequest.AddJsonBody(auditLog);
            await restClient.ExecuteAsync(restRequest);
        }

        public class AdminLoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


    }
}
