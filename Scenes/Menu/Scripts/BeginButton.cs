using Godot;
using System;

public class BeginButton : TextureButton
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
	}
	
	
	
	private void _on_TextureButton_button_down()
	{
		GetTree().ChangeScene("res://Scenes/World1.tscn");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

