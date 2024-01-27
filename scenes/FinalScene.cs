using Godot;
using System;

public partial class FinalScene : Node3D
{
    AnimationPlayer animPlayer;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("finalScene");
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionPressed("exit"))
        {
            GetTree().ChangeSceneToFile("res://scenes/MainMenu/MainMenu.tscn");
        }
    }
}
