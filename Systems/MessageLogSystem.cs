public class MessageLogSystem
{
    private readonly List<string> messages = new List<string>();
    private readonly int maxMessages;
    private int messageWidth = GameConfig.Instance.messageWidth;
    private int gameWidth = GameConfig.Instance.gameWidth;
    private int consoleHeight = GameConfig.Instance.consoleHeight;


    public MessageLogSystem(int maxMessages)
    {
        this.maxMessages = maxMessages;
    }

    public void OnMessageEvent(MessageEvent messageEvent)
    {
        Add(messageEvent.Message);
    }

    private void Add(string message)
    {
        messages.Add(message);
        if (messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }
    }

    public List<string> GetMessages()
    {
        return messages;
    }

    public void DisplayMessages()
    {
        var messages = GetMessages();
        // Assuming the console height is enough to accommodate maxMessages without adjustment
        int startY = consoleHeight - maxMessages; // This might need adjusting based on your needs
        for (int i = 0; i < messages.Count; i++)
        {
            // Set the cursor position to start right after the game area for x, and calculate y based on the index
            Console.SetCursorPosition(gameWidth, startY + i);
            // Ensure the message fits in the message area, potentially trimming it if it's too long
            string messageToDisplay = messages[i].Length > messageWidth ? messages[i].Substring(0, messageWidth) : messages[i];
            Console.Write(messageToDisplay.PadRight(messageWidth));
        }
    }

public void LogMessage(string message)
{
    // Break the message into lines that fit within the messageWidth.
    List<string> lines = WordWrap(message, messageWidth);

    foreach (var line in lines)
    {
        Add(line); // Add each line to the message log.

        // Ensure old messages are removed if exceeding maxMessages.
        while (messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }
    }
}

// Helper method to wrap text at word boundaries.
private List<string> WordWrap(string text, int lineWidth)
{
    List<string> lines = new List<string>();
    var words = text.Split(' ');
    var currentLine = "";

    foreach (var word in words)
    {
        if ((currentLine.Length > 0) && (currentLine.Length + word.Length >= lineWidth))
        {
            lines.Add(currentLine);
            currentLine = "";
        }

        if (currentLine.Length > 0)
            currentLine += " ";
        currentLine += word;
    }

    if (currentLine.Length > 0)
        lines.Add(currentLine);

    return lines;
}


    public void ClearMessages()
    {
        for (int line = Console.WindowHeight - maxMessages; line < Console.WindowHeight; line++)
        {
            Console.SetCursorPosition(messageWidth, line);
            Console.Write(new string('.', messageWidth));
        }
    }
}
