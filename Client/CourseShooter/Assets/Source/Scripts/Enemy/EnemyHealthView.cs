using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class EnemyHealthView : MonoBehaviour
{
    [SerializeField] private LookAtRotator _progressBarRotator;

    private ProgressBar _progressBar;
    private HealthPresenter _healthPresenter;
    private MainCameraHolder _cameraHolder;
    private bool _isInitialized = false;

    private void Awake()
    {
        _progressBar = GetComponent<ProgressBar>();
    }

    public void Init(HealthPresenter healthPresenter, MainCameraHolder mainCameraHolder)
    {
        gameObject.SetActive(false);

        _healthPresenter = healthPresenter;
        _cameraHolder = mainCameraHolder;

        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        OnCameraChanged(_cameraHolder.ActiveCamera);
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Enable();
        _healthPresenter.HealthChanged += OnHealthChanged;
        _cameraHolder.CameraChanged += OnCameraChanged;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Disable();
        _healthPresenter.HealthChanged -= OnHealthChanged;
        _cameraHolder.CameraChanged -= OnCameraChanged;
    }

    public void TakeDamage(int value, ShooterData shooterData)
    {
        _healthPresenter.TakeDamage(value, shooterData);
    }

    public void SetHealth(int value)
    {
        _healthPresenter.SetHealth(value);
    }

    public void SetHealthColor(Color color)
    {
        _progressBar.SetColor(color);
    }

    private void OnHealthChanged()
    {
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
    }

    private void OnCameraChanged(Camera camera)
    {
        _progressBarRotator.SetTarget(camera.transform);
    }
}
