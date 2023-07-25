using UnityEngine;

public interface IInputsHandler
{
    public Vector3 MoveDirection { get; }
    public bool IsPressedKeyJump { get; }
}
