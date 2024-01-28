using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	public void _on_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level.tscn");
	}

	public void _on_button_2_pressed()
	{
		GetTree().Quit();
	}
}
