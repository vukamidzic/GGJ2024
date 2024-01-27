using Godot;
using System;

public partial class Mines : Item
{
    [Export]
    PackedScene minesPlacingScene;
    MinesPlacing minesPlacing;
    public AnimationPlayer animPlayer;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public override void shoot(Player player)
    {
        minesPlacing = (MinesPlacing)minesPlacingScene.Instantiate();
        player.raycast.ForceRaycastUpdate();
        GetParent().GetParent().GetParent().AddChild(minesPlacing);
        minesPlacing.playerInstance = player;
        minesPlacing.GlobalPosition = player.raycast.GetCollisionPoint();
    }
}
