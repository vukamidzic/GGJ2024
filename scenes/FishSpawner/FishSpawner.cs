using Godot;
using System;

public partial class FishSpawner : Node3D
{
    [Export]
    public int enemyCounter;
    [Export]
    PackedScene fishScene;
    [Export]
    PackedScene goldenFishScene;
    StandardFish standardFish;
    GoldenFish goldenFish;
    Timer timer;
    float offset = 1.0f;
    float position = -8.0f;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Start();
    }

    public void _on_timer_timeout()
    {
        Node fish = fishScene.Instantiate();
        standardFish = (StandardFish)fish;
        Node goldFish = goldenFishScene.Instantiate();
        goldenFish = (GoldenFish)goldFish;
        // if(position > -10.0f)
        //     offset = 1.0f;
        // if(position < -8.0f)
        //     offset = -1.0f;
        // position += offset;
        // standardFish.GlobalPosition += new Vector3(position, 0.0f, 0.0f);
        // if(timer.WaitTime>2.0f)
        //     timer.WaitTime -= 0.5f;
        enemyCounter--;
        if(enemyCounter > 0)
        {
            AddChild(standardFish);
            timer.Start();
        }
        else
        {
            AddChild(goldenFish);
            enemyCounter = 5;
        }
    }
}
