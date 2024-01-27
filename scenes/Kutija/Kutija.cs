using Godot;
using System;

public partial class Kutija : Area3D
{
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<Node3D>("toolbox").GetNode<AnimationPlayer>("AnimationPlayer2");
    }
    public void open(Player player)
    {
        GD.Print("otvaranje");
        player.levelUp();
        animPlayer.Play("kutijaAnimations/opening");
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
