using Godot;
using System;

public partial class GunDecal : Node3D
{
    AnimationPlayer animPlayer;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("bulletDecal");
    }

    public void _on_animation_player_animation_finished(String animName)
    {
        if(animName == "bulletDecal")
            QueueFree();
    }
}
