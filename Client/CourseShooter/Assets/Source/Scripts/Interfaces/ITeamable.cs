using System;

public interface ITeamable
{
    public event Action<int, ITeamable> TeamChanged;
    public event Action HealthOver;
    public event Action<ITeamable> Leaved;
    public int TeamIndex { get; }
    public bool IsAlive { get; }
    public void SetTeamIndex(int teamIndex);
}
