using MasPatas.Application.DTOs;
using MasPatas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasPatas.API.Controllers;

[ApiController]
[Route("api/products")]
[Authorize(Policy = "AdminOnly")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        => Ok(await _service.CreateProductAsync(request, cancellationToken));

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ProductDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetProductsAsync(cancellationToken));
}
