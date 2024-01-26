using Godot;
using System;

public partial class Player : CharacterBody3D
{
    enum STATE {
        WALK,
        FISH,
        QUIT
    }
    [Export]
    public float speed;
    [Export]
    public float mouseSens;
    STATE state;
    Godot.Vector3 moveVector;
    Camera3D camera;
    public override void _Ready()
    {
        state = STATE.WALK;
        camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            camera.RotateX(Mathf.DegToRad(mouseMotion.Relative.Y*mouseSens*-1));
            camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.X, -75.0f, 75.0f), 0.0f, 0.0f);
            this.RotateY(Mathf.DegToRad(mouseMotion.Relative.X*mouseSens*-1)); 
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        switch(state)
        {
            case STATE.WALK:
                walkState(delta);
                break;
            case STATE.FISH:
                break;
            case STATE.QUIT:
                quitState();
                break;
        }
    }

    public void walkState(double delta)
    {
        moveVector = new Godot.Vector3(0.0f, 0.0f, 0.0f);
        if(Input.IsActionPressed("up"))
            moveVector += -camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("down"))
            moveVector += camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("left"))
            moveVector += -camera.GlobalTransform.Basis.X;
        if(Input.IsActionPressed("right"))
            moveVector += camera.GlobalTransform.Basis.X;
        this.Velocity = moveVector.Normalized() * speed;

        //quit state
        if(Input.IsActionJustPressed("exit"))
            state = STATE.QUIT;

        MoveAndSlide();
    }

    public void quitState()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        if(Input.IsActionJustPressed("exit"))
            GetTree().Quit();
    }
}
