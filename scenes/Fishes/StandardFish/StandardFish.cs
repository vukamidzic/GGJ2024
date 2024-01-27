using Godot;
using System;

public partial class StandardFish : CharacterBody3D
{
    public void destroy()
    {
        QueueFree();
    }
}
