using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

public class TextureResources
{
    public static ContentManager contentManager;

    // using seperate Lists for bi-directional compatibility
    public static List<string> names = new List<string>();
    public static List<Texture2D> textures = new List<Texture2D>();

    public static void Add(string textureName) {
        if (contentManager != null) {
            names.Add(textureName);
            textures.Add(contentManager.Load<Texture2D>(textureName));
        }
    }

    public static void AddAll(string[] textureNames)
    {
        foreach(string tn in textureNames) { Add(tn); }
    }

    public static Texture2D Get(string textureName)
    {
        return textures[names.IndexOf(textureName)];
    }

    public static string Get(Texture2D texture)
    {
        return names[textures.IndexOf(texture)];
    }
}