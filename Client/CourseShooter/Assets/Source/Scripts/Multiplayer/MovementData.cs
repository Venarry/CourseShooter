using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementData
{
    public Vector3 Position;
    public Vector3 Direction;

    public MovementData(Vector3 position, Vector3 direction)
    {
        Position = position;
        Direction = direction;
    }
}
