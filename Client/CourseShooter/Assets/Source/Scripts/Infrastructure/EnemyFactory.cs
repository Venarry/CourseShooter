using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);

    public EnemyView Create(Player thisPlayer, Vector3 position)
    {
        EnemyView enemy = Object.Instantiate(_prefab, position, Quaternion.identity);
        PlayerWeaponModel weaponModel = new();
        PlayerWeaponPresenter weaponPresenter = new(weaponModel);
        enemy.Init(weaponPresenter, thisPlayer);

        return enemy;
    }
}
