using System;

public class Initializer
{
    private static Initializer manager;
    public static Initializer Instance()
    {
        if (manager == null) { manager = new Initializer(); }
        return manager;
    }

    private Initializer() { }

    public void Start()
    {

    }
}