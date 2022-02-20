using Godot;
using System;

public class ParallaxBackground : Godot.ParallaxBackground
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Godot.ParallaxBackground sample = (Godot.ParallaxBackground)this;
		var newScrollOffset = new Vector2(this.ScrollOffset);
		newScrollOffset.x -= 50 * delta;
		newScrollOffset.y += 40 * delta;
		this.ScrollOffset = newScrollOffset;
	}
}
