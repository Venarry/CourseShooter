using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInfo
{
    public Vector3 ShootPoint;
    public Vector3 ShootRotation;
    public Vector3 ShootDirection;
    public ShooterData ShooterData = new();

    public void SetShootPoint(Vector3 shootPoint)
    {
        ShootPoint = shootPoint;
    }

    public void SetShootRotation(Vector3 rotation)
    {
        ShootRotation = rotation;
    }

    public void SetShootDirection(Vector3 direction)
    {
        ShootDirection = direction;
    }

    public void SetShooterData(ShooterData shooterData)
    {
        ShooterData = shooterData;
    }

    public void SetTeamIndex(int index)
    {
        ShooterData.SetTeamIndex(index);
    }

    public void SetClientId(string id)
    {
        ShooterData.SetClientId(id);
    }
}
