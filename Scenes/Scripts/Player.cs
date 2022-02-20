using Godot;
using System;

public class Player : KinematicBody
{
	private RayCast wallCheck;
	private int stepScale = 2;
	
	public override void _Ready()
	{
		wallCheck = GetNode<RayCast>("Wall_Checker");
	}
	
	public override void _Process(float delta)
	{
		if(
			Input.IsActionJustPressed("move_forward") ||
			Input.IsActionJustPressed("move_backward") ||
			Input.IsActionJustPressed("move_left") ||
			Input.IsActionJustPressed("move_right")
		)
		{
			Vector3 pos = Translation;
			Vector3 dir = new Vector3(0, 0, 0);
		
			if(Input.IsActionJustPressed("move_forward"))
			{
				dir.z -= stepScale;
			} else if(Input.IsActionJustPressed("move_backward"))
			{
				dir.z += stepScale;
			} else if(Input.IsActionJustPressed("move_left"))
			{
				dir.x -= stepScale;
			} else if(Input.IsActionJustPressed("move_right"))
			{
				dir.x += stepScale;
			}
			
			wallCheck.CastTo = dir;
			wallCheck.ForceRaycastUpdate();
			
			if (wallCheck.IsColliding())
			{
			} else {
				pos += dir;
			}
			
			Translation = pos;
		}
	}
}
