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
	private float targetDegrees;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
			} else {
				Vector3 rot = RotationDegrees;
				rot.y = Mathf.Stepify(rot.y, 90F);
				RotationDegrees = rot;
				isRotating = false;
			}
		}
		
		if (isMouseOn && canClick)
		{
			if (Input.IsActionJustPressed("Primary_Click"))
			{
				isRotating = true;
				canClick = false;
				targetDegrees = Mathf.Stepify(RotationDegrees.y + 90, 90);
			}
			else if (Input.IsActionJustPressed("Secondary_Click"))
			{
				isRotating = true;
				canClick = false;
				targetDegrees = Mathf.Stepify(RotationDegrees.y - 90, 90);
			}
		}
	}

	private void _on_Tunnel_Brick_mouse_entered()
	{
		view.Visible = true;
		isMouseOn = true;
	}
	
	private void _on_Tunnel_Brick_mouse_exited()
	{
		view.Visible = false;
		isMouseOn = false;
	}
}
