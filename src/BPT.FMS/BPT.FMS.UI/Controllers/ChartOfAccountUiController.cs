using BPT.FMS.Application.Features.ChartOfAccount.Queries;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Features.ChartOfAccount.Queries;
using BPT.FMS.UI.Helpers;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BPT.FMS.UI.Controllers
{
    public class ChartOfAccountUiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ChartOfAccountUiController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FmsApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllChartOfAccountsAsync(Guid childId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ChartOfAccount/all/{childId}");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new List<ChartOfAccountDto>());
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<ChartOfAccountDto>>(stream, _jsonOptions);

                return Json(result ?? new List<ChartOfAccountDto>());
            }
            catch
            {
                return Json(new List<ChartOfAccountDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChartOfAccountDto dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/ChartOfAccount", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to create chart of account";
                return Json(new { success = false, message = "Failed to create chart of account" });
            }
            TempData["SuccessMessage"] = "Chart of account created successfully";
            return Json(new { success = true, message = "Chart of account created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/ChartOfAccount/{id}");

            if (!response.IsSuccessStatusCode)
                return Json(new { success = false, message = "Failed to update chart of account" });

            var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<object>(stream, _jsonOptions);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ChartOfAccountDto dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/ChartOfAccount/{dto.Id}", content);

            if (!response.IsSuccessStatusCode)  
                return Json(new { success = false, message = "Failed to update chart of account" }); 

            TempData["SuccessMessage"] = "Chart of account updated successfully";
            return Json(new { success = true, message = "Chart of account updated successfully" }); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/ChartOfAccount/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete chart of account";
                return Json(new { success = false, message = "Failed to delete chart of account" });
            }
         
            TempData["SuccessMessage"] = "Chart of account deleted successfully";
            return Json(new { success = true, message = "Chart of account deleted successfully" });
        }

        [HttpPost]
        public async Task<JsonResult> GetChartOfAccountsJsonDataAsync([FromBody] GetChartOfAccountsDto query)
        {
            try
            {
                var url = $"api/ChartOfAccount/paginated?pageIndex={query.PageIndex}&pageSize={query.PageSize}" +
                 $"&sortColumn={Uri.EscapeDataString(query.FormatSortExpression("AccountName", "AccountType", "ParentAccount", "CreateAt"))}&search={Uri.EscapeDataString(query.Search.Value)}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(DataTables.EmptyResult);
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<object>(stream, _jsonOptions);

                return Json(result ?? DataTables.EmptyResult);
            }
            catch
            {
                return Json(DataTables.EmptyResult);
            }
        }
    }
}


