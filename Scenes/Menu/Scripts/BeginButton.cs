using Godot;
using System;

public class BeginButton : TextureButton
{
	[Export]
	private PackedScene FirstScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
	}
	
	
	
	private void _on_TextureButton_button_down()
	{
		GetTree().ChangeSceneTo(FirstScene);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

