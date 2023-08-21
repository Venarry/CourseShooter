using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);
    private MainCameraHolder _cameraHolder;
    private TeamMatchMultiplayerHandler _stateHandlerRoom;

    public void SetEnemyData(MainCameraHolder mainCameraHolder, TeamMatchMultiplayerHandler stateHandlerRoom)
    {
        _cameraHolder = mainCameraHolder;
        _stateHandlerRoom = stateHandlerRoom;
    }

    public EnemyView Create(Player thisPlayer, string sessionId)
    {
        Vector3 spawnPosition = new(thisPlayer.Position.x, thisPlayer.Position.y, thisPlayer.Position.z);

        EnemyView enemy = Object.Instantiate(_prefab, spawnPosition, Quaternion.identity);

        PlayerWeaponModel weaponModel = new();
        PlayerWeaponPresenter weaponPresenter = new(weaponModel);

        enemy.GetComponent<PlayerWeaponView>().Init(weaponPresenter);

        HealthModel healthModel = new(maxValue: 100);
        HealthPresenter healthPresenter = new(healthModel);
        enemy.GetComponent<EnemyHealthView>().Init(healthPresenter, _cameraHolder);

        enemy.Init(thisPlayer, _cameraHolder, _stateHandlerRoom, sessionId);

        return enemy;
    }
}
