using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputsHandler : IInputsHandler
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";

    public Vector3 MoveDirection => new(Input.GetAxisRaw(Horizontal), 0, Input.GetAxisRaw(Vertical));
    public Vector3 RotationDirection => new(Input.GetAxisRaw(MouseX), Input.GetAxisRaw(MouseY), 0);
    public bool IsPressedShoot => Input.GetMouseButton(0);
    public bool IsPressedNextWeapon => Input.GetKeyDown(KeyCode.Q);
    public bool IsPressedKeyJump => Input.GetKey(KeyCode.Space);
}
