using Application.Interfaces;

namespace Application.UseCases;

public class RejectTicketUseCase(ITicketRepository ticketRepository)
{
    public async Task ExecuteAsync(Guid ticketId)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId);
        if (ticket is null)
            throw new ArgumentException("Ticket not found");

        ticket.RejectResponse();
        await ticketRepository.SaveAsync(ticket);
    }
}