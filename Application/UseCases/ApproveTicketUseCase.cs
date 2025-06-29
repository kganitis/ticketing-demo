using Application.Interfaces;

namespace Application.UseCases;

public class ApproveTicketUseCase(ITicketRepository ticketRepository, IBookingPlatformClient bookingPlatformClient)
{
    public async Task ExecuteAsync(Guid ticketId)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId)
                     ?? throw new ArgumentException("Ticket not found");

        ticket.ApproveResponse();
        await ticketRepository.SaveAsync(ticket);

        await bookingPlatformClient.NotifyResponseApprovedAsync(
            ticket.Id,
            ticket.ProposedResponse!.Message,
            ticket.ProposedResponse.ActionToTake
        );
    }
}
