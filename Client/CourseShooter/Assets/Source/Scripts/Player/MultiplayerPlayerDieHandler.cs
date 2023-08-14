using System.Collections;
using UnityEngine;

public class MultiplayerPlayerDieHandler : MonoBehaviour
{
    private const float RespawnTime = 3;

    private Camera _followCamera;
    private PlayerView _playerView;
    private SpawnPointsDataSource _spawnPointsDataSource;
    private WaitForSeconds _waitForSeconds = new(RespawnTime);

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
        _waitForSeconds = new(RespawnTime);
    }

    public void Init(Camera followCamera, SpawnPointsDataSource spawnPointsDataSource)
    {
        _followCamera = followCamera;
        _spawnPointsDataSource = spawnPointsDataSource;
    }

    private void OnEnable()
    {
        _playerView.Killed += OnKilled;
    }

    private void OnDisable()
    {
        _playerView.Killed -= OnKilled;
    }

    private void OnKilled(OwnerData ownerData)
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        _playerView.SetBehaviourState(false);
        _followCamera.gameObject.SetActive(true);

        yield return _waitForSeconds;

        Vector3 respawnPosition = _spawnPointsDataSource.GetRandomSpawnPosition(_playerView.TeamIndex);

        _playerView.SetBehaviourState(true);
        _followCamera.gameObject.SetActive(false);
        _playerView.Respawn(respawnPosition);
    }
}
