using Godot;
using System;

public class Goal : Area
{
	[Export]
	private PackedScene nextScene;
	
//	[Signal]
//	public delegate void levelCompletedSignal(Vector3 pos);
	
//	public override void _Ready()
//	{
//		Godot.Collections.Array walkables = GetTree().GetNodesInGroup("walkables");
//		for (int i = 0; i < walkables.Count; i++) {
//			Godot.Object walkable = (Godot.Object)walkables[i];
//			Connect("levelCompletedSignal", walkable, "_levelCompleted");
//		}
//	}
	
	private void _on_Goal_body_entered(object body)
	{
		Godot.Collections.Array walkables = GetTree().GetNodesInGroup("walkables");
		for (int i = 0; i < walkables.Count; i++)
		{
			Godot.Object walkable = (Godot.Object)walkables[i];
			if (walkable.HasMethod("_levelCompleted"))
			{
				walkable.Call("_levelCompleted", Translation);
			}
		}
		
//		EmitSignal("levelCompletedSignal", Translation);
//		
//		GetTree().ChangeSceneTo(nextScene);
	}
}



