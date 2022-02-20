using Godot;
using System;

public class Perspective : Camera
{
	private RayCast cursor3d;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor3d = GetNode<RayCast>("Cursor3d");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
//		Vector2 mousePos = GetViewport().GetMousePosition();
//		Plane dropPlane = new Plane(new Vector3(0, 0, 100), 100);
//		Vector3? mousePos3D = dropPlane.IntersectRay(ProjectRayOrigin(mousePos), ProjectRayNormal(mousePos));
//
//		if (mousePos3D == null)
//		{
//			mousePos3D = new Vector3(0, 0, 0);
//		}
//
//		Vector3 mousePos3DNormal = ((Vector3)mousePos3D);
//
//		cursor3d.CastTo = new Vector3(mousePos3DNormal.x * 1, mousePos3DNormal.y * 1, mousePos3DNormal.z * 1);
//		cursor3d.ForceRaycastUpdate();
//
//		GD.PrintS(cursor3d.CastTo);
//
//		if (cursor3d.IsColliding())
//		{
//			GD.PrintS("We Colliding!");
//		}
		
//		GD.PrintS(mousePos3D);
	}
	
	private void _on_Tunnel_Brick2_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		GetNode<MeshInstance>("/root/Spatial/Tunnel_Brick2/View_Target").Visible = true;
	}
}
