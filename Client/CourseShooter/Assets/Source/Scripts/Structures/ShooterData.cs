public struct ShooterData
{
    public ShooterData(int ownerTeamIndex)
    {
        TeamIndex = ownerTeamIndex;
    }

    public int TeamIndex { get; private set; }
}
