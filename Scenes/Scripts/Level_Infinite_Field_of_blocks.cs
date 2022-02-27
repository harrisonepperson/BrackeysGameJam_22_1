using Godot;
using System;
using System.Collections.Generic;

public class Level_Infinite_Field_of_blocks : Spatial
{
	KinematicBody player;
	private Random rand = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<KinematicBody>("SpawnPoint/Player");
		GetNode<WorldEnvironment>("SpawnPoint/Default").Environment.FogEnabled = true;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eKey && eKey.Pressed && !eKey.Echo)
		{
			string key = OS.GetScancodeString(eKey.Scancode);
			if (key == "W" || key == "A" || key == "S" || key =="D")
			{
				Vector3 playerPos = player.Translation;

				if (key == "W") {
					playerPos += Vector3.Forward * 2;
				} else if (key == "S") {
					playerPos += Vector3.Back * 2;
				} else if (key == "A") {
					playerPos += Vector3.Left * 2;
				} else {
					playerPos += Vector3.Right * 2;
				}

				// Go over the visible square and comprise a list of points that are in an area around the players current view
				int circleBlockRadius = 32;
				List<Vector3> spotsToCheck = new List<Vector3>();
				for (int x = 0 - circleBlockRadius; x <= circleBlockRadius; x = x + 2)
				{
					for (int z = 0 - circleBlockRadius; z <= 4; z = z + 2)
					{
						Vector3 spot = new Vector3(playerPos.x + x, 0, playerPos.z + z).Round();
						float dist = playerPos.DistanceSquaredTo(spot);

						if (dist <= circleBlockRadius * circleBlockRadius)
						{
							spotsToCheck.Add(spot);
						}
					}
				}

				// Loop over all the bricks in the scene
				// Check if they are in the circle of view: leave alone :: set as able to be nuked
				Godot.Collections.Array walkables = GetTree().GetNodesInGroup("walkables");
				for (int i = 0; i < walkables.Count; i++)
				{
					Godot.RigidBody walkable = (Godot.RigidBody)walkables[i];

					Vector3 approxWalkLoc = walkable.GlobalTransform.origin.Round();

					float dist = playerPos.DistanceSquaredTo(approxWalkLoc);
					bool spotCovered = spotsToCheck.Contains(approxWalkLoc);

					if (spotCovered)
					{
						spotsToCheck.Remove(approxWalkLoc);

						if (walkable.IsInGroup("out_of_view"))
						{
							walkable.RemoveFromGroup("out_of_view");
						}
					} else if (dist > 640) {
						if (!walkable.IsInGroup("out_of_view"))
						{
							walkable.AddToGroup("out_of_view");
						}
					}
				}
				
				// Loop over all blocks out of view and move them to an uncovered spot in the ring (if any)
				Godot.Collections.Array outOfView = GetTree().GetNodesInGroup("out_of_view");
				for (int i = 0; i < outOfView.Count; i ++)
				{
					if (spotsToCheck.Count > 0)
					{
						Godot.RigidBody plat = (Godot.RigidBody)outOfView[i];

						Vector3 pos = spotsToCheck[0];
						pos.x += ((float)rand.Next(-100, 100) / 500);
						pos.z += ((float)rand.Next(-100, 100) / 500);

						plat.Translation = pos;
						spotsToCheck.RemoveAt(0);
					} else {
						break;
					}
				}
			}
		}
	}
}
