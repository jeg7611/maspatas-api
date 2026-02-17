using MasPatas.Application.DTOs;
using MasPatas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasPatas.API.Controllers;

[ApiController]
[Route("api/sales")]
[Authorize(Policy = "SellerOrAdmin")]
public class SalesController : ControllerBase
{
    private readonly SaleService _service;

    public SalesController(SaleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create([FromBody] RegisterSaleRequest request, CancellationToken cancellationToken)
        => Ok(await _service.RegisterSaleAsync(request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetSalesAsync(cancellationToken));
}
