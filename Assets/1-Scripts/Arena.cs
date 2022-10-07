using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Boundary
{
    public float TopY;
    public float TopX;
    public float BotY;
    public float BotX;

    public Boundary(float TopY,float TopX, float BotY, float BotX)
    {
        this.TopY = TopY;
        this.TopX = TopX;
        this.BotY = BotY;
        this.BotX = BotX;
    }
}

public class Arena 
{
    public Boundary ArenaBoundary;

    public Arena(Boundary boundary)
    {
        ArenaBoundary = boundary;
    }
}
