using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected int Damage { get; private set; }
    protected ShooterData ShootData { get; private set; }

    protected void SetShootPoint(Vector3 shootPoint)
    {
        transform.position = shootPoint;
    }

    protected void SetShootRotation(Vector3 shootPoint)
    {
        transform.eulerAngles = shootPoint;
    }

    protected void SetDamage(int damage)
    {
        if(damage < 0)
            damage = 0;

        Damage = damage;
    }

    protected void SetShootData(ShooterData shootData)
    {
        ShootData = shootData;
    }
}
