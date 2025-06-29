using Application.Interfaces;
using Infrastructure.Llm;
using Infrastructure.BookingPlatform;
using Infrastructure.Tickets;

var builder = WebApplication.CreateBuilder(args);

var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrWhiteSpace(openAiKey))
{
    Console.WriteLine("Missing OPENAI_API_KEY environment variable.");
    throw new InvalidOperationException("OpenAI key not set.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILlmService>(_ => new OpenAiLlmService(openAiKey));
builder.Services.AddSingleton<IBookingPlatformClient, MockBookingPlatformClient>();
builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();

// builder.Services.AddHttpClient<IBookingPlatformClient, LinkTwistClient>(client =>
// {
//     client.BaseAddress = new Uri("https://api.link-twist.com");
// });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketingDemo API");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", () => "ticketingdemo up and running");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();