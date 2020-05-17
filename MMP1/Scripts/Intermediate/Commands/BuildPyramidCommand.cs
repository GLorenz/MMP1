// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;

public class BuildPyramidCommand : ICommand
{
    private Rectangle space;
    public BuildPyramidCommand(Rectangle space)
    {
        this.space = space;
    }

    public void Execute()
    {
        Board.Instance().space = space;
        Board.Instance().BuildPyramidInSpace();
    }
}