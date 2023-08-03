using UnityEngine;

public class PlayerFactory
{
    private readonly PlayerView _prefab = Resources.Load<PlayerView>(ResourcesPath.PlayerPrefab);
    private readonly WeaponFactory _weaponFactory = new();

    public PlayerView Create(MultiplayerHandler multiplayerHandler, Vector3 position)
    {
        PlayerView player = Object.Instantiate(_prefab, position, Quaternion.identity);

        PlayerWeaponModel playerWeaponModel = new();
        PlayerWeaponPresenter playerWeaponPresenter = new(playerWeaponModel);
        IInputsHandler inputsHandler = new KeyboardInputsHandler();
        player.Init(multiplayerHandler, playerWeaponPresenter, inputsHandler);

        WeaponView minigun = _weaponFactory.Create(ResourcesPath.Minigun);
        WeaponView pistol = _weaponFactory.Create(ResourcesPath.Pistol);
        player.AddWeapon(minigun);
        player.AddWeapon(pistol);

        return player;
    }
}
