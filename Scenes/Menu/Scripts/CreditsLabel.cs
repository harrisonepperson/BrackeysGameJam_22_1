using Godot;
using System;

public class CreditsLabel : Label
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private string nextScene = "res://Scenes/Menu/Credits.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	private void _on_CreditsLabel_gui_input(object e)
	{
		if (e is InputEventMouseButton && ((InputEventMouseButton)e).Pressed && ((InputEventMouseButton)e).ButtonIndex == 1)
		{
			GetTree().ChangeScene(nextScene);
		}
		if (OS.HasTouchscreenUiHint() && e is InputEventScreenTouch touch && touch.Pressed) {
			GetTree().ChangeScene(nextScene);
		}
	}
}


