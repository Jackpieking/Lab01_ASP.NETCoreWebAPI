using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTOs.Product.Incomings;
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

    /// <summary>
    ///     Create new product.
    /// </summary>
    /// <returns>
    ///     Http code.
    /// </returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/product
    ///     {
    ///         "productName": "string",
    ///         "categoryId": int,
    ///         "unitPrice": decimal,
    ///         "unitsInStock": int
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation fail.</response>
    /// <response code="500">Server error.</response>
    [HttpPost]
    public async Task<IActionResult> PostProductAsync(
        [FromBody] CreateProductDto dto,
        CancellationToken ct
    )
    {
        // Is category found by category id.
        var isCategoryFound = await _unitOfWork.CategoryRepository.IsCategoryFoundByCategoryIdAsync(
            categoryId: dto.CategoryId,
            ct: ct
        );

        if (!isCategoryFound)
        {
            return NotFound(value: $"Category with id = {dto.CategoryId} is not found !!");
        }

        // Is product name found by product name.
        var isProductNameFound =
            await _unitOfWork.ProductRepository.IsProductFoundByProductNameAsync(
                productName: dto.ProductName,
                ct: ct
            );

        if (isProductNameFound)
        {
            return Conflict(error: $"Product with name = {dto.ProductName} already exists !!");
        }

        // Create new product and add it to database.
        var dbResult = await _unitOfWork.ProductRepository.CreateProductAsync(
            newProduct: InitNewProduct(),
            ct: ct
        );

        if (!dbResult)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();

        Product InitNewProduct()
        {
            return new()
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock
            };
        }
    }

    [HttpDelete(template: "{productId:int}")]
    public async Task<IActionResult> DeleteProductAsync(
        [FromRoute] [Required] [Range(minimum: default, maximum: int.MaxValue)] int productId,
        CancellationToken ct
    )
    {
        // Is product found by product id.
        var isProductFound = await _unitOfWork.ProductRepository.IsProductFoundByProductIdAsync(
            productId: productId,
            ct: ct
        );

        if (!isProductFound)
        {
            return NotFound(value: $"Product is not found by product Id [{productId}] !!");
        }

        // Delete product.
        var dbResult = await _unitOfWork.ProductRepository.RemoveProductAsync(
            productId: productId,
            ct: ct
        );

        if (!dbResult)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }
}
