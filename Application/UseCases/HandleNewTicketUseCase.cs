using Application.Interfaces;

namespace Application.UseCases;

public class HandleNewTicketUseCase(
    ITicketRepository ticketRepository,
    ILlmService llmService)
{
    public async Task ExecuteAsync(Guid ticketId)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId);
        if (ticket is null)
            throw new ArgumentException("Ticket not found");

        var response = await llmService.GenerateResponseAsync(ticket);
        ticket.ProposeResponse(response);
        await ticketRepository.SaveAsync(ticket);
    }
}