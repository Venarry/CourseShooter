using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereBullet : Bullet
{
    private float _destroyTimer = 8f;
    private float _lifeTime;

    public void Init(Vector3 shootPoint, Vector3 shootDirection, ShooterData shooterData, int damage)
    {
        SetDamage(damage);
        SetShootPoint(shootPoint);
        SetShootRotation(shootDirection);
        SetShootData(shooterData);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * 0.05f;

        _lifeTime += Time.deltaTime;

        if(_lifeTime >= _destroyTimer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Damage, ShootData);
            Destroy(gameObject);
        }
    }
}
