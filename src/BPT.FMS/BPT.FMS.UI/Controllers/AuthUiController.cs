using Azure;
using Azure.Core;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.UI.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace BPT.FMS.UI.Controllers
{
    public class AuthUiController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthUiController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FmsApi");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                Response.Redirect("/Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginRequest = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthenticationResultDto>(responseContent);

                    HttpContext.Session.SetString("AdminId", result.User.Id.ToString());
                    HttpContext.Session.SetString("AdminEmail", result.User.Email);
                    HttpContext.Session.SetString("AdminName", result.User.UserName);
                    HttpContext.Session.SetString("AccessLevel", result.User.AccessLevel);
                    HttpContext.Session.SetString("JwtToken", result.Token ?? "");

                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToAction("Index", "DashboardUi");
                }
                return View(model);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Unable to connect to the server. Please try again later.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var adminEmail = HttpContext.Session.GetString("AdminEmail");
            TempData["SuccessMessage"] = $"User logged out successfully.";

            HttpContext.Session.Clear();

            return RedirectToAction("Login","AuthUi");
        }
    }
}