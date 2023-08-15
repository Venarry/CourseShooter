using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereBullet : Bullet
{
    private float _destroyTimer = 8f;
    private float _lifeTime;

    public void Init(Transform shootPoint, int damage)
    {
        SetDamage(damage);
        SetShootPoint(shootPoint);
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
            ShootData ownerData = new();
            damageable.TakeDamage(Damage, ownerData);
            Destroy(gameObject);
        }
    }
}
