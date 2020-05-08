using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

public class TextureResources
{
    public static ContentManager contentManager;

    // using seperate Lists for easy bi-directional compatibility
    public static List<string> names = new List<string>();
    public static List<Texture2D> textures = new List<Texture2D>();

    public static string imgDir = "img/";
    public static string[] defaultNames = new string[] {
        "arrow",
        "red",
        "green",
        "Board",
        "Background",
        "Play",
        "PlayerBlack",
        "PlayerGreen",
        "PlayerRed",
        "PlayerWhite",
        "QM",
        "QMLight",
        "Title",
        "PyramidBackgroundFloor1",
        "PyramidBackgroundFloor2",
        "PyramidBackgroundFloor3",
        "PyramidBackgroundFloor4",
        "PyramidField",
        "PyramidFieldConnectionShort",
        "PyramidFieldConnectionLong"
    };

    public static void LoadDefault()
    {
        AddAll(defaultNames);
    }

    public static void Add(string textureName) {
        if (contentManager != null) {
            names.Add(textureName);
            textures.Add(contentManager.Load<Texture2D>(imgDir + textureName));
        }
    }

    public static void AddAll(IEnumerable<string> textureNames)
    {
        foreach(string tn in textureNames) { Add(tn); }
    }

    public static Texture2D Get(string textureName)
    {
        return textures[Math.Max(0,names.IndexOf(textureName))];
    }

    public static string Get(Texture2D texture)
    {
        return names[textures.IndexOf(texture)];
    }
}