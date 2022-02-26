using Godot;
using System;

public class Ambient_Nature_Collection : Spatial
{
	[Export]
	private bool showGrassTop = true;
	
	[Export]
	private bool showTree = false;
	
	[Export]
	private bool showFallingLeaves = true;
	
	[Export]
	private bool showFlowers = false;
	
	[Export]
	private bool showButterflies = false;
	
	[Export]
	private bool showVines = false;
	
	private Random rand = new Random();

	public override void _Ready()
	{
		Vector3 blockRot = new Vector3(0, rand.Next(0, 360), 0);
		
		GetNode<RigidBody>("Floater/Tree").Visible = showTree;
		GetNode<CollisionShape>("Floater/Tree/Collider").Disabled = !showTree;
		GetNode<Particles>("Floater/Tree/Tree/Falling Leaves").Emitting = showTree && showFallingLeaves;
		GetNode<Spatial>("Floater/Rock/GrassTop").Visible = showGrassTop;
		GetNode<Particles>("Floater/Flower_Patch").Emitting = showFlowers;
		GetNode<Spatial>("Floater/Vines").Visible = showVines;
		GetNode<Particles>("Floater/Butterflies/Flap").Emitting = showButterflies;
		
		GetNode<MeshInstance>("Floater/Rock").Rotation = blockRot;
	}
}
