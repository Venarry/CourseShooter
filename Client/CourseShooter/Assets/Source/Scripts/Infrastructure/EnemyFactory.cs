using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyView _prefab = Resources.Load<EnemyView>(ResourcesPath.EnemyPrefab);

    public EnemyView Create(Vector3 position)
    {
        EnemyView enemy = Object.Instantiate(_prefab, position, Quaternion.identity);

        return enemy;
    }
}
