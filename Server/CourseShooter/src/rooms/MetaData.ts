import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class MetaData extends Schema 
{
    constructor() 
    {
        super();

        //this.Name = `User№${}`;
    }

    @type("string")
    Password;
}