using Godot;
using System;

public partial class Gun : Item
{
    [Export]
    float swayThreshold;
    public AnimationPlayer animPlayer;
    public AnimationPlayer animPlayer2;
    float mouseMove;
    StandardFish standardFish;
    GoldenFish goldenFish;
    Node3D tracer;

    public override void _Ready()
    {
        tracer = GetNode<Node3D>("Tracer");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            mouseMove = -mouseMotion.Relative.X;
        }   
    }

    public override void _PhysicsProcess(double delta)
    {
            gunSway(delta);
    }

    public void gunSway(double delta)
    {
        if(mouseMove != null)
		{
			if(mouseMove>swayThreshold)
				Rotation = Rotation.Lerp(new Godot.Vector3(0.0f, 0.1f, 0.0f), (float)(delta*5));
			else if(mouseMove<-swayThreshold)
				Rotation = Rotation.Lerp(new Godot.Vector3(0.0f, -0.1f, 0.0f), (float)(delta*5));
			else
				Rotation = Rotation.Lerp(new Godot.Vector3(0.0f, 0.0f, 0.0f), (float)(delta*5));
		}
    }

    public override void shoot(Player player)
    {
        player.raycast.ForceRaycastUpdate();
        // GD.Print(player.raycast.GetCollisionPoint());
        // tracer.LookAt(player.raycast.GetCollisionPoint(), Vector3.Forward);
        if(player.raycast.IsColliding())
        { 
            if(player.raycast.GetCollider().GetType() == typeof(StandardFish))
            {
                player.animPlayer.Play("score");
                standardFish = (StandardFish)player.raycast.GetCollider();
                standardFish.destroy();
            }
            if(player.raycast.GetCollider().GetType() == typeof(GoldenFish))
            {
                goldenFish = (GoldenFish)player.raycast.GetCollider();
                goldenFish.destroy(player);
            }
        }
        animPlayer.Stop();
        animPlayer.Play("shoot");
    }

}
