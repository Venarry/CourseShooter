using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereBullet : Bullet
{
    public void Init(Transform shootPoint, int damage)
    {
        SetDamage(damage);
        SetShootPoint(shootPoint);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
