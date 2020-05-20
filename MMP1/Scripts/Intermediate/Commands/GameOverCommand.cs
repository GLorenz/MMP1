// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class GameOverCommand : INetworkCommand
{
    public const string name = "GaOv";

    private GhostPlayer winner;
    public GameOverCommand(GhostPlayer winner)
    {
        this.winner = winner;
    }

    public void Execute()
    {
        Game1.OnGameOver4Real(winner);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, winner.UID, "", shouldShare);
    }

    public static GameOverCommand FromSerializable(SerializableCommand sCommand)
    {
        GhostPlayer winner = PlayerManager.Instance().GetByUID(sCommand.UID);
        return new GameOverCommand(winner);
    }
}