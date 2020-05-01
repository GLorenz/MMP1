using System;
using Microsoft.Xna.Framework;

public class CommandHandler
{
    public delegate INetworkCommand GenerateFromInput(SerializableCommand sCommand);
    public static void Handle(SerializableCommand seriCommand)
    {
        GenerateFromInput inputFunc = null;
        switch (seriCommand.typeName)
        {
            case MoveCommand.name:
                inputFunc = MoveCommand.FromInput;
                break;
            case CreateGhostPlayerCommand.name:
                inputFunc = CreateGhostPlayerCommand.FromSerializable;
                break;
            case CreateGhostMeepleCommand.name:
                inputFunc = CreateGhostMeepleCommand.FromSerializable;
                break;
        }

        if (inputFunc != null) {
            CommandQueue.Instance().Queue(inputFunc(seriCommand));
        }
    }
}