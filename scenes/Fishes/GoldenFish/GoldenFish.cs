using Godot;
using System;

public partial class GoldenFish : CharacterBody3D
{
    [Export]
    float speed;
    OmniLight3D light;
    Vector3 moveVector;
    public FishSpawner spawn;
    public override void _Ready()
    {
        light = GetNode<OmniLight3D>("OmniLight3D");
        light.LightEnergy = 5.0f;
        moveVector = new Vector3(0.0f, 0.0f, 1.0f);
    }
    public override void _PhysicsProcess(double delta)
    {
        if(this.IsOnWall())
            moveVector *= -1;

        this.Velocity = moveVector*speed;
        MoveAndSlide();
    }
    public void lightOn()
    {
        light.LightEnergy = 16.0f;
    }
    public void lightOff()
    {
        light.LightEnergy = 5.0f;
    }
    public void destroy(Player player)
    {
        player.fail();
        QueueFree();
    }

}
