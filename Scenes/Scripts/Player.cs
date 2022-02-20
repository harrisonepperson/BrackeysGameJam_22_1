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

			if (Input.IsActionJustPressed("move_forward"))
			{
				dir.z -= stepScale;
				animationPlayer.Play(WalkAnimationName);
				animationPlayer.Queue(IdleAnimationName);
			}
			else if (Input.IsActionJustPressed("move_backward"))
			{
				dir.z += stepScale;
				animationPlayer.PlayBackwards(WalkAnimationName);
				animationPlayer.Queue(IdleAnimationName);
			}
			else if (Input.IsActionJustPressed("move_left"))
			{
				dir.x -= stepScale;
				animationPlayer.PlayBackwards(WalkAnimationName);
				animationPlayer.Queue(IdleAnimationName);
			}
			else if (Input.IsActionJustPressed("move_right"))
			{
				dir.x += stepScale;
				animationPlayer.PlayBackwards(WalkAnimationName);
				animationPlayer.Queue(IdleAnimationName);
			}

			wallCheck.CastTo = dir;
			wallCheck.ForceRaycastUpdate();

			if (wallCheck.IsColliding())
			{
			}
			else
			{
				pos += dir;
				StepsToHideDirectionHint--;
			}

			Translation = pos;
		}
	}
}
