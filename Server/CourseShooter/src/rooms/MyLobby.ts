import { Client, LobbyRoom } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class LobbyState extends Schema
{
    @type("number")
    PlayersCount = 0;

    AddPlayer()
    {
        this.PlayersCount++;
    }

    RemovePlayer()
    {
        this.PlayersCount--;
    }
}

export class MyLobby extends LobbyRoom
{
    maxClients = 15000;

    async onCreate(options)
    {
        await super.onCreate(options);
        this.setState(new LobbyState());
    }

    onJoin(client: Client, options?: any)
    {
        super.onJoin(client, options);
        this.state.AddPlayer();
    }

    onLeave(client: Client, consented?: boolean){
        super.onLeave(client);
        this.state.RemovePlayer();
    }

    onDispose(){
        super.onDispose();
    }
}