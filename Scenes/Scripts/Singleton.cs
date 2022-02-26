using Godot;
using System;

public class Singleton : Node
{
	public float masterAudioLevel;
	public float musicAudioLevel;
	public float sfxAudioLevel;
	
	public bool bloomEnabled = true;
	
	public override void _Ready()
	{
		ResourceLoader.Load("res://Audio/music_loop.wav");
		ResourceLoader.Load("res://Audio/Sounds/player_move.mp3");
		ResourceLoader.Load("res://Audio/Sounds/block_rotate.mp3");
		ResourceLoader.Load("res://Audio/Sounds/goal_proximity.wav");
		ResourceLoader.Load("res://Audio/Sounds/level_complete.wav");
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("restart")) {
			GetTree().ReloadCurrentScene();
		}
	}
}
