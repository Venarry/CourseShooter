// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
	[Type(0, "string")]
	public string Name = default(string);

	[Type(1, "number")]
	public float Health = default(float);

	[Type(2, "ref", typeof(MyVector3))]
	public MyVector3 Position = new MyVector3();

	[Type(3, "ref", typeof(MyVector3))]
	public MyVector3 Direction = new MyVector3();

	[Type(4, "ref", typeof(MyVector3))]
	public MyVector3 Rotation = new MyVector3();

	[Type(5, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> WeaponPaths = new ArraySchema<string>();

	[Type(6, "number")]
	public float ActiveWeapon = default(float);

	[Type(7, "number")]
	public float TeamIndex = default(float);

	[Type(8, "boolean")]
	public bool IsSpawned = default(bool);
}

