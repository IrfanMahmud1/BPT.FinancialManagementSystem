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
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BPT.FMS.UI.Controllers
{
    public class JournalUiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public JournalUiController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> JournalEntries(Guid id)
        {
            try
            {
                var url = $"api/Journal/entries?journalId={id}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new List<JournalEntryDto>());
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<JournalEntryDto>>(stream, _jsonOptions);

                return View(result ?? new List<JournalEntryDto>());
            }
            catch
            {
                return View(new List<JournalEntryDto>());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"UI: Received form data with {form.Count} keys");
                foreach (var key in form.Keys)
                {
                    Console.WriteLine($"UI: Form key: {key} = {form[key]}");
                }
                
                var type = form["Type"].ToString();
                var referenceNo = form["ReferenceNo"].ToString();
                var date = form["Date"].ToString();
                
                var entries = new List<JournalEntryDto>();
                
                // Parse entries from form data
                var entryCount = 0;
                while (form.ContainsKey($"Entries[{entryCount}].ChartOfAccountId"))
                {
                    var entry = new JournalEntryDto
                    {
                        ChartOfAccountId = Guid.Parse(form[$"Entries[{entryCount}].ChartOfAccountId"].ToString()),
                        Debit = decimal.Parse(form[$"Entries[{entryCount}].Debit"].ToString()),
                        Credit = decimal.Parse(form[$"Entries[{entryCount}].Credit"].ToString())
                    };
                    entries.Add(entry);
                    entryCount++;
                }

                var model = new JournalDto
                {
                    Type = type,
                    ReferenceNo = referenceNo,
                    Date = DateTime.Parse(date),
                    Entries = entries
                };

                var jsonContent = JsonSerializer.Serialize(model);
                Console.WriteLine($"UI: Sending to API: {jsonContent}");
                
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Journal", content);
                
                Console.WriteLine($"UI: API Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Failed to create journal: {errorContent}" });
                }
                
                return Json(new { success = true, message = "Journal created successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Journal/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete journal";
                return Json(new { success = false, message = "Failed to delete journal" });
            }
         
            TempData["SuccessMessage"] = "Journal deleted successfully";
            return Json(new { success = true, message = "Journal deleted successfully" });
        }

        [HttpPost]
        public async Task<JsonResult> GetJournalsJsonDataAsync([FromBody] GetJournalsDto query)
        {
            try
            {
                var url = $"api/Journal/paginated?pageIndex={query.PageIndex}&pageSize={query.PageSize}" +
                 $"&sortColumn={Uri.EscapeDataString(query.FormatSortExpression("Type", "ReferenceNo", "Date"))}&search={Uri.EscapeDataString(query.Search.Value)}";

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

