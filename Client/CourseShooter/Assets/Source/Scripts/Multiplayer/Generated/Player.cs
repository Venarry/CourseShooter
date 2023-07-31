// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
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

	[Type(6, "ref", typeof(MyVector3))]
	public MyVector3 Rotation = new MyVector3();
}

