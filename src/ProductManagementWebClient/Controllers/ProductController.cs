using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProductManagementWebClient.Commons;
using ProductManagementWebClient.Models;

namespace ProductManagementWebClient.Controllers;

public class ProductController : Controller
{
    private const string ProductApiEndpoint = "https://localhost:7114/api/product";
    private const string CategoryApiEndpoint = "https://localhost:7114/api/category";
    private readonly HttpClient _httpClient;

    public ProductController()
    {
        _httpClient = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync(ProductApiEndpoint);
        var json = await response.Content.ReadAsStringAsync();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var productList = JsonSerializer.Deserialize<List<ProductEntity>>(json, jsonSerializerOptions);

        return View(productList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await GetCategoriesAsync();

        return View();
    }

    private async Task GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(CategoryApiEndpoint);

        var json = await response.Content.ReadAsStringAsync();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var categoryList = JsonSerializer.Deserialize<List<CategoryEntity>>(json, jsonSerializerOptions);

        ViewData[ViewDataKeys.CategoryList] = categoryList;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"{ProductApiEndpoint}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var product = JsonSerializer.Deserialize<ProductEntity>(json, jsonSerializerOptions);

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEntity product)
    {
        var json = JsonSerializer.Serialize(product);

        var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(ProductApiEndpoint, bodyContent);

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"{ProductApiEndpoint}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var product = JsonSerializer.Deserialize<ProductEntity>(json, jsonSerializerOptions);

        await GetCategoriesAsync();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEntity product)
    {
        var json = JsonSerializer.Serialize(product);

        var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{ProductApiEndpoint}/{product.ProductId}", bodyContent);

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{ProductApiEndpoint}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            ViewData[ViewDataKeys.ErrorMessage] = "Product is not found.";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
