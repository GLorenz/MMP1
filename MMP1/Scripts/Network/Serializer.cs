// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class Serializer
{
    public static byte[] SerializeInput(SerializableCommand input)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            new BinaryFormatter().Serialize(stream, input);
            return stream.ToArray();
        }
    }

    public static SerializableCommand Deserialize(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            try
            {
                return (SerializableCommand)new BinaryFormatter().Deserialize(stream);
            }
            catch(SerializationException se)
            {
                Console.WriteLine(se.Message);
                return null;
            }
        }
    }
}