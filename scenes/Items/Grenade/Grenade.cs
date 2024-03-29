using Godot;
using System;

public partial class Grenade : Item
{
    Area3D damageArea;
    StandardFish standardFish;
    GoldenFish goldenFish;
    public AnimationPlayer animPlayer;
    Vector3 oldPos;
    MeshInstance3D grenadeMesh;
    MeshInstance3D viewmodelMesh;
    Player playerInstance;
    bool canShoot;
    public override void _Ready()
    {
        damageArea = GetNode<Area3D>("DamageArea");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        grenadeMesh = GetNode<MeshInstance3D>("MeshInstance3D");
        viewmodelMesh = GetNode<MeshInstance3D>("MeshInstance3D2");
        damageArea.Monitoring = false;
        damageArea.Visible = false;
        grenadeMesh.Visible = false;
        viewmodelMesh.Visible = true;
        canShoot = true;
        GD.Print(oldPos);
    }
    public override void shoot(Player player)
    {
        if(canShoot)
        {
            canShoot = false;
            player.raycast.ForceRaycastUpdate();
            if(player.raycast.IsColliding())
            { 
                damageArea.GlobalPosition = player.raycast.GetCollisionPoint();
                grenadeMesh.GlobalPosition = player.raycast.GetCollisionPoint();
            }
            damageArea.Monitoring = true;
            damageArea.Visible = true;
            viewmodelMesh.Visible = false;
            grenadeMesh.Visible = true;
            playerInstance = player;
        }
    }

    public void _on_damage_area_body_entered(Node3D body)
    {
        if(body.GetType() == typeof(StandardFish))
        {
            if(!animPlayer.IsPlaying())
                animPlayer.Play("explosion");
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
        if(animName == "explosion")
        {
            damageArea.Monitoring = false;
            damageArea.Visible = false;
            viewmodelMesh.Visible = true;
            grenadeMesh.Visible = false;
            canShoot = true;
        }
    }
}
