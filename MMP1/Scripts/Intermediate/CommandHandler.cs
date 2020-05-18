// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class CommandHandler
{
    public delegate INetworkCommand GenerateFromInput(SerializableCommand sCommand);

    public static void Handle(SerializableCommand seriCommand)
    {
        GenerateFromInput inputFunc = null;
        switch (seriCommand.typeName)
        {
            // have to use explicit names and functions
            // because static members cannot be inherited or marked as abstract
            case MoveMBECommand.name:
                inputFunc = MoveMBECommand.FromInput;
                break;
            case MoveGMCommand.name:
                inputFunc = MoveGMCommand.FromInput;
                break;
            case MoveQBECommand.name:
                inputFunc = MoveQBECommand.FromInput;
                break;
            case CreateGhostPlayerCommand.name:
                inputFunc = CreateGhostPlayerCommand.FromSerializable;
                break;
            case ColorRequestedCommand.name:
                inputFunc = ColorRequestedCommand.FromSerializable;
                break;
            case ColorClaimedCommand.name:
                inputFunc = ColorClaimedCommand.FromSerializable;
                break;
            case GameOverCommand.name:
                inputFunc = GameOverCommand.FromSerializable;
                break;
        }

        if (inputFunc != null)
        {
            CommandQueue.Queue(inputFunc(seriCommand));
        }
    }
}