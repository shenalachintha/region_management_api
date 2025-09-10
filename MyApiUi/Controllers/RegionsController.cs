using Microsoft.AspNetCore.Mvc;
using MyApiUi.Models;
using MyApiUi.Models.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApiUi.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> regions = new List<RegionDto>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7159/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                var apiRegions = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
                if (apiRegions != null)
                {
                    regions.AddRange(apiRegions);
                }
            }
            catch (Exception ex)
            {
                // Optionally log the exception
            }

            return View("Index3", regions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Index4", new AddRegionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRegionViewModel addRegionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index4", new AddRegionViewModel());
            }

            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7159/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Redirect to Index to show updated region list
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to add region.");
            return View("Index4", addRegionViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        { 
            var client = httpClientFactory.CreateClient();
           var httpResponseMessage= await client.GetFromJsonAsync<RegionDto>($"https://localhost:7159/api/regions/{id.ToString()}");
            if(httpResponseMessage is not null)
            {
                return View("Edit", httpResponseMessage);
            }

            return View(null);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(RegionDto request) 
        { 
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7159/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage=await client.SendAsync(httpRequestMessage);
            var responseContent=await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if(responseContent is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage=await client.DeleteAsync($"https://localhost:7159/api/regions/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Regions");
 ;

            }
            catch (Exception ex)
            {
            }
            return View("Edit");
        }
    }
}