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

	public override void _Ready()
	{
//		GD.Load<PackedScene>(nextScene);
	}
	
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
		
		AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("GoalReached");
		
//		EmitSignal("levelCompletedSignal", Translation);
//		
//		GetTree().ChangeSceneTo(nextScene);
	}
	
	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		GetTree().ChangeSceneTo(nextScene);
	}
}
