// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;

[Serializable()]
public class SerializableCommand
{
    public string UID;
    public string typeName;
    public string body;
    public bool shouldShare;

    public SerializableCommand(string typeName, string UID, string body, bool shouldShare)
    {
        this.typeName = typeName;
        this.UID = UID;
        this.body = body;
        this.shouldShare = shouldShare;
    }
}