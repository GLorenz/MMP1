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
            byte[] result = stream.ToArray();
            Console.WriteLine(result.Length);
            return result;
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
    /*private static char splitter = '#';

    public static SerializableCommand Deserialize(byte[] data)
    {
        string s = Encoding.ASCII.GetString(data);
        Console.WriteLine(s);
        string[] all = s.Split(splitter);
        return new SerializableCommand(all[0], all[1], all[2], false);
    }

    public static byte[] SerializeInput(SerializableCommand input)
    {
        string all = input.typeName + splitter + input.UID + splitter + input.body;

        return Encoding.ASCII.GetBytes(all);
    }*/

    /*public static byte[] SerializeInput(SerializableCommand input)
    {
        byte[] result = new byte[nameLength + UIDLength + bodyLength];

        byte[] name = Encoding.Unicode.GetBytes(input.typeName);
        byte[] uid = Encoding.Unicode.GetBytes(input.UID);
        byte[] body = Encoding.Unicode.GetBytes(input.body);

        

        Array.Copy(name, 0, result, 0, name.Length);
        Array.Copy(uid, 0, result, nameLength, uid.Length);
        Array.Copy(body, 0, result, nameLength + UIDLength, body.Length);

        return result;
    }*/
}