using System.Collections;
using UnityEngine;

public class MultiplayerPlayerDieHandler : MonoBehaviour
{
    private const float RespawnTime = 3;

    private Camera _followCamera;
    private PlayerView _playerView;
    private PlayerRespawner _playerRespawner;
    private WaitForSeconds _waitForSeconds = new(RespawnTime);

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
        _waitForSeconds = new(RespawnTime);
    }

    public void Init(Camera followCamera, PlayerRespawner playerRespawner)
    {
        _followCamera = followCamera;
        _playerRespawner = playerRespawner;
    }

    private void OnEnable()
    {
        _playerView.Killed += OnKilled;
    }

    private void OnDisable()
    {
        _playerView.Killed -= OnKilled;
    }

    private void OnKilled(ShootData ownerData)
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        _playerView.SetBehaviourState(false);
        _followCamera.gameObject.SetActive(true);

        yield return _waitForSeconds;

        _playerView.SetBehaviourState(true);
        _followCamera.gameObject.SetActive(false);
        _playerRespawner.Respawn(_playerView);
    }
}
