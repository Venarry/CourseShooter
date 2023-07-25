using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _newPosition;

    public void OnChange(List<DataChange> dataChanges)
    {
        Vector3 targetPosition = transform.position;

        foreach (DataChange change in dataChanges)
        {
            switch (change.Field)
            {
                case "x":
                    targetPosition.x = (float)change.Value;
                    break;

                case "y":
                    targetPosition.y = (float)change.Value;
                    break;

                case "z":
                    targetPosition.z = (float)change.Value;
                    break;

                case "DirectionX":
                    _moveDirection.x = (float)change.Value;
                    break;

                case "DirectionY":
                    _moveDirection.y = (float)change.Value;
                    break;

                case "DirectionZ":
                    _moveDirection.z = (float)change.Value;
                    break;
            }
        }

        _newPosition = targetPosition;
    }

    private void Update()
    {
        InterpolateWithLerp();
    }

    private void InterpolateWithLerp()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier);
    }

    private void InterpolateWithPredicate()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition + _moveDirection, interpolationMultiplier);
    }
}
