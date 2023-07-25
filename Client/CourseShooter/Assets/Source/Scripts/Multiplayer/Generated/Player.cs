using Colyseus.Schema;

public partial class Player : Schema 
{
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);
	
	[Type(2, "number")]
	public float z = default(float);

    [Type(3, "number")]
    public float DirectionX = default(float);

    [Type(4, "number")]
    public float DirectionY = default(float);

    [Type(5, "number")]
    public float DirectionZ = default(float);
}

