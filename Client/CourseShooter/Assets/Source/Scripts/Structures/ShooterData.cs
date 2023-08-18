using System;

[Serializable]
public struct ShooterData
{
    public ShooterData(int ownerTeamIndex, string sessionId)
    {
        TeamIndex = ownerTeamIndex;
        ClientId = sessionId;
    }

    public string ClientId;
    public int TeamIndex;

    public void SetTeamIndex(int index)
    {
        TeamIndex = index;
    }

    public void SetClientId(string id)
    {
        ClientId = id;
    }
}
