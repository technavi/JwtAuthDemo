using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JwtAuthApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory  httpClientFactory)
        {
            _logger = logger;
            _clientFactory = httpClientFactory;
        }

        [BindProperty]
        public string  UserName{ get; set; }

        [BindProperty]
        public string Password{ get; set; }

        public async Task OnGet()
        {
           
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44323/api/Account/login");
            //request.Headers.Add("username", "admin");
            //request.Headers.Add("password", "securePassword");
            
            ////request.Headers.Add("Accept", "application/vnd.github.v3+json");
            ////request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            //var client = _clientFactory.CreateClient();

            //var response = await client.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //    using var responseStream = await response.Content.ReadAsStreamAsync();
            //    //Branches = await JsonSerializer.DeserializeAsync
            //    //    <IEnumerable<GitHubBranch>>(responseStream);
            //}
            ////else
            //{
            //    GetBranchesError = true;
            //    Branches = Array.Empty<GitHubBranch>();
            //}
        }

        public async Task OnPostAsync()
        {
            var u = UserName;
            var p = Password;

            string baseUrl = "https://localhost:44323";
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            Users userModel = new Users
            {
                UserName = UserName,
                Password = Password
            };

            string stringData = JsonConvert.SerializeObject(userModel);
            var contentData = new StringContent(stringData,System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/Account/login", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
          JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

            HttpContext.Session.SetString("token", jwt.accessToken);

            //ViewBag.Message = "User logged in successfully!";

            //return View("Index");

        }

        //public async Task<IActionResult> Index()
        //{
            
        //}
    }
   public class Users
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class JWT
    {
        public string accessToken { get; set; }
        public string role { get; set; }
    }
}
