using Godot;
using System;

public class Goal : Area
{
	[Export]
	private PackedScene nextScene;
	private AnimationPlayer anim;
	
	AudioStreamPlayer goalCompleteSound;
	
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
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		goalCompleteSound = GetNode<AudioStreamPlayer>("GoalComplete");
	}
	
	public void _on_Goal_body_entered(object body)
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
		
		playAnimation();
		
//		EmitSignal("levelCompletedSignal", Translation);
//		
//		GetTree().ChangeSceneTo(nextScene);
	}
	
	public void playAnimation()
	{
		goalCompleteSound.Play();
		anim.Play("GoalReached");
	}

	private async void _on_AnimationPlayer_animation_finished(String anim_name)
	{		
//		await ToSignal(GetTree().CreateTimer(1), "timeout");
		GetTree().ChangeSceneTo(nextScene);
	}
}


