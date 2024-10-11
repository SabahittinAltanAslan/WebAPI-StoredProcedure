using Microsoft.AspNetCore.Mvc;
using OrderService.API.Models;
using OrderService.Repositories;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderRepository _orderRepository;

    public OrdersController(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> Get()
    {
        var orders = _orderRepository.GetOrders();
        return Ok(orders);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Order order)
    {
        var orderId = _orderRepository.AddOrder(order);
        return CreatedAtAction(nameof(Get), new { id = orderId }, order);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Order order)
    {
        order.OrderId = id;
        _orderRepository.UpdateOrder(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _orderRepository.DeleteOrder(id);
        return NoContent();
    }
}
