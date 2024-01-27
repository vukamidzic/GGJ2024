using Godot;
using System;

public partial class Pecaljka : Item
{
    enum STATE{
        CAPTURED,
        FREE
    }
    STATE state;

    MeshInstance3D mamac;
    public override void _Ready()
    {
        state = STATE.FREE;
        mamac = GetNode<MeshInstance3D>("Mamac");
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
        mamac.Position += new Vector3(mouseMoveX*0.05f, 0.0f, mouseMoveY*0.05f);
    }
    public override void shoot(Player player)
    {
        if(state == STATE.CAPTURED)
        {
            state = STATE.FREE;
            player.state = Player.STATE.WALK;
            mamac.Position = new Vector3(0.0f, 0.0f, -1.0f);
        }
        else if(state == STATE.FREE)
        {
            state = STATE.CAPTURED;
            player.state = Player.STATE.FISH;
            player.raycast.ForceRaycastUpdate();
            if(player.raycast.IsColliding())
            {
                mamac.GlobalPosition = player.raycast.GetCollisionPoint();
            }
        }
    }
}
