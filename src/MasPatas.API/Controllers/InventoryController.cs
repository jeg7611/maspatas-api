using MasPatas.Application.DTOs;
using MasPatas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasPatas.API.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize(Policy = "SellerOrAdmin")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _service;

    public InventoryController(InventoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<InventoryDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetInventoryAsync(cancellationToken));
}
