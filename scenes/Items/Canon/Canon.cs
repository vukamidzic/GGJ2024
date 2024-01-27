using Godot;
using System;

public partial class Canon : Item
{
    Area3D damageArea;
    StandardFish standardFish;
    GoldenFish goldenFish;
    Player playerInstance;
    public AnimationPlayer animPlayer;
    public override void _Ready()
    {
        damageArea = GetNode<Area3D>("Area3D");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        damageArea.Monitoring = false;
    }
    public override void shoot(Player player)
    {
        damageArea.Monitoring = true;
        playerInstance = player;
        if(!animPlayer.IsPlaying())
            animPlayer.Play("shooting");
    }

    public void _on_area_3d_body_entered(Node3D body)
    {
        if(body.GetType() == typeof(StandardFish))
        {
            StandardFish standardFish = (StandardFish)body;
            playerInstance.animPlayer.Play("score");
            standardFish.destroy();
        }
        if(body.GetType() == typeof(GoldenFish))
        {
            goldenFish = (GoldenFish)body;
            goldenFish.destroy(playerInstance);
        }
    }
}
