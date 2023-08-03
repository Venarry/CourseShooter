using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected int Damage { get; private set; }

    protected void SetShootPoint(Transform shootPoint)
    {
        transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
    }

    protected void SetDamage(int damage)
    {
        if(damage < 0)
            damage = 0;

        Damage = damage;
    }
}
