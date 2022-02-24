using Godot;
using System;

public class OutroBG : Panel
{
	private ulong time_start = 0;
	
	private string menuScene = "res://Scenes/Menu/Menu3D.tscn";
	public override void _Ready()
	{
		GD.Load<PackedScene>(menuScene);
		
		var newColor = this.Modulate;
		newColor.a = 0;
		this.Modulate = newColor;
		time_start = OS.GetUnixTime();
	}

	public override void _Process(float delta)
	{
		var newColor = this.Modulate;
		newColor.a = Mathf.Min(1, newColor.a + (delta / 10F));
		this.Modulate = newColor;


		var time_elapsed = OS.GetUnixTime() - time_start;

		if (time_elapsed > 10)
		{
			GetTree().ChangeScene(menuScene);
		}
	}
}
