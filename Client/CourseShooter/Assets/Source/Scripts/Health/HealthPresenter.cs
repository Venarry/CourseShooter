using System;

public class HealthPresenter
{
    private readonly HealthModel _healthModel;

    public event Action HealthChanged;
    public event Action<OwnerData> HealthOver;

    public HealthPresenter(HealthModel healthModel)
    {
        _healthModel = healthModel;
    }

    public float HealthNormalized => (float)_healthModel.Value / _healthModel.MaxValue;
    public int Health => _healthModel.Value;
    public int MaxHealth => _healthModel.MaxValue;

    public void Enable()
    {
        _healthModel.HealthChanged += OnHealthChanged;
        _healthModel.Killed += OnKilled;
    }

    public void Disable()
    {
        _healthModel.HealthChanged -= OnHealthChanged;
        _healthModel.Killed -= OnKilled;
    }

    public void Add(int value)
    {
        _healthModel.Add(value);
    }

    public void Restore()
    {
        _healthModel.Restore();
    }

    public void SetHealth(int value)
    {
        _healthModel.SetHealth(value);
    }

    public void SetMaxHealth(int value)
    {
        _healthModel.SetMaxHealth(value);
    }

    public void TakeDamage(int value, OwnerData ownerData)
    {
        _healthModel.TakeDamage(value, ownerData);
    }

    private void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }

    private void OnKilled(OwnerData ownerData)
    {
        HealthOver?.Invoke(ownerData);
    }
}
