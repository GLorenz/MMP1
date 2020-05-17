﻿// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class GameOverCommand : INetworkCommand
{
    public const string name = "GaOv";

    private GhostMeeple winner;
    public GameOverCommand(GhostMeeple winner)
    {
        this.winner = winner;
    }

    public void Execute()
    {
        Game1.OnGameOver(winner);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, winner.UID, "", shouldShare);
    }

    public static GameOverCommand FromSerializable(SerializableCommand sCommand)
    {
        GhostMeeple winner = (GhostMeeple)Board.Instance().FindByUID(sCommand.UID);
        return new GameOverCommand(winner);
    }
}