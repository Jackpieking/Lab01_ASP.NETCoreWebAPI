using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTOs.Product.Outgoings;

namespace ProjectManagementAPI.Controllers;

[Produces(contentType: MediaTypeNames.Application.Json)]
[ApiController]
[Route(template: "api/[controller]")]
public sealed class ProductController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Get all products.
    /// </summary>
    /// <returns>
    ///     A list of all product.
    /// </returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/product
    ///
    /// </remarks>
    /// <response code="200">A list of all products.</response>
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync(CancellationToken ct)
    {
        var foundProducts = await _unitOfWork.ProductRepository.GetProductsAsync(ct: ct);

        return Ok(
            value: foundProducts.Select(selector: foundProduct => new GetProductsDto
            {
                ProductId = foundProduct.ProductId,
                ProductName = foundProduct.ProductName,
                CategoryId = foundProduct.CategoryId,
                UnitPrice = foundProduct.UnitPrice,
                UnitsInStock = foundProduct.UnitsInStock
            })
        );
    }
}
