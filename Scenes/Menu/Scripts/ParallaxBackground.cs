using Godot;
using System;

public class ParallaxBackground : Godot.ParallaxBackground
{
	[Export]
	float mouseParalaxInfluence = 2;
	
	Vector2 mousePos = new Vector2(0, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Vector2 newMousePos = GetViewport().GetMousePosition();
		Vector2 offset = (newMousePos - mousePos).Normalized();
		mousePos = newMousePos;
		
		// Godot.ParallaxBackground sample = (Godot.ParallaxBackground)this;
		var newScrollOffset = new Vector2(this.ScrollOffset);
		newScrollOffset.x -= 50 * delta + (offset.x * mouseParalaxInfluence);
		newScrollOffset.y += 40 * delta - (offset.y * mouseParalaxInfluence * 4/5);
		this.ScrollOffset = newScrollOffset;
	}
}
