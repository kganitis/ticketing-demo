using Domain.External;

namespace Application.Interfaces;

public interface IBookingPlatformClient
{
    Task NotifyResponseApprovedAsync(Guid ticketId, string message, string? action);
    Task<IEnumerable<BookingDto>> GetRecentBookingsAsync(DateTime from, DateTime to);
    Task<BookingDto?> GetBookingByCodeAsync(string bookingCode);
}