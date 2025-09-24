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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BPT.FMS.UI.Controllers
{
    public class VoucherUiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public VoucherUiController(IHttpClientFactory httpClientFactory)
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
        public async Task<JsonResult> GetAllVoucherEntriesAsync(GetVoucherEntriesDto query)
        {
            try
            {
                var url = $"api/Voucher/all/{query.voucherId}?pageIndex={query.PageIndex}&pageSize={query.PageSize}" +
                 $"&sortColumn={Uri.EscapeDataString(query.FormatSortExpression("VoucherType", "Account", "Debit","Credit"))}&search={Uri.EscapeDataString(query.Search.Value)}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new List<ChartOfAccountDto>());
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<VoucherEntryDto>>(stream, _jsonOptions);

                return Json(result ?? new List<VoucherEntryDto>());
            }
            catch
            {
                return Json(new List<VoucherEntryDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(VoucherDto model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Voucher", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to create voucher";
                return Json(new { success = false, message = "Failed to create voucher" });
            }
            TempData["SuccessMessage"] = "Voucher created successfully";
            return Json(new { success = true, message = "Voucher created successfully" });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Voucher/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete voucher";
                return Json(new { success = false, message = "Failed to delete voucher" });
            }
         
            TempData["SuccessMessage"] = "Voucher deleted successfully";
            return Json(new { success = true, message = "Voucher deleted successfully" });
        }

        [HttpPost]
        public async Task<JsonResult> GetVouchersJsonDataAsync([FromBody] GetVouchersDto query)
        {
            try
            {
                var url = $"api/Voucher/paginated?pageIndex={query.PageIndex}&pageSize={query.PageSize}" +
                 $"&sortColumn={Uri.EscapeDataString(query.FormatSortExpression("Type", "Description", "Date"))}&search={Uri.EscapeDataString(query.Search.Value)}";

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


