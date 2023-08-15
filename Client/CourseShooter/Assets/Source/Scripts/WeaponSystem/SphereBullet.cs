using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereBullet : Bullet
{
    private float _destroyTimer = 8f;
    private float _lifeTime;

    public void Init(Transform shootPoint, int damage, ShooterData shootData)
    {
        SetDamage(damage);
        SetShootPoint(shootPoint);
        SetShootData(shootData);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward;

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
