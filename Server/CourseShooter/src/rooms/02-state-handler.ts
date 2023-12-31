import { Room, Client, ClientState, updateLobby, LobbyRoom } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";
import { MetaData } from "./MetaData";

export class MyVector3 extends Schema
{
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    constructor (x: number, y: number, z: number)
    {
        super();

        this.x = x;
        this.y = y;
        this.z = z;
    }

    SetValues(newValues: MyVector3)
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

    SetData(teamIndex: number, startScore: number)
    {
        this.TeamIndex = teamIndex;
        this.Score = startScore;
    }

    AddScore()
    {
        this.Score++;
    }

    SetScore(value: number)
    {
        this.Score = value;
    }
}

export class Player extends Schema 
{
    constructor(sessionId: string) 
    {
        super();

        this.Name = `User№${sessionId}`;
    }

    @type("string")
    Name;

    @type("number")
    Health;

    @type(MyVector3)
    Position = new MyVector3(0, 0, 0);

    @type(MyVector3)
    Direction = new MyVector3(0, 0, 0);

    @type(MyVector3)
    Rotation = new MyVector3(0, 0, 0);

    @type([ "string" ]) 
    WeaponPaths = new ArraySchema<string>();

    @type("number")
    ActiveWeapon: number;

    @type("number")
    TeamIndex: number;

    @type("boolean")
    IsSpawned = false;

    SetSpawnState(state: boolean)
    {
        this.IsSpawned = state;
    }

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

    @type( "string" )
    MapName;

    AddScore(teamIndex: number)
    {
        if(this.Score.has(teamIndex.toString()))
        {
            this.Score.get(teamIndex.toString()).AddScore();
        }
        else
        {
            var startScore = 1;
            var scoreData = new MapScoreData(teamIndex, startScore);
            this.Score.set(teamIndex.toString(), scoreData);
        }
    }

    SetScore(teamIndex: number, value: number)
    {
        if(this.Score.has(teamIndex.toString()))
        {
            this.Score.get(teamIndex.toString()).SetScore(value);
        }
        else
        {
            var scoreData = new MapScoreData(teamIndex, value);
            this.Score.set(teamIndex.toString(), scoreData);
        }
    }

    createPlayer(sessionId: string, position: MyVector3) 
    {
        const player = new Player(sessionId);
        player.SetMovePosition(position);
        this.players.set(sessionId, player);
    }

    SetSpawnState(sessionId: string, state: boolean)
    {
        this.players.get(sessionId).SetSpawnState(state);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movementData: any) 
    {
        this.players.get(sessionId).SetMoveData(movementData);
    }

    SetPlayerPosition (sessionId: string, position: MyVector3) 
    {
        this.players.get(sessionId).SetMovePosition(position);
    }

    SetPlayerDirection (sessionId: string, direction: MyVector3) 
    {
        this.players.get(sessionId).SetMoveDirection(direction);
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

    SetPlayerHealth(sessionId: string, value: number)
    {
        this.players.get(sessionId).SetHealth(value);
    }

    SetTeam(sessionId: string, value: number)
    {
        this.players.get(sessionId).SetTeam(value);
    }
}

export class StateHandlerRoom extends Room<State>
{
    maxClients = 4;
    isRoundOver: boolean = false;

    onCreate (options) 
    {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.setMetadata(options).then(() => updateLobby(this));

        console.log(this.metadata);

        this.onMessage("OnPlayerSpawn", (client, data) => 
        {
            var spawnPosition = new MyVector3(data.Position.x, data.Position.y, data.Position.z);
            this.state.createPlayer(client.sessionId, spawnPosition);
            this.state.SetTeam(client.sessionId, data.TeamIndex);
            console.log("spawn " + client.sessionId);
        });

        this.onMessage("SetPosition", (client, position) => 
        {
            this.state.SetPlayerPosition(client.sessionId, position);
        });

        this.onMessage("SetDirection", (client, direction) => 
        {
            this.state.SetPlayerDirection(client.sessionId, direction);
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
            this.state.SetTeam(client.sessionId, data);
        });

        this.onMessage("OnHealthChanged", (client, data) => 
        {
            this.state.SetPlayerHealth(client.sessionId, data);
        });

        this.onMessage("OnEnemyHealthChanged", (client, data) => 
        {
            this.state.SetPlayerHealth(data.Id, data.Value);
        });

        this.onMessage("SetScore", (client, data) => 
        {
            this.state.SetScore(data.TeamIndex, data.Value);
        });

        this.onMessage("OnRespawn", (client, id) => 
        {
            this.broadcast("Respawn", id, {except: client});
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) 
    {
        /*if(this.metadata.Version != data.Version)
        {
            this.lock();
        }*/
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
