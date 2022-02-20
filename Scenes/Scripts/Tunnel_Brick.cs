using Godot;
using System;

public class Tunnel_Brick : Area
{
	private MeshInstance view;
	private bool isLocked = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		view = GetNode<MeshInstance>("View_Target");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!isLocked && Input.IsActionJustPressed("Primary_Click"))
		{
			Vector3 rot = RotationDegrees;
			rot.y += 90;
			
			RotationDegrees = rot;
		}
	}

	private void _on_Tunnel_Brick_mouse_entered()
	{
		view.Visible = true;
		isLocked = false;
		
	}
	
	private void _on_Tunnel_Brick_mouse_exited()
	{
		view.Visible = false;
		isLocked = true;
	}
}
