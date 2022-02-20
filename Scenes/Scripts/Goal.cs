using Godot;
using System;

public class Goal : Area
{
	[Export]
	private PackedScene nextScene;
	
	private void _on_Goal_body_entered(object body)
	{
		GD.PrintS("Hit the Goal!");
		GetTree().ChangeSceneTo(nextScene);
	}
}



