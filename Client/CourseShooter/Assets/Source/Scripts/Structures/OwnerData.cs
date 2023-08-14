public struct OwnerData
{
    public OwnerData(int ownerTeamIndex)
    {
        TeamIndex = ownerTeamIndex;
    }

    public int TeamIndex { get; private set; }
}
