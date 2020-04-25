using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
    public static BinaryFormatter formatter = new BinaryFormatter();

    public static byte[] SerializeInput(Input input)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, input);
            return stream.ToArray();
        }
        
    }

    public static Input Deserialize(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            return (Input)formatter.Deserialize(stream);
        }
    }
}