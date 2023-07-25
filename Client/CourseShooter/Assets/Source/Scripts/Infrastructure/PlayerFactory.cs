using UnityEngine;

public class PlayerFactory
{
    private readonly PlayerMovement _prefab = Resources.Load<PlayerMovement>(ResourcesPath.PlayerPrefab);

    public PlayerMovement Create(Vector3 position)
    {
        PlayerMovement player = Object.Instantiate(_prefab, position, Quaternion.identity);

        IInputsHandler inputsHandler = new KeyboardInputsHandler();
        player.Init(inputsHandler);

        return player;
    }
}
