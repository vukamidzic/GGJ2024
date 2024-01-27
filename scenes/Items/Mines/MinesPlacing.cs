using Godot;
using System;

public partial class MinesPlacing : Area3D
{
    public Player playerInstance;
    AnimationPlayer animPlayer;
    AudioStreamPlayer audioPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }
    public void _on_body_entered(Node3D body)
    {
        if(body.GetType() == typeof(StandardFish))
        {
            StandardFish standardFish = (StandardFish)body;
            playerInstance.animPlayer.Play("score");
            playerInstance.audioStream2.Play();
            if(!animPlayer.IsPlaying())
                animPlayer.Play("explode");
            audioPlayer.Play();
            standardFish.destroy();
            QueueFree();
        }
        if(body.GetType() == typeof(GoldenFish))
        {
            GoldenFish goldenFish = (GoldenFish)body;
            goldenFish.destroy(playerInstance);
        }
    }
}
