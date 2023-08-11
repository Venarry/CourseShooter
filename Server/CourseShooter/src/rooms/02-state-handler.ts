import { Room, Client, ClientState } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class MyVector3 extends Schema
{
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    constructor (x = 0, y = 0, z = 0)
    {
        super();

        this.x = x;
        this.y = y;
        this.z = z;
    }

    SetValues(newValues: any)
    {
        this.x = newValues.x;
        this.y = newValues.y;
        this.z = newValues.z;
    }
}

export class Player extends Schema 
{
    constructor(sessionId = "") 
    {
        super();

        this.Name = `User№${sessionId}`;
    }

    @type("string")
    Name;

    @type("number")
    SpawnPointIndex = Math.floor(Math.random() * 2);

    @type(MyVector3)
    Position = new MyVector3(); //Math.floor(Math.random() * 7 - 3.5)

    @type(MyVector3)
    Direction = new MyVector3();

    @type(MyVector3)
    Rotation = new MyVector3();

    @type([ "string" ]) 
    WeaponPaths = new ArraySchema<string>();

    @type("number")
    ActiveWeapon: number;

    SetMoveData(newMovemetData: any)
    {
        this.Position.SetValues(newMovemetData.Position);
        this.Direction.SetValues(newMovemetData.Direction);
    }

    SetRotation(targetRotation: any)
    {
        this.Rotation.SetValues(targetRotation);
    }

    AddWeapon(weaponPath: string)
    {
        this.WeaponPaths.push(weaponPath);
    }

    SwitchWeapon(index: number)
    {
        this.ActiveWeapon = index;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player(sessionId));
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) 
    {
        this.players.get(sessionId).SetMoveData(movement);
    }

    RotatePlayer(sessionId: string, targetRotation: any)
    {
        this.players.get(sessionId).SetRotation(targetRotation);
    }

    AddWeapon(sessionId: string, weaponPath: string)
    {
        this.players.get(sessionId).AddWeapon(weaponPath);
    }

    SwitchWeapon(sessionId: string, index: number)
    {
        this.players.get(sessionId).SwitchWeapon(index);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("Move", (client, data) => 
        {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("Rotate", (client, data) => 
        {
            this.state.RotatePlayer(client.sessionId, data);
        });

        this.onMessage("AddWeapon", (client, data) => 
        {
            //this.broadcast("WeaponAdded", data, {except: client});
            this.state.AddWeapon(client.sessionId, data);
        });

        this.onMessage("SwitchWeapon", (client, data) => 
        {
            //this.broadcast("WeaponSwitched", data, {except: client});
            this.state.SwitchWeapon(client.sessionId, data);
        });

        this.onMessage("OnShoot", (client, data) => 
        {
            this.broadcast("Shoot", data, {except: client});
        });

        this.onMessage("MessageSent", (client, data) => 
        {
            this.broadcast("MessageSent", `[${this.state.players.get(client.sessionId).Name}] ${data}`);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
