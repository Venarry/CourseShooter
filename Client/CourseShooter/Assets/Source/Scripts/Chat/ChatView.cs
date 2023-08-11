using System;
using TMPro;
using UnityEngine;

public class ChatView : MonoBehaviour
{
    [SerializeField] private GameObject _chat;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Transform _messagePoint;
    [SerializeField] private TMP_Text _messagePrefab;

    private ChatPresenter _chatPresenter;

    private void Awake()
    {
        _chatPresenter = new(new ChatModel(), this);

        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Show();
        }
    }

    private void OnEnable()
    {
        _inputField.onSelect.AddListener(OnChatSelect);
        _inputField.onDeselect.AddListener(OnChatDeselect);

        _inputField.onSubmit.AddListener(OnMessageSubmit);
        _chatPresenter.Enable();
    }

    public void SentMessage(string message)
    {
        _chatPresenter.AddMessage(message);
    }

    public void ShowMessage(string message)
    {
        TMP_Text spawnedMessage = Instantiate(_messagePrefab, Vector3.zero, Quaternion.identity);
        spawnedMessage.transform.SetParent(_messagePoint);
        spawnedMessage.transform.SetAsFirstSibling();
        spawnedMessage.transform.localScale = Vector3.one;
        spawnedMessage.text = message;
    }

    public void OnMessageSubmit(string message)
    {
        if (message == "")
            return;

        MultiplayerHandler.Instance.SendPlayerData("MessageSent", message);
        _inputField.text = "";
        _inputField.ActivateInputField();
    }

    public void OnChatSelect(string message)
    {
        Debug.Log("select");
        PauseHandler.AddPauseLevel();
        MapSettings.ShowCursor();
    }

    public void OnChatDeselect(string message)
    {
        Debug.Log("deselect");
        PauseHandler.RemovePauseLevel();
        MapSettings.HideCursor();
        Hide();
    }

    private void Show()
    {
        _chat.SetActive(true);
        _inputField.Select();
    }

    private void Hide()
    {
        _chat.SetActive(false);
    }
}
