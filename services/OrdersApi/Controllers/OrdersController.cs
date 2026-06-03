using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        CreateOrderRequest request)
    {
        var order = await _orderService.CreateOrderAsync(request);

        return Ok(order);
    }
}