using Application.Interfaces;
using Domain.Entities;
using System.Collections.Concurrent;

namespace Infrastructure.Tickets;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly ConcurrentDictionary<Guid, Ticket> _storage = new();

    public Task AddAsync(Ticket ticket)
    {
        if (!_storage.TryAdd(ticket.Id, ticket))
            throw new InvalidOperationException($"Ticket with ID {ticket.Id} already exists.");
        return Task.CompletedTask;
    }

    public Task<Ticket?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var ticket);
        return Task.FromResult(ticket);
    }

    public Task SaveAsync(Ticket ticket)
    {
        _storage[ticket.Id] = ticket;
        return Task.CompletedTask;
    }
}