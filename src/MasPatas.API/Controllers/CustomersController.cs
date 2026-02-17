using MasPatas.Application.DTOs;
using MasPatas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasPatas.API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _service;

    public CustomersController(CustomerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
        => Ok(await _service.CreateCustomerAsync(request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> Get(CancellationToken cancellationToken)
        => Ok(await _service.GetCustomersAsync(cancellationToken));
}
