using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id);
    Task SaveAsync(Ticket ticket);
    Task AddAsync(Ticket ticket);
}