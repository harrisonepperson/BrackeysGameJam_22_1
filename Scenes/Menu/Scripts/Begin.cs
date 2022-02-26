using Godot;
using System;

public class Begin : Sprite3D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	private PackedScene FirstScene;


	private bool isMouseOn = false;
	private bool fade = false;

	private Sprite MenuLogo;
	private Label MenuText;
	private Goal goal;


	public override void _Ready()
	{
		MenuLogo = GetNode<Sprite>("../../Spatial/Camera/menu/Center/Logo");
		MenuText = GetNode<Label>("../../Spatial/Camera/menu/Center/Label");
		goal = GetNode<Goal>("../../Goal");

		var transparent = MenuLogo.Modulate;
		transparent.a = 0;
		MenuLogo.Modulate = transparent;
		MenuText.Modulate = transparent;
	}

	public override void _Process(float delta)
	{
		if (isMouseOn)
		{
			if (Input.IsActionJustPressed("Primary_Click"))
			{
				goal.playAnimation();
				fade = true;
			}
		}

		var newLogoColor = MenuLogo.Modulate;
		newLogoColor.a = fade ? newLogoColor.a - (delta / 1.5F) : Mathf.Min(1, newLogoColor.a + (delta / 5F));
		MenuLogo.Modulate = newLogoColor;

		var newTextColor = MenuText.Modulate;
		newTextColor.a = fade ? newTextColor.a - (delta * 1.2F) : Mathf.Min(1, newLogoColor.a + (delta / 5F));
		MenuText.Modulate = newTextColor;
	}

	private void _on_Area_mouse_entered()
	{
		isMouseOn = true;
	}

	private void _on_Area_mouse_exited()
	{
		isMouseOn = false;
	}
	
	private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		if (OS.HasTouchscreenUiHint()) {
			if (@event is InputEventScreenTouch touch && touch.Pressed)
			{
				goal.playAnimation();
				fade = true;
			}
		}
	}
}
