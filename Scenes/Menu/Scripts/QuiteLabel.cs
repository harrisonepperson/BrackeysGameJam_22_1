using Godot;
using System;

public class QuiteLabel : Label
{
	private void _on_QuiteLabel_gui_input(object e)
	{
		if (e is InputEventMouseButton && ((InputEventMouseButton)e).Pressed && ((InputEventMouseButton)e).ButtonIndex == 1)
		{
			GetTree().Quit();
		}
		if (OS.HasTouchscreenUiHint() && e is InputEventScreenTouch touch && touch.Pressed) {
			GetTree().Quit();
		}
	}
}
