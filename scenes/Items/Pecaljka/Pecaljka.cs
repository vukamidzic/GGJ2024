using Godot;
using System;

public partial class Pecaljka : Item
{
    enum STATE{
        CAPTURED,
        FREE
    }
    STATE state;
    Area3D mamac;
    StandardFish standardFish;
    GoldenFish goldenFish;
    public AnimationPlayer animPlayer;
    public AnimationPlayer meshAnimPlayer;
    AudioStreamPlayer audioStream;
    int minus;
    public override void _Ready()
    {
        state = STATE.FREE;
        mamac = GetNode<Area3D>("Mamac");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        meshAnimPlayer = GetNode<Node3D>("pecac_anim").GetNode<AnimationPlayer>("AnimationPlayer");
        audioStream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            if(state == STATE.CAPTURED) moveMamac(mouseMotion.Relative.X, mouseMotion.Relative.Y);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
    }

    public void moveMamac(float mouseMoveX, float mouseMoveY)
    {
        mamac.GlobalPosition += new Vector3(-minus*mouseMoveY*0.05f, 0.0f, minus*mouseMoveX*0.05f);
    }
    public override void shoot(Player player)
    {
        if(state == STATE.CAPTURED)
        {
            if((mamac.GlobalPosition.X - player.GlobalPosition.X) > 0)
                minus = 1;
            else
                minus = -1;
            if(standardFish != null)
            {
                player.animPlayer.Play("score");
                player.audioStream2.Play();
                standardFish.destroy();
            }
            if(goldenFish != null)
                goldenFish.destroy(player);
            state = STATE.FREE;
            player.state = Player.STATE.WALK;
            mamac.Position = new Vector3(0.0f, 0.0f, -1.0f);
        }
        else if(state == STATE.FREE)
        {
            audioStream.Play();
            player.raycast.ForceRaycastUpdate();
            if(player.raycast.IsColliding())
            {  
                state = STATE.CAPTURED;
                player.state = Player.STATE.FISH;
                mamac.GlobalPosition = new Vector3(player.raycast.GetCollisionPoint().X, 1.0f, player.raycast.GetCollisionPoint().Z);
            }
            meshAnimPlayer.Play("pecanje");
        }
    }

    public void _on_area_3d_body_entered(CharacterBody3D body)
    {
        GD.Print("uslo");
        if(body.GetType() == typeof(StandardFish))
        {
            standardFish = (StandardFish)body;
            standardFish.lightOn();
        }
        else if(body.GetType() == typeof(GoldenFish))
        {
            goldenFish = (GoldenFish)body;
            goldenFish.lightOn();
        }
    }

    public void _on_area_3d_body_exited(CharacterBody3D body)
    {
        if(body.GetType() == typeof(StandardFish) && (standardFish != null))
        {
            standardFish.lightOff();
            standardFish = null;
        }
        else if(body.GetType() == typeof(GoldenFish) && (goldenFish != null))
        {
            goldenFish.lightOff();
            goldenFish = null;
        }
    }
}
