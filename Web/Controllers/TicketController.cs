using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[ApiController]
[Route("tickets")]
public class TicketController(
    ITicketRepository ticketRepository,
    ILlmService llmService,
    IBookingPlatformClient bookingPlatformClient) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request)
    {
        var ticket = new Ticket(request.CustomerMessage);
        await ticketRepository.AddAsync(ticket);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, new { ticket.Id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await ticketRepository.GetByIdAsync(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost("{id:guid}/auto-handle")]
    public async Task<IActionResult> AutoHandle(Guid id)
    {
        var useCase = new HandleNewTicketUseCase(ticketRepository, llmService);
        await useCase.ExecuteAsync(id);
        return Ok();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var useCase = new ApproveTicketUseCase(ticketRepository, bookingPlatformClient);
        await useCase.ExecuteAsync(id);
        return Ok();
    }
}