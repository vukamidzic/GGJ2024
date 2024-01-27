using Godot;
using System;

public partial class StandardFish : CharacterBody3D
{
    [Export]
    float speed;
    OmniLight3D light;
    Vector3 moveVector;
    public override void _Ready()
    {
        light = GetNode<OmniLight3D>("OmniLight3D");
        light.LightEnergy = 0.0f;
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
        light.LightEnergy = 0.0f;
    }
    public void destroy()
    {
        QueueFree();
    }
}
