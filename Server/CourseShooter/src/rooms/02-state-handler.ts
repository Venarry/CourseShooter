import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class MyVector3 extends Schema
{
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;
}

export class Player extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 7 - 3.5);

    @type("number")
    y = 0;

    @type("number")
    z = Math.floor(Math.random() * 7 - 3.5);

    @type("number")
    DirectionX = 0;

    @type("number")
    DirectionY = 0;

    @type("number")
    DirectionZ = 0;

    @type(MyVector3)
    Rotation = new MyVector3();

    SetPosition(moveData: any)
    {
        this.x = moveData.Position.x;
        this.y = moveData.Position.y;
        this.z = moveData.Position.z;

        this.DirectionX = moveData.Direction.x;
        this.DirectionY = moveData.Direction.y;
        this.DirectionZ = moveData.Direction.z;
    }

    SetRotation(targetRotation: MyVector3)
    {
        this.Rotation.x = targetRotation.x;
        this.Rotation.y = targetRotation.y;
        this.Rotation.z = targetRotation.z;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) 
    {
        this.players.get(sessionId).SetPosition(movement);
    }

    RotatePlayer(sessionId: string, targetRotation: any)
    {
        this.players.get(sessionId).SetRotation(targetRotation);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => 
        {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("Rotate", (client, data) => 
        {
            this.state.RotatePlayer(client.sessionId, data);
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
