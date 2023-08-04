using System;
using System.Collections.Generic;

public class ChatModel
{
    public List<string> _messages = new();

    public event Action<string> MessageHasSent;

    public void AddMessage(string message)
    {
        _messages.Add(message);
        MessageHasSent?.Invoke(message);
    }
}
