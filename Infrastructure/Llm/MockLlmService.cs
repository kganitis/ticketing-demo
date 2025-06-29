using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Llm;

public class MockLlmService : ILlmService
{
    public Task<Response> GenerateResponseAsync(Ticket ticket)
    {
        var mockMessage = $"[LLM] Auto-reply to: \"{ticket.CustomerMessage}\"";
        var mockAction = ticket.CustomerMessage.ToLower().Contains("cancel")
            ? "CancelReservation"
            : null;

        var response = new Response(mockMessage, mockAction);
        return Task.FromResult(response);
    }
}