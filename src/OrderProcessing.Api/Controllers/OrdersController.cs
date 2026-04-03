using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.CreateOrder.Commands;

namespace OrderProcessing.Api.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommand createOrderCommand)
    {
        await _mediator.Send(createOrderCommand);
        return Accepted();
    }
}
