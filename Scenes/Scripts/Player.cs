using Godot;
using System;

public class Player : KinematicBody
{
	[Export]
	private float debug_Timescale = 1;
	
	[Export]
	private int StepsToHideDirectionHint = 1;
	
	[Export]
	private float cameraFollowSpeed = 4.5F;
	
	private Spatial directionHint;
	private Joy_Area joyStick;
	private Control joyStickControl;

	private bool isSpawning = true;
	private RayCast wallCheck;
	private RayCast stepCheck;
	private int stepScale = 2;

	private AnimationPlayer animationPlayer;
	private static string IdleAnimationName = "IdleLoop";
	private static string WalkAnimationName = "WalkLoop";
	
	AnimationPlayer hopAnim;
	bool checkHopAnimState = false;
	bool stickToGround = true;
	Spatial Camera_Carrier;
	Spatial Heading_Container;
	Spatial Hopping_Container;
	Vector3 targetPos;
	
	AudioStreamPlayer3D playerMoveSound;
	Particles impactCrumbles;
	Particles impactDust;
	
	public override void _Ready()
	{
		directionHint = GetNode<Spatial>("Direction_Hint");
		joyStick = GetNode<Joy_Area>("Camera_Carrier/Camera/JoyStick/Joy_Area");
		joyStickControl = GetNode<Control>("Camera_Carrier/Camera/JoyStick");
		
		Camera_Carrier = GetNode<Spatial>("Camera_Carrier");
		Heading_Container = GetNode<Spatial>("Heading_Container");
		Hopping_Container = GetNode<Spatial>("Heading_Container/Hopping_Container");
		
		playerMoveSound = GetNode<AudioStreamPlayer3D>("Heading_Container/Hopping_Container/Impact_Effects/Sound");
		impactCrumbles = GetNode<Particles>("Heading_Container/Hopping_Container/Impact_Effects/Crumbles");
		impactDust = GetNode<Particles>("Heading_Container/Hopping_Container/Impact_Effects/Dust");
		
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
		
		Engine.TimeScale = debug_Timescale;
	}


	public override void _Process(float delta)
	{
		Vector2 joyHeading = joyStick.joyHeading;
		
		if (StepsToHideDirectionHint == 0)
		{
			directionHint.Visible = false;
		}
		
		if (stickToGround && !isSpawning)
		{
			stepCheck.ForceRaycastUpdate();
			if (stepCheck.IsColliding())
			{
				// Get collision point
				Vector3 collisionPoint = stepCheck.GetCollisionPoint();
				
				// Translate player note to match y offset of collision point
				Vector3 pos = Hopping_Container.Translation;
				pos.y = 0.15F + collisionPoint.y;
				Hopping_Container.Translation = pos;
			}
		}
		
		if (checkHopAnimState)
		{
			// Wait For animation to stop
			if (hopAnim.IsPlaying())
			{
				stickToGround = false;
				Translation = Translation.LinearInterpolate(targetPos, cameraFollowSpeed * delta);
				return;
			} else {
				stickToGround = true;
				
				// Teleport player to new pos
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
			Input.IsActionJustPressed("move_right") ||
			joyHeading != Vector2.Zero
		)
		{
			isSpawning = false;
			Vector3 pos = Translation;
			Vector3 dir = new Vector3(0, 0, 0);
			
			if (Input.IsActionJustPressed("move_forward") || joyHeading == Vector2.Up)
			{
				dir.z -= stepScale;
			}
			else if (Input.IsActionJustPressed("move_backward") || joyHeading == Vector2.Down)
			{
				dir.z += stepScale;
			}
			else if (Input.IsActionJustPressed("move_left") || joyHeading == Vector2.Left)
			{
				dir.x -= stepScale;
			}
			else if (Input.IsActionJustPressed("move_right") || joyHeading == Vector2.Right)
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
		}
	}
	
	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		if (OS.HasTouchscreenUiHint()) {
			joyStickControl.Visible = true;
		} else {
			directionHint.Visible = true;
		}
	}
}
