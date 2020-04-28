using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
    public static BinaryFormatter formatter = new BinaryFormatter();

    public static byte[] SerializeInput(SerializableCommand input)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, input);
            return stream.ToArray();
        }
        
    }

    public static SerializableCommand Deserialize(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            try
            {
                return (SerializableCommand)formatter.Deserialize(stream);
            }
            catch(SerializationException se)
            {
                Console.WriteLine(se.Message);
                return null;
            }
        }
    }
}