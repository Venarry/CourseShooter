using UnityEngine;

public class PlayerFactory
{
    private readonly PlayerView _prefab = Resources.Load<PlayerView>(ResourcesPath.PlayerPrefab);

    public PlayerView Create(Vector3 position)
    {
        PlayerView player = Object.Instantiate(_prefab, position, Quaternion.identity);

        IInputsHandler inputsHandler = new KeyboardInputsHandler();
        player.Init(inputsHandler);

        return player;
    }
}
