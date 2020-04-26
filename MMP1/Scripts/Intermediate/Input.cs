using System;
using System.Threading;

[Serializable]
public class Input
{
    [NonSerialized]
    private static int curId = -1;
    
    public int id { get; private set; }
    public string typeName { get; set; }
    public string targetUID { get; set; }
    public string body { get; set; }
    public bool shouldShare { get; set; }

    public Input(string typeName, string targetUID, string body, bool shouldShare)
    {
        this.id = getNextId();
        this.typeName = typeName;
        this.targetUID = targetUID;
        this.body = body;
        this.shouldShare = shouldShare;
    }

    private static int getNextId()
    {
        return Interlocked.Increment(ref curId);
    }
}