﻿// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class AddGhostMeepleToPlayerManager : ICommand
{
    private GhostMeeple meeple;

    public AddGhostMeepleToPlayerManager(GhostMeeple meeple)
    {
        this.meeple = meeple;
    }

    public void Execute()
    {
        PlayerManager.Instance().AddMeepleRef(meeple);
    }
}