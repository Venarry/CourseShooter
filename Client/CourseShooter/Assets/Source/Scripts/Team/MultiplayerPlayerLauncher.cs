using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerLauncher
{
    private const string MessagePlayerSpawn = "OnPlayerSpawn";

    private readonly TeamSelector _teamSelector;
    private readonly TeamMatchMultiplayerHandler _multiplayerHandler;

    public MultiplayerPlayerLauncher(TeamSelector teamSelector, TeamMatchMultiplayerHandler multiplayerHandler)
    {
        _teamSelector = teamSelector;
        _multiplayerHandler = multiplayerHandler;
    }

    public void Enable()
    {
        _multiplayerHandler.PlayerSpawned += OnPlayaerSpawned;
        _teamSelector.PlayerLaunched += OnPlayerlaunched;
    }

    public void Disable()
    {
        _multiplayerHandler.PlayerSpawned -= OnPlayaerSpawned;
        _teamSelector.PlayerLaunched -= OnPlayerlaunched;
    }

    private void OnPlayerlaunched(Dictionary<string, object> data)
    {
        _multiplayerHandler.SendPlayerData(MessagePlayerSpawn, data);
    }

    private void OnPlayaerSpawned(PlayerView player)
    {
        _teamSelector.SetPlayer(player);
    }
}
