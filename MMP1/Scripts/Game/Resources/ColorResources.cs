// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;

public class ColorResources
{
    public static readonly Color Black = new Color(5, 5, 23, 255);
    public static readonly Color White = new Color(253, 253, 255, 255);
    public static readonly Color Red = new Color(207, 92, 54, 255);
    public static readonly Color Green = new Color(133, 163, 146, 255);

    public static Color ForName(string name)
    {
        switch (name) {
            case "Red":
            case "red":
                return Red;
            case "White":
            case "white":
                return White;
            case "Green":
            case "green":
                return Green;
            default:
                return Black;
        }
    }
}