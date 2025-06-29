namespace Domain.ValueObjects;

public sealed class Response
{
    public string Message { get; }
    public string? ActionToTake { get; }

    public Response(string message, string? actionToTake = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        Message = message;
        ActionToTake = actionToTake;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Response other) return false;
        return Message == other.Message && ActionToTake == other.ActionToTake;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, ActionToTake);
    }
}