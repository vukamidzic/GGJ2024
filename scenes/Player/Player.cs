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
	Camera3D viewmodelCamera;
	public RayCast3D raycast;
    public Gun gun;
	public Pecaljka pecaljka;
	public Grenade grenade;
	public Canon canon;
	public Mines mines;
	public Item item;
	public bool canOpen;
	public bool canShoot;
	public int levelCounter;
	public Node3D usableObject;
	public ColorRect colorRect;
	public AnimationPlayer animPlayer;
	AudioStreamPlayer audioStream;
	public AudioStreamPlayer audioStream2;
	Item[] items;
	public override void _Ready()
	{
		state = STATE.WALK;
		camera = GetNode<Camera3D>("Camera3D");
		viewmodelCamera = camera.GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport").GetNode<Camera3D>("ViewmodelCamera");
        pecaljka = camera.GetNode<Pecaljka>("Pecaljka");
		gun = camera.GetNode<Gun>("Gun");
		grenade = camera.GetNode<Grenade>("Grenade");
		canon = GetParent().GetNode<Canon>("Canon");
		mines = camera.GetNode<Mines>("Mines");
		raycast = camera.GetNode<RayCast3D>("RayCast3D");
		colorRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("ColorRect");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		audioStream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		audioStream2 = GetNode<AudioStreamPlayer>("AudioStreamPlayer2");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		colorRect.Visible = false;
		canOpen = false;
		canShoot = true;
		levelCounter = 0;
		items = new Item[4]{pecaljka, gun, mines, canon};
		item = items[0];
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
		viewmodelCamera.GlobalTransform = camera.GlobalTransform;
		switch(state)
		{
			case STATE.WALK:
				walkState(delta);
                shootState(delta);
				useState();
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

		if(moveVector != new Vector3(0.0f, 0.0f, 0.0f) && !audioStream.Playing)
			audioStream.Play();
		else if(moveVector == new Vector3(0.0f, 0.0f, 0.0f) && audioStream.Playing)
			audioStream.Stop();
		//quit state
		if(Input.IsActionJustPressed("exit"))
			state = STATE.QUIT;

		MoveAndSlide();
	}

    public void shootState(double delta)
    {
        if(Input.IsActionJustPressed("accept") && canShoot)
            item.shoot(this);
    }

	public void useState()
	{
		if(canOpen && Input.IsActionJustPressed("use") && usableObject != null)
			((Kutija)usableObject).open(this);
	}

	public void quitState()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		if(Input.IsActionJustPressed("exit"))
			GetTree().Quit();
	}

	public void fail()
	{
		if(item.GetType() == typeof(Pecaljka)) pecaljka.animPlayer.Play("golden");
		if(item.GetType() == typeof(Gun)) gun.animPlayer2.Play("golden");
		if(item.GetType() == typeof(Mines)) mines.animPlayer.Play("golden");
		if(item.GetType() == typeof(Canon)) canon.animPlayer.Play("golden");
		canShoot = false;
	}

	public void levelUp()
	{
		levelCounter++;
		item.Visible = false;
		item = items[levelCounter];
		item.Visible = true;
		canShoot = true;
	}
}
