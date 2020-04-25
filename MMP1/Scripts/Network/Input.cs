using System;

[Serializable]
public class Input
{
    [NonSerialized]
    private static int curId;
    
    public int id { get; private set; }
    public string typeName { get; set; }
    public string targetIdentifier { get; set; }
    public string body { get; set; }
    public bool shouldShare { get; set; }

    public Input(string typeName, string body)
    {
        this.id = getNextId();
        this.typeName = typeName;
        this.body = body;
    }

    private static int getNextId()
    {
        return curId++;
    }
}