using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public enum STATE {
		WALK,
		FISH,
		QUIT
	}
	[Export]
	public float speed;
	[Export]
	public float mouseSens;
	public STATE state;
	Godot.Vector3 moveVector;
	Camera3D camera;
	public RayCast3D raycast;
    Gun gun;
	Pecaljka pecaljka;
	Item item;
	public override void _Ready()
	{
		state = STATE.WALK;
		camera = GetNode<Camera3D>("Camera3D");
        pecaljka = camera.GetNode<Pecaljka>("Pecaljka");
		raycast = camera.GetNode<RayCast3D>("RayCast3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		item = pecaljka;
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion && state != STATE.FISH)
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
                shootState(delta);
				break;
			case STATE.FISH:
				fishState(delta);
				shootState(delta);
				break;
			case STATE.QUIT:
				quitState();
				break;
		}
	}

	public void fishState(double delta)
	{
		if(Input.IsActionJustPressed("exit"))
			state = STATE.QUIT;
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
        moveVector.Y = 0.0f;
        this.Velocity = moveVector.Normalized() * speed;

		//quit state
		if(Input.IsActionJustPressed("exit"))
			state = STATE.QUIT;

		MoveAndSlide();
	}

    public void shootState(double delta)
    {
        if(Input.IsActionJustPressed("accept"))
            item.shoot(this);
    }

	public void quitState()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		if(Input.IsActionJustPressed("exit"))
			GetTree().Quit();
	}
}
