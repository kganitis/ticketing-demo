using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CustomerMessage { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Received;

    public Response? ProposedResponse { get; private set; }

    public Ticket(string customerMessage)
    {
        if (string.IsNullOrWhiteSpace(customerMessage))
            throw new ArgumentException("Message is required.", nameof(customerMessage));

        CustomerMessage = customerMessage;
    }

    public void ProposeResponse(Response response)
    {
        if (Status != TicketStatus.Received)
            throw new InvalidOperationException("Can only propose a response when ticket is received.");

        ProposedResponse = response;
        Status = TicketStatus.UnderReview;
    }

    public void ApproveResponse()
    {
        if (Status != TicketStatus.UnderReview || ProposedResponse is null)
            throw new InvalidOperationException("Cannot approve without a proposed response.");

        Status = TicketStatus.Approved;
    }

    public void RejectResponse()
    {
        if (Status != TicketStatus.UnderReview || ProposedResponse is null)
            throw new InvalidOperationException("Cannot reject without a proposed response.");

        ProposedResponse = null;
        Status = TicketStatus.Rejected;
    }
}