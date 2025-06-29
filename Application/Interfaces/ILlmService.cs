using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Interfaces;

public interface ILlmService
{
    Task<Response> GenerateResponseAsync(Ticket ticket);
}