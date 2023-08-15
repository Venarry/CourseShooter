using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory
{
    private readonly PlayerView _prefab = Resources.Load<PlayerView>(ResourcesPath.PlayerPrefab);
    private readonly WeaponFactory _weaponFactory = new();

    private MultiplayerHandler _multiplayerHandler;
    private SpawnPointsDataSource _spawnPointsDataSource;
    private Camera _deathCamera;

    public void SetPlayerData(MultiplayerHandler multiplayerHandler, 
        SpawnPointsDataSource spawnPointsDataSource,
        Camera deathCamera)
    {
        _multiplayerHandler = multiplayerHandler;
        _spawnPointsDataSource = spawnPointsDataSource;
        _deathCamera = deathCamera;
    }

    public PlayerView Create(/*MultiplayerHandler multiplayerHandler, 
        SpawnPointsDataSource spawnPointsDataSource,
        Camera followCamera, */
        Vector3 position, 
        int teamIndex, 
        bool isMultiplayer)
    {
        PlayerRespawner playerRespawner = new(_spawnPointsDataSource);

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
            player.AddComponent<PlayerMultiplayerHandler>().Init(_multiplayerHandler);
            player.AddComponent<MultiplayerPlayerDieHandler>().Init(_deathCamera, playerRespawner);
        }

        player.Init(healthPresenter, inputsHandler, teamIndex);
        player.SetTeamIndex(teamIndex);

        WeaponView minigun = _weaponFactory.Create(ResourcesPath.Minigun);
        player.AddWeapon(minigun);

        WeaponView pistol = _weaponFactory.Create(ResourcesPath.Pistol);
        player.AddWeapon(pistol);

        return player;
    }
}
