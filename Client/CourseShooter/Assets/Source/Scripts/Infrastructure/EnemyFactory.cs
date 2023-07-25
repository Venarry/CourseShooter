using UnityEngine;

public class EnemyFactory
{
    private readonly EnemyMovement _prefab = Resources.Load<EnemyMovement>(ResourcesPath.EnemyPrefab);

    public EnemyMovement Create(Vector3 position)
    {
        EnemyMovement enemy = Object.Instantiate(_prefab, position, Quaternion.identity);

        return enemy;
    }
}
