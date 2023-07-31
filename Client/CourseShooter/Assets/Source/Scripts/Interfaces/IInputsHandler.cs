using UnityEngine;

public interface IInputsHandler
{
    public Vector3 MoveDirection { get; }
    public Vector3 RotationDirection { get; }
    public bool IsPressedKeyJump { get; }
}
