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
    int minus;
    public override void _Ready()
    {
        state = STATE.FREE;
        mamac = GetNode<Area3D>("Mamac");
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
                standardFish.destroy();
            state = STATE.FREE;
            player.state = Player.STATE.WALK;
            mamac.Position = new Vector3(0.0f, 0.0f, -1.0f);
        }
        else if(state == STATE.FREE)
        {
            if(player.raycast.IsColliding())
            {  
                state = STATE.CAPTURED;
                player.state = Player.STATE.FISH;
                player.raycast.ForceRaycastUpdate();
                mamac.GlobalPosition = new Vector3(player.raycast.GetCollisionPoint().X, 1.0f, player.raycast.GetCollisionPoint().Z);
            }
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
    }

    public void _on_area_3d_body_exited(CharacterBody3D body)
    {
        if(body.GetType() == typeof(StandardFish))
        {
            standardFish.lightOff();
            standardFish = null;
        }
    }
}
