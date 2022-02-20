using Godot;
using System;

public class Player : KinematicBody
{
	[Export]
	private int StepsToHideDirectionHint = 3;

	private RayCast wallCheck;
	private int stepScale = 2;

	private AnimationPlayer animationPlayer;
	private static string IdleAnimationName = "IdleLoop";
	private static string WalkAnimationName = "WalkLoop";

	public override void _Ready()
	{
		GetNode<Spatial>("Direction_Hint").Visible = true;
		wallCheck = GetNode<RayCast>("Wall_Checker");


		//Set up player animation
		var animationPlayerPath = "character/AnimationPlayer";
		animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);
		if (animationPlayer == null)
		{
			throw new Exception("Animation Player not found at path: " + animationPlayerPath);
		}
		var idle = animationPlayer.GetAnimation(IdleAnimationName);
		idle.Loop = true;
		idle.TrackSetInterpolationType(0, Animation.InterpolationType.Nearest);
		var walk = animationPlayer.GetAnimation(WalkAnimationName);
		walk.TrackSetInterpolationType(0, Animation.InterpolationType.Nearest);
	}


	public override void _Process(float delta)
	{
		if (StepsToHideDirectionHint == 0)
		{
			GetNode<Spatial>("Direction_Hint").Visible = false;
		}

		if (
			Input.IsActionJustPressed("move_forward") ||
			Input.IsActionJustPressed("move_backward") ||
			Input.IsActionJustPressed("move_left") ||
			Input.IsActionJustPressed("move_right")
		)
		{
			Vector3 pos = Translation;
			Vector3 dir = new Vector3(0, 0, 0);
			bool reverseAnim = false;

			if (Input.IsActionJustPressed("move_forward"))
			{
				dir.z -= stepScale;
			}
			else if (Input.IsActionJustPressed("move_backward"))
			{
				dir.z += stepScale;
				reverseAnim = true;
			}
			else if (Input.IsActionJustPressed("move_left"))
			{
				dir.x -= stepScale;
				reverseAnim = true;
			}
			else if (Input.IsActionJustPressed("move_right"))
			{
				dir.x += stepScale;
			}

			wallCheck.CastTo = dir;
			wallCheck.ForceRaycastUpdate();

			if (!wallCheck.IsColliding())
			{
				pos += dir;
				StepsToHideDirectionHint--;
				
				if (reverseAnim)
				{
					animationPlayer.PlayBackwards(WalkAnimationName);
				} else {
					animationPlayer.Play(WalkAnimationName);
				}
				animationPlayer.Queue(IdleAnimationName);
				
				Translation = pos;
			}
		}
	}
}
