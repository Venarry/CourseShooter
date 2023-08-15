using System.Collections;
using UnityEngine;

public class MultiplayerPlayerDieHandler : MonoBehaviour
{
    private const float RespawnTime = 3;

    private Camera _followCamera;
    private MainCameraHolder _cameraHolder;
    private PlayerView _playerView;
    private PlayerRespawner _playerRespawner;
    private WaitForSeconds _waitForSeconds = new(RespawnTime);

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
        _waitForSeconds = new(RespawnTime);
    }

    public void Init(Camera followCamera, MainCameraHolder mainCameraHolder, PlayerRespawner playerRespawner)
    {
        _followCamera = followCamera;
        _cameraHolder = mainCameraHolder;
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

    private void OnKilled(ShooterData ownerData)
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        _playerView.SetBehaviourState(false);
        _cameraHolder.SetCamera(_followCamera);

        yield return _waitForSeconds;

        _playerView.SetBehaviourState(true);
        _followCamera.gameObject.SetActive(false);
        _playerRespawner.Respawn(_playerView);
    }
}
