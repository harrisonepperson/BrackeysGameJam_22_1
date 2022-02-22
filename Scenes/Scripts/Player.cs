using Godot;
using System;

public class Player : KinematicBody
{
	[Export]
	private int StepsToHideDirectionHint = 3;
	
	[Export]
	private float cameraFollowSpeed = 5F;

	private RayCast wallCheck;
	private RayCast stepCheck;
	private int stepScale = 2;

	private AnimationPlayer animationPlayer;
	private static string IdleAnimationName = "IdleLoop";
	private static string WalkAnimationName = "WalkLoop";
	
	AnimationPlayer hopAnim;
	bool checkHopAnimState = false;
	Spatial Camera_Carrier;
	Spatial Heading_Container;
	Spatial Hopping_Container;
	Vector3 targetPos;
	
	AudioStreamPlayer playerMoveSound;
	Particles impactCrumbles;
	Particles impactDust;
	
	public override void _Ready()
	{
		Camera_Carrier = GetNode<Spatial>("Camera_Carrier");
		Heading_Container = GetNode<Spatial>("Heading_Container");
		Hopping_Container = GetNode<Spatial>("Heading_Container/Hopping_Container");
		
		playerMoveSound = GetNode<AudioStreamPlayer>("Heading_Container/Hopping_Container/Impact_Effects/Sound");
		impactCrumbles = GetNode<Particles>("Heading_Container/Hopping_Container/Impact_Effects/Crumbles");
		impactDust = GetNode<Particles>("Heading_Container/Hopping_Container/Impact_Effects/Dust");
		
		GetNode<Spatial>("Direction_Hint").Visible = true;
		wallCheck = GetNode<RayCast>("Wall_Checker");
		stepCheck = GetNode<RayCast>("Heading_Container/Hopping_Container/Foot");


		//Set up player animation
		var animationPlayerPath = "Heading_Container/Hopping_Container/character/AnimationPlayer";
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
		
		// Set up hop animation
		hopAnim = GetNode<AnimationPlayer>("HopAnim");
	}


	public override void _Process(float delta)
	{
		if (StepsToHideDirectionHint == 0)
		{
			GetNode<Spatial>("Direction_Hint").Visible = false;
		}
		
//		if (Input.IsActionPressed("Primary_Click"))
//		{
//			GD.PrintS(Camera_Carrier.Translation, targetPos);
//		}
//		Camera_Carrier.Translation = Camera_Carrier.Translation.LinearInterpolate(targetPos, cameraFollowSpeed * delta);
		
		if (checkHopAnimState)
		{
			// Wait For animation to stop
			if (hopAnim.IsPlaying())
			{
//				Vector3 lerpTarget = new Vector3(targetPos.x, targetPos.y, targetPos.z);
//				Camera_Carrier.Translation = Camera_Carrier.Translation.LinearInterpolate(lerpTarget, cameraFollowSpeed * delta);
				return;
			} else {
				// Teleport player to new pos
//				Camera_Carrier.Translation = Vector3.Zero;
				Hopping_Container.Translation = Vector3.Zero;
				Translation = targetPos;
				
				// Play foot dust and sound
				playerMoveSound.Play();
//				impactDust.Emitting = true;
				
				// Check if floor knows how to sink
				stepCheck.ForceRaycastUpdate();
				if (stepCheck.IsColliding())
				{
					Godot.Object steppedOn = stepCheck.GetCollider();
					if (steppedOn != null && steppedOn.HasMethod("_steppedOn"))
					{
						// Play pebble fall anim
//						impactCrumbles.Emitting = true;
						
						// Play block drop anim
						steppedOn.Call("_steppedOn");
					}
				}
				
				checkHopAnimState = false;
			}
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
				StepsToHideDirectionHint--;
				
				/* TODO:
					* Queue player key presses
					* Make Camera Follow player horizontal
					* Poll block pos and stick character to block while it sinks and rises
					* Release player input queque
				*/
				
				// Orient the player to face target block
				Vector3 rot = Heading_Container.Rotation;
				Vector2 heading = new Vector2(rot.x, rot.z);
				Vector2 clickHeading = new Vector2(dir.z, dir.x);
				float angle = Mathf.Rad2Deg(heading.AngleToPoint(clickHeading));
				
				rot.y = (rot.y + angle + 360) % 360;
				Heading_Container.RotationDegrees = rot;
				
				targetPos = pos + dir;
				
				// Play the hop animation
				animationPlayer.Play(WalkAnimationName);
				animationPlayer.Queue(IdleAnimationName);
				hopAnim.Play("Hop");
				checkHopAnimState = true;
			}
			
//			if (!wallCheck.IsColliding())
//			{
//				if (reverseAnim)
//				{
//					animationPlayer.PlayBackwards(WalkAnimationName);
//				}
//				else
//				{
//					animationPlayer.Play(WalkAnimationName);
//				}
//				animationPlayer.Queue(IdleAnimationName);
//			}
		}
	}
}
