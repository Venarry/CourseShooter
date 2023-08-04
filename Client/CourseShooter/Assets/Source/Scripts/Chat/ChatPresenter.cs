using System;

public class ChatPresenter
{
    private readonly ChatModel _chatModel;
    private readonly ChatView _chatView;

    public ChatPresenter(ChatModel chatModel, ChatView chatView)
    {
        _chatModel = chatModel;
        _chatView = chatView;
    }

    public void Enable()
    {
        _chatModel.MessageHasSent += OnMessageSent;
    }

    public void Disable()
    {
        _chatModel.MessageHasSent -= OnMessageSent;
    }

    public void AddMessage(string message)
    {
        _chatModel.AddMessage(message);
    }

    private void OnMessageSent(string message)
    {
        _chatView.ShowMessage(message);
    }
}
