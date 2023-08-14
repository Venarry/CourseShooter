using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _message;

    public void SetText(string message)
    {
        _message.text = message;
    }
}
