using Godot;
using System;

public class Ambient_Nature_Collection : Spatial
{
	[Export]
	private bool showGrassTop = true;
	
	[Export]
	private bool showTree = true;
	
	[Export]
	private bool showFallingLeaves = true;
	
	[Export]
	private bool showFlowers = true;
	
	[Export]
	private bool showButterflies = true;
	
	[Export]
	private bool showVines = true;

	public override void _Ready()
	{
		GetNode<RigidBody>("Floater/Tree").Visible = showTree;
		GetNode<CollisionShape>("Floater/Tree/Collider").Disabled = !showTree;
		GetNode<Particles>("Floater/Tree/Tree/Falling Leaves").Emitting = showFallingLeaves;
		GetNode<Spatial>("Floater/GrassTop").Visible = showGrassTop;
		GetNode<Particles>("Floater/Flower_Patch").Emitting = showFlowers;
		GetNode<Spatial>("Floater/Vines").Visible = showVines;
		GetNode<Particles>("Floater/Butterflies/Flap").Emitting = showButterflies;
	}
}
