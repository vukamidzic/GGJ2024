using Godot;
using System;

public partial class Kutija : Area3D
{
    AnimationPlayer animPlayer;
    AudioStreamPlayer audioPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<Node3D>("KutijaModel").GetNode<AnimationPlayer>("AnimationPlayer");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }
    public void open(Player player)
    {
        GD.Print("otvaranje");
        player.levelUp();
        audioPlayer.Play();
        animPlayer.Play("noveanimacije/opening");
    }

    public void _on_body_entered(Node3D body)
    {
        if(body.GetType() == typeof(Player))
        {
            GD.Print(body);
            if(((Player)body).canShoot == false)
            {
                ((Player)body).canOpen = true;
                ((Player)body).usableObject = this;
            }
        }
    }

    public void _on_body_exited(Node3D body)
    {
        if(body.GetType() == typeof(Player))
        {
            ((Player)body).canOpen = false;
            ((Player)body).usableObject = null;
        }
    }
}
