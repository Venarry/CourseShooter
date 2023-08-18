using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);
    private MainCameraHolder _cameraHolder;

    public void SetEnemyData(MainCameraHolder mainCameraHolder)
    {
        _cameraHolder = mainCameraHolder;
    }

    public EnemyView Create(Player thisPlayer)
    {
        Vector3 spawnPosition = new(thisPlayer.Position.x, thisPlayer.Position.y, thisPlayer.Position.z);

        EnemyView enemy = Object.Instantiate(_prefab, spawnPosition, Quaternion.identity);

        PlayerWeaponModel weaponModel = new();
        PlayerWeaponPresenter weaponPresenter = new(weaponModel);

        enemy.GetComponent<PlayerWeaponView>().Init(weaponPresenter);

        HealthModel healthModel = new(maxValue: 100);
        HealthPresenter healthPresenter = new(healthModel);
        enemy.GetComponent<EnemyHealthView>().Init(healthPresenter, _cameraHolder);

        enemy.Init(thisPlayer, _cameraHolder);

        return enemy;
    }
}
