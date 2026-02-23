using MasPatas.Application.DTOs;
using MasPatas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasPatas.API.Controllers;

[ApiController]
[Route("api/inventory/movements")]
[Authorize(Policy = "SellerOrAdmin")]
public class InventoryMovementsController : ControllerBase
{
    private readonly InventoryService _service;

    public InventoryMovementsController(InventoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<InventoryMovementDto>> Create([FromBody] AddInventoryMovementRequest request, CancellationToken cancellationToken)
        => Ok(await _service.AddInventoryMovementAsync(request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<List<InventoryMovementDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetInventoryMovementsAsync(cancellationToken));
}
