public class MessageEvent
{
    public string Message { get; private set; }

    public MessageEvent(string message)
    {
        Message = message;
    }
}
