using Godot;
using System;

public partial class Canon : Item
{
    Area3D damageArea;
    StandardFish standardFish;
    GoldenFish goldenFish;
    Player playerInstance;
    public AnimationPlayer animPlayer;
    AudioStreamPlayer audioPlayer;
    ColorRect colorRect;
    public override void _Ready()
    {
        damageArea = GetNode<Area3D>("Area3D");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        colorRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("ColorRect");
        colorRect.Visible = false;
        damageArea.Monitoring = false;
    }
    public override void shoot(Player player)
    {
        damageArea.Monitoring = true;
        playerInstance = player;
        audioPlayer.Play();
        if(!animPlayer.IsPlaying())
            animPlayer.Play("shooting");
    }

    public void _on_area_3d_body_entered(Node3D body)
    {
        if(body.GetType() == typeof(StandardFish))
        {
            StandardFish standardFish = (StandardFish)body;
            playerInstance.animPlayer.Play("score");
            playerInstance.audioStream2.Play();
            standardFish.destroy();
        }
        if(body.GetType() == typeof(GoldenFish))
        {
            goldenFish = (GoldenFish)body;
            goldenFish.destroy(playerInstance);
        }
    }

    public void _on_animation_player_animation_finished(String animName)
    {
        if(animName == "shooting")
            damageArea.Monitoring = false;
        if(animName == "golden")
            GetTree().ChangeSceneToFile("res://scenes/FinalScene.tscn");
    }
}
