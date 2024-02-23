using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
        private String endPointURL = string.Empty;
        public UserController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            endPointURL = "http://localhost:5037";
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> Index(string? userName)
        {

            
            HttpResponseMessage departmentRes = await _httpClient.GetAsync($"{endPointURL}/department");
            HttpResponseMessage userRes = await _httpClient.GetAsync($"{endPointURL}/user?userName={userName}");

            if (departmentRes.IsSuccessStatusCode)
            {
                string response = await departmentRes.Content.ReadAsStringAsync();
                dynamic responseRaw = JsonConvert.DeserializeObject<dynamic>(response);
                dynamic listDepartmentData = responseRaw["data"];
                ViewData["listDepartmentData"] = listDepartmentData;
            }
            else
            {
                return View("Error");
            }

            if (userRes.IsSuccessStatusCode)
            {
                string response = await userRes.Content.ReadAsStringAsync();
                dynamic responseRaw = JsonConvert.DeserializeObject<dynamic>(response);
                dynamic listUserData = responseRaw["data"];
                ViewData["listUserData"] = listUserData;
                return View();
            }
            else
            {
                return View("Error");
            }
        }
    }
}
