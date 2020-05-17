// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;

public class UnitConvert
{
    public static int screenWidth = 1920;
    public static int screenHeight = 1080;
    public static int gameSpaceUnits = 1000;

    // not using this:
    // private static Vector2 toGameConverter = new Vector2(gameSpaceUnits / screenWidth, gameSpaceUnits / screenHeight);
    // because Point uses ints which would mess up the devision close to 1
    // an Vector2 isn't really comptible with Point

    public static int ToScreenRelative(int value)
    {
        // multiplying first becuase no floating point division
        return value * gameSpaceUnits / screenWidth;
    }

    public static int ToAbsoluteWidth(int value)
    {
        // multiplying first becuase no floating point division
        return value * screenWidth / gameSpaceUnits;
    }

    public static int ToAbsoluteHeight(int value)
    {
        // multiplying first becuase no floating point division
        return value * screenHeight / gameSpaceUnits;
    }

    public static Point ToScreenRelative(Point absolute)
    {
        // multiplying first becuase no floating point division
        return new Point(absolute.X * gameSpaceUnits / screenWidth , absolute.Y * gameSpaceUnits / screenHeight);
    }

    public static Point ToAbsolute(Point relative)
    {
        // multiplying first becuase no floating point division
        return new Point(relative.X * screenWidth / gameSpaceUnits, relative.Y * screenHeight / gameSpaceUnits);
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

    public static Point ToRectangleUnits(Rectangle orientation, Point point, bool pointIsRelative)
    {
        return new Point((point.X - (pointIsRelative ? 0 : orientation.X)) / orientation.Width, (point.Y - (pointIsRelative ? 0 : orientation.Y)) / orientation.Height);
    }
}
