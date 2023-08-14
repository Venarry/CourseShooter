using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory
{
    private readonly PlayerView _prefab = Resources.Load<PlayerView>(ResourcesPath.PlayerPrefab);
    private readonly WeaponFactory _weaponFactory = new();

    public PlayerView Create(MultiplayerHandler multiplayerHandler, 
        SpawnPointsDataSource spawnPointsDataSource,
        Camera followCamera, 
        Vector3 position, 
        int teamNumber, 
        bool isMultiplayer)
    {
        PlayerView player = Object.Instantiate(_prefab, position, Quaternion.identity);

        PlayerWeaponModel playerWeaponModel = new();
        PlayerWeaponPresenter playerWeaponPresenter = new(playerWeaponModel);

        IInputsHandler inputsHandler = new KeyboardInputsHandler();
        player.GetComponent<PlayerWeaponView>().Init(playerWeaponPresenter);

        int maxHealth = 100;

        HealthModel healthModel = new(maxHealth);
        HealthPresenter healthPresenter = new(healthModel);

        if (isMultiplayer == true)
        {
            player.AddComponent<PlayerMultiplayerHandler>().Init(multiplayerHandler);
            player.AddComponent<MultiplayerPlayerDieHandler>().Init(followCamera, spawnPointsDataSource);
        }

        player.Init(healthPresenter, inputsHandler, teamNumber);
        player.SetTeamIndex(teamNumber);

        WeaponView minigun = _weaponFactory.Create(ResourcesPath.Minigun);
        player.AddWeapon(minigun);

        WeaponView pistol = _weaponFactory.Create(ResourcesPath.Pistol);
        player.AddWeapon(pistol);

        return player;
    }
}
