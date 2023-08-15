using System;

public class HealthModel
{
    public event Action HealthChanged;
    public event Action HealthOver;
    public event Action<ShooterData> Killed;

    public HealthModel(int maxValue)
    {
        MaxValue = maxValue;
        Value = maxValue;
    }

    public HealthModel(int maxValue, int value)
    {
        MaxValue = maxValue;
        Value = value;
    }

    public int Value { get; private set; }
    public int MaxValue { get; private set; }

    public void Restore()
    {
        Value = MaxValue;
        HealthChanged?.Invoke();
    }

    public void SetHealth(int value)
    {
        if (value < 0)
        {
            value = 0;
            HealthOver?.Invoke();
        }

        Value = value;
        HealthChanged?.Invoke();
    }

    public void SetMaxHealth(int value)
    {
        if (value < 1)
            value = 1;

        MaxValue = value;
    }

    public void TakeDamage(int value, ShooterData ownerData)
    {
        if(value < 0)
            value = 0;

        Value -= value;

        if (Value < 0)
            Killed?.Invoke(ownerData);

        HealthChanged?.Invoke();
    }

    public void Add(int value)
    {
        if(value < 0)
            value = 0;

        Value += value;

        if(Value > MaxValue)
            Value = MaxValue;

        HealthChanged?.Invoke();
    }
}
