using Godot;
using System;

public class Singleton : Node
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("restart")) {
			GetTree().ReloadCurrentScene();
		}
		if (Input.IsActionJustPressed("ui_cancel")) {
			GetTree().Quit();
		}
	}
}
