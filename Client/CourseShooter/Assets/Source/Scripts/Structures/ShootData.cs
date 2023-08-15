public struct ShootData
{
    public ShootData(int ownerTeamIndex)
    {
        TeamIndex = ownerTeamIndex;
    }

    public int TeamIndex { get; private set; }
}
