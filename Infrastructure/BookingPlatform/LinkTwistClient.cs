using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Application.Interfaces;
using Domain.External;

namespace Infrastructure.BookingPlatform;

public class LinkTwistClient(HttpClient httpClient, IConfiguration config)
    : IBookingPlatformClient
{
    private readonly string _apiKey = config["LINKTWIST_API_KEY"] ??
                                      throw new InvalidOperationException(
                                          "Missing LINKTWIST_API_KEY");

    public async Task NotifyResponseApprovedAsync(Guid ticketId, string message,
        string? action)
    {
        // TODO Replace with real LinkTwist call
        
        Console.WriteLine(
            $"[LinkTwist] Would send approved response for ticket {ticketId}.");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine($"Action: {action ?? "none"}");
        
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<BookingDto>> GetRecentBookingsAsync(DateTime from,
        DateTime to)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"/bookings?booking_date_time_from={from:s}&booking_date_time_to={to:s}");
        request.Headers.Add("API-Key", _apiKey);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<BookingResponse>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Select(b => new BookingDto(b.Code!,
                   DateTime.Parse(b.CreatedAt ?? DateTime.MinValue.ToString()),
                   b.ChannelExtraInfo)).ToList()
               ?? Enumerable.Empty<BookingDto>();
    }

    public async Task<BookingDto?> GetBookingByCodeAsync(string bookingCode)
    {
        var request =
            new HttpRequestMessage(HttpMethod.Get, $"/bookings/{bookingCode}");
        request.Headers.Add("API-Key", _apiKey);

        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var booking = JsonSerializer.Deserialize<BookingResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return booking is null
            ? null
            : new BookingDto(booking.Code!,
                DateTime.Parse(booking.CreatedAt ?? DateTime.MinValue.ToString()),
                booking.ChannelExtraInfo);
    }

    private class BookingResponse
    {
        public string? Code { get; set; }
        public string? CreatedAt { get; set; }
        public string? ChannelExtraInfo { get; set; }
    }
}