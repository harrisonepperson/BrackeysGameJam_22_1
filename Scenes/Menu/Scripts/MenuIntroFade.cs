using Godot;
using System;

public class MenuIntroFade : Panel
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var newColor = this.Modulate;
		newColor.a = 1;
		this.Modulate = newColor;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var newColor = this.Modulate;
		newColor.a = Mathf.Min(1, newColor.a - (delta / 5F));
		this.Modulate = newColor;
	}
}
