using Godot;
using System;

public partial class GoldenFish : CharacterBody3D
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
    public void destroy(Player player)
    {
        player.fail();
    }

}
