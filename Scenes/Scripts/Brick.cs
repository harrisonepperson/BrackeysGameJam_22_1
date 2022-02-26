using Godot;
using System;

public class Brick : Area
{
	[Export]
	private float rotationSpeed = 5F;

	private MeshInstance view;
	private bool isMouseOn = false;
	private bool isRotating = false;
	private bool canClick = true;
	private bool playerOnBlock = false;
	private float targetDegrees;
	
	AudioStreamPlayer3D blockRotateSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		blockRotateSound = GetNode<AudioStreamPlayer3D>("Sounds");
		
		view = GetNode<MeshInstance>("View_Target");
		targetDegrees = RotationDegrees.y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (isRotating)
		{
			if (Mathf.IsEqualApprox(RotationDegrees.y, targetDegrees, 30F))
			{
				canClick = true;
			}

			if (!Mathf.IsEqualApprox(RotationDegrees.y, targetDegrees, 1F))
			{
				Vector3 rot = RotationDegrees;
				rot.y = targetDegrees;

				RotationDegrees = RotationDegrees.LinearInterpolate(rot, rotationSpeed * delta);
			}
			else
			{
				Vector3 rot = RotationDegrees;
				rot.y = Mathf.Stepify(rot.y, 90F);
				RotationDegrees = rot;
				isRotating = false;
			}
		}

		if (isMouseOn && canClick && !playerOnBlock)
		{
			bool shouldPlaySound = false;
			if (Input.IsActionJustPressed("Primary_Click"))
			{
				isRotating = true;
				canClick = false;
				targetDegrees = Mathf.Stepify(RotationDegrees.y + 90, 90);
				shouldPlaySound = true;
			}
			else if (Input.IsActionJustPressed("Secondary_Click"))
			{
				isRotating = true;
				canClick = false;
				targetDegrees = Mathf.Stepify(RotationDegrees.y - 90, 90);
				shouldPlaySound = true;
			}
			
			if (shouldPlaySound) {
				if(!blockRotateSound.IsPlaying())
				{
					blockRotateSound.Play();
				}
			}
		}
	}

	private void _on_Tunnel_Brick_mouse_entered()
	{
//		view.Visible = true;
		isMouseOn = true;
	}

	private void _on_Tunnel_Brick_mouse_exited()
	{
//		view.Visible = false;
		isMouseOn = false;
	}
	
	private void _on_Brick_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		if (!playerOnBlock && OS.HasTouchscreenUiHint()) {
			if (@event is InputEventScreenTouch touch && touch.Pressed)
			{
				isRotating = true;
				canClick = false;
				targetDegrees = Mathf.Stepify(RotationDegrees.y + 90, 90);

				if(!blockRotateSound.IsPlaying())
				{
					blockRotateSound.Play();
				}
			}
		}
	}
	
	private void _on_Player_Detector_body_entered(object body)
	{
		playerOnBlock = true;
	}
	
	private void _on_Player_Detector_body_exited(object body)
	{
		playerOnBlock = false;
	}
}
