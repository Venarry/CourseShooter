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

export class MapScoreData extends Schema
{
    @type("number")
    TeamIndex = 0;

    @type("number")
    Score = 0;

    constructor (teamIndex: number, startScore: number)
    {
        super();
        this.TeamIndex = teamIndex;
        this.Score = startScore;
    }

    AddScore()
    {
        this.Score++;
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
    Health;

    @type(MyVector3)
    Position = new MyVector3();

    @type(MyVector3)
    Direction = new MyVector3();

    @type(MyVector3)
    Rotation = new MyVector3();

    @type([ "string" ]) 
    WeaponPaths = new ArraySchema<string>();

    @type("number")
    ActiveWeapon: number;

    @type("number")
    TeamIndex: number;

    SetMoveData(newMovemetData: any)
    {
        this.Position.SetValues(newMovemetData.Position);
        this.Direction.SetValues(newMovemetData.Direction);
    }

    SetMovePosition(position: MyVector3)
    {
        this.Position.SetValues(position);
    }

    SetMoveDirection(position: MyVector3)
    {
        this.Direction.SetValues(position);
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

    SetHealth(value: number)
    {
        this.Health = value;
    }

    SetTeam(index: number)
    {
        this.TeamIndex = index;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type({ map: MapScoreData })
    Score = new MapSchema<MapScoreData>();
    
    AddScore(data: any)
    {
        if(this.Score.has(data.TeamIndex))
        {
            this.Score.get(data.TeamIndex).AddScore();
        }
        else
        {
            var startScore = 1;
            this.Score.set(data.TeamIndex, new MapScoreData(data.TeamIndex, startScore));
        }

        console.log("set index: " + data.TeamIndex + " with score " + this.Score.get(data.TeamIndex).Score);

        //console.log(this.Score);
    }

    /*@type([ "number" ])
    Score = new ArraySchema<"number">();

    AddScore(data: any)
    {
        //console.log(data.TeamIndex);

        if(this.Score.length - 1 >= data.TeamIndex)
        {
            var currentScore: "number" = this.Score[data.TeamIndex];
            var newScore: "number" = currentScore + 1 as "number";
            this.Score[data.TeamIndex] = newScore;
        }
        else
        {
            var startScore: "number" = 1 as unknown as "number";
            this.Score.set(data.TeamIndex, startScore);
        }

        console.log(this.Score);
    }*/

    createPlayer(sessionId: string, data: any) 
    {
        const player = new Player(sessionId);
        player.SetMovePosition(data.Position);
        this.players.set(sessionId, player);
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

    SetHealth(sessionId: string, value: number)
    {
        this.players.get(sessionId).SetHealth(value);
    }

    SetTeam(sessionId: string, value: number)
    {
        
        this.players.get(sessionId).SetTeam(value);
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
            this.state.AddWeapon(client.sessionId, data);
        });

        this.onMessage("SwitchWeapon", (client, data) => 
        {
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

        this.onMessage("OnTeamIndexChanged", (client, data) => 
        {
            console.log("team " + data);
            this.state.SetTeam(client.sessionId, data);
        });

        this.onMessage("OnDamageTaken", (client, data) => 
        {
            this.state.SetHealth(client.sessionId, data);
        });

        this.onMessage("OnKilled", (client, data) => 
        {
            this.state.AddScore(data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data);
        console.log(data.TeamIndex);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
