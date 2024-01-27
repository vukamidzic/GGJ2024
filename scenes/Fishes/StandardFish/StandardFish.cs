using Godot;
using System;

public partial class StandardFish : CharacterBody3D
{
    OmniLight3D light;
    public override void _Ready()
    {
        light = GetNode<OmniLight3D>("OmniLight3D");
        light.LightEnergy = 0.0f;
    }
    public void lightOn()
    {
        light.LightEnergy = 16.0f;
    }
    public void lightOff()
    {
        light.LightEnergy = 0.0f;
    }
    public void destroy()
    {
        QueueFree();
    }
}
