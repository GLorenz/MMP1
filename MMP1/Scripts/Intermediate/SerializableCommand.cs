using System;

[Serializable]
public class SerializableCommand
{
    public int UID { get; set; }
    public string typeName { get; set; }
    public string body { get; set; }
    public bool shouldShare { get; set; }

    public SerializableCommand(string typeName, int UID, string body, bool shouldShare)
    {
        this.typeName = typeName;
        this.UID = UID;
        this.body = body;
        this.shouldShare = shouldShare;
    }
}