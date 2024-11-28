using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json;
using BankDataModels;

namespace BankWebApplication.Controllers
{
    [Route("api")]
    public class APIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("userLogin")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            Console.WriteLine("UserLogin API hit");

            if (userLoginRequest == null)
            {
                Console.WriteLine("userLoginRequest is null");
                return BadRequest(new { message = "Request body is null" });
            }
            Console.WriteLine("got here");
            Console.WriteLine(userLoginRequest.Username);

            if (string.IsNullOrEmpty(userLoginRequest.Username))
            {
                Console.WriteLine("Username cannot be empty");

                return BadRequest(new { message = "Username cannot be empty" });
            }
            Console.WriteLine("got here twoo");

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users/getUserByUsername", Method.Post);
            restRequest.AddJsonBody(userLoginRequest);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                User user = JsonConvert.DeserializeObject<User>(restResponse.Content);
                Console.WriteLine(user.Name + user.PhoneNo);
                return new ObjectResult(user) { StatusCode = 200 };
            }

            return  BadRequest(restResponse.Content);          

        }

        [HttpPost]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updatedUser)
        {
            Console.WriteLine("Name" + updatedUser.Name);
            Console.WriteLine(updatedUser.PhoneNo);
            Console.WriteLine(updatedUser.Email);
            Console.WriteLine(updatedUser.Password);
            Console.WriteLine(updatedUser.UserId);
            Console.WriteLine(updatedUser.Address);
            Console.WriteLine(updatedUser.ProfilePicBase64);

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users/updateUser/" + updatedUser.UserId, Method.Put);

            restRequest.AddJsonBody(updatedUser);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
               
                Console.WriteLine("User details updated successfully.");
                User user = JsonConvert.DeserializeObject<User>(restResponse.Content);
                Console.WriteLine(user.Name + user.PhoneNo);
                Console.WriteLine(user.ProfilePicdBytes);
                return new ObjectResult(user) { StatusCode = 200 };
            }

            return new ObjectResult(null) { StatusCode = 404 };
        }

        [HttpGet]
        [Route("getUserAccounts/{userId}")]
        public async Task<IActionResult> GetUserAccounts(string userId)
        {

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users/getUserAccounts/" + userId, Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                List<Account> accountList = JsonConvert.DeserializeObject<List<Account>>(restResponse.Content);

                foreach (var account in accountList)
                {
                    Console.WriteLine("Account ID: " + account.AccountId);
                    Console.WriteLine("Account Number: " + account.AccountNumber);
                    Console.WriteLine("Account Name: " + account.AccountName);
                    Console.WriteLine("Account Balance: " + account.AccountBalance);
                    Console.WriteLine("-----------------------------");
                }
                return new ObjectResult(accountList) { StatusCode = 200 };

            }

            return new ObjectResult(null) { StatusCode = 404 };

        }

        [HttpGet]
        [Route("getAccountTransactions/{accountId}")]
        public async Task<IActionResult> GetAccountTransactions(string accountId)
        {

            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Accounts/getAccountTransactions/" + accountId, Method.Get);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                List<Transaction> transactionList = JsonConvert.DeserializeObject<List<Transaction>>(restResponse.Content);
                return new ObjectResult(transactionList) { StatusCode = 200 };

            }

            return new ObjectResult(null) { StatusCode = 404 };

        }

        [HttpPost]
        [Route("makeTransaction/{userId}")]
        public async Task<IActionResult> MakeTransaction([FromBody] TransactionRequest transactionRequest, int userId)
        {
            Console.WriteLine(transactionRequest.SenderAccountNo);
            Console.WriteLine(transactionRequest.ReceiverAccountNo);
            Console.WriteLine(transactionRequest.TransactionAmount);
            Console.WriteLine(userId);
            RestClient restClient = new RestClient("http://localhost:5046");
            RestRequest restRequest = new RestRequest("api/Users/makeTransaction/" + userId, Method.Post);

            restRequest.AddJsonBody(transactionRequest);
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.Content);
                return Ok(restResponse.Content);
            }
            else

            {
                Console.WriteLine(restResponse.Content);
                return BadRequest(restResponse.Content);

            }

        }
    }
}
