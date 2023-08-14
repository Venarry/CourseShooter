using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);

    public EnemyView Create(Player thisPlayer)
    {
        Vector3 spawnPosition = new(thisPlayer.Position.x, thisPlayer.Position.y, thisPlayer.Position.z);

        EnemyView enemy = Object.Instantiate(_prefab, spawnPosition, Quaternion.identity);

        PlayerWeaponModel weaponModel = new();
        PlayerWeaponPresenter weaponPresenter = new(weaponModel);

        enemy.GetComponent<PlayerWeaponView>().Init(weaponPresenter);

        HealthModel healthModel = new(maxValue: 100);
        HealthPresenter healthPresenter = new(healthModel);

        enemy.Init(healthPresenter, thisPlayer);

        return enemy;
    }
}
