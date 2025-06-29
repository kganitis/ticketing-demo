using Application.Interfaces;
using Domain.External;

namespace Infrastructure.BookingPlatform;

public class MockBookingPlatformClient : IBookingPlatformClient
{
    public Task NotifyResponseApprovedAsync(Guid ticketId, string message, string? action)
    {
        Console.WriteLine($"[BookingPlatform] Ticket {ticketId} approved:");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine($"Action: {action ?? "none"}");
        return Task.CompletedTask;
    }

    public Task<IEnumerable<BookingDto>> GetRecentBookingsAsync(DateTime from, DateTime to)
    {
        Console.WriteLine($"[BookingPlatform] Mock fetch of bookings from {from} to {to}.");
        return Task.FromResult(Enumerable.Empty<BookingDto>());
    }

    public Task<BookingDto?> GetBookingByCodeAsync(string bookingCode)
    {
        Console.WriteLine($"[BookingPlatform] Mock fetch of booking with code: {bookingCode}.");
        return Task.FromResult<BookingDto?>(null);
    }
}