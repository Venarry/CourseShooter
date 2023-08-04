using System;
using TMPro;
using UnityEngine;

public class ChatView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Transform _messagePoint;
    [SerializeField] private TMP_Text _messagePrefab;

    private ChatPresenter _chatPresenter;
    public event Action<string> MessageSubmited;

    private void Awake()
    {
        _chatPresenter = new(new ChatModel(), this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _inputField.Select();
        }
    }

    private void OnEnable()
    {
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
        spawnedMessage.transform.SetParent(_messagePoint, true);
        spawnedMessage.transform.localScale = Vector3.one;
        spawnedMessage.text = message;
    }

    public void OnMessageSubmit(string message)
    {
        MultiplayerHandler.Instance.SendPlayerData("MessageSent", message);
    }
}
