// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
	[Type(0, "ref", typeof(MyVector3))]
	public MyVector3 Position = new MyVector3();

	[Type(1, "ref", typeof(MyVector3))]
	public MyVector3 Direction = new MyVector3();

	[Type(2, "ref", typeof(MyVector3))]
	public MyVector3 Rotation = new MyVector3();
}

