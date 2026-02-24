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

    [HttpPost("sell")]
    public async Task<ActionResult<SaleDto>> Sell([FromBody] CreateSaleCommand request, CancellationToken cancellationToken)
        => Ok(await _service.CreateSaleAsync(request, cancellationToken));

    [HttpPost("pay")]
    public async Task<ActionResult<SaleDto>> Pay([FromBody] RegisterPaymentCommand request, CancellationToken cancellationToken)
        => Ok(await _service.RegisterPaymentAsync(request, cancellationToken));

    [HttpPost("cancel")]
    public async Task<ActionResult<SaleDto>> Cancel([FromBody] CancelSaleCommand request, CancellationToken cancellationToken)
        => Ok(await _service.CancelSaleAsync(request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetSalesAsync(cancellationToken));
}
