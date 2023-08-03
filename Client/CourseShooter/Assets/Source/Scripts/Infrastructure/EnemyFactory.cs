using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);

    public EnemyView Create(Player thisPlayer)
    {
        Vector3 position = new(thisPlayer.Position.x, thisPlayer.Position.y, thisPlayer.Position.z);

        EnemyView enemy = Object.Instantiate(_prefab, position, Quaternion.identity);
        PlayerWeaponModel weaponModel = new();
        PlayerWeaponPresenter weaponPresenter = new(weaponModel);
        enemy.Init(weaponPresenter, thisPlayer);
        //enemy.SwitchWeapon((int)thisPlayer.ActiveWeapon);

        return enemy;
    }
}
