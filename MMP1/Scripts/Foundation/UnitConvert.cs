﻿using Microsoft.Xna.Framework;

public class UnitConvert
{
    public static int screenWidth = 1920;
    public static int screenHeight = 1080;
    public static int gameSpaceUnits = 1000;

    // not using this:
    // private static Vector2 toGameConverter = new Vector2(gameSpaceUnits / screenWidth, gameSpaceUnits / screenHeight);
    // because Point uses ints which would mess up the devision close to 1
    // an Vector2 isn't really comptible with Point

    public static Point ToScreenRelative(Point absolute)
    {
        // multiplying first becuase no floating point division
        return new Point(absolute.X * screenWidth / gameSpaceUnits, absolute.Y * screenHeight / gameSpaceUnits);
    }

    public static Point ToAbsolute(Point relative)
    {
        // multiplying first becuase no floating point division
        return new Point(relative.X * gameSpaceUnits / screenWidth, relative.Y * gameSpaceUnits / screenHeight);
    }

    public static Rectangle ToScreenRelative(Rectangle absolute)
    {
        // multiplying first becuase no floating point division
        return new Rectangle(absolute.X * screenWidth / gameSpaceUnits, absolute.Y * screenHeight / gameSpaceUnits, absolute.Width * screenWidth / gameSpaceUnits, absolute.Height * screenHeight / gameSpaceUnits);
    }

    public static Rectangle ToAbsolute(Rectangle relative)
    {
        // multiplying first becuase no floating point division
        return new Rectangle(relative.X * gameSpaceUnits / screenWidth, relative.Y * gameSpaceUnits / screenHeight, relative.Width * gameSpaceUnits / screenWidth, relative.Height * gameSpaceUnits / screenHeight);
    }
}