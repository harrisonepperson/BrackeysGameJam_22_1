using Godot;
using System;

public class Joy_Area : Area2D
{
	private double pi = Math.PI;
	private bool isPressed = false;
	private Sprite joyOuter;
	private Sprite joyInner;
	
	public Vector2 joyHeading = Vector2.Zero;
	
	public override void _Ready()
	{
		joyOuter = GetNode<Sprite>("JoyOuter");
		joyInner = GetNode<Sprite>("JoyInner");
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (OS.HasTouchscreenUiHint() && isPressed) {
			Vector2 heading = joyHeading;
			
			// Detect touch end
			if (@event is InputEventScreenTouch touch && !touch.Pressed)
			{
				isPressed = false;
				
				Vector2 joyOuterSize = Scale;
				Vector2 joyInnerSize = joyInner.Scale;
				joyInner.Position = new Vector2((joyOuterSize.x - joyInnerSize.x) / 2, (joyOuterSize.y - joyInnerSize.y) / 2);
				heading = Vector2.Zero;
			}
			
			// Detect touch drag
			if (@event is InputEventScreenDrag drag)
			{
				// Get touchPos
				Vector2 screenSize = OS.WindowSize;
				Vector2 mousePos = drag.Position;
				
				// Get Center of JoyStick
				Vector2 joyPos = GlobalPosition;
				Vector2 joySize = Scale;
				Vector2 joyCenter = new Vector2(joyPos.x + (joySize.x / 2), joyPos.y + (joySize.y / 2));

				float joyAngle = joyCenter.AngleToPoint(mousePos);
				float joyDist = joyCenter.DistanceTo(mousePos);

				if (joyDist >= 8)
				{
					// Determine direction
					if ((pi / 4) < joyAngle && joyAngle <= pi - (pi / 4)){
						// UP
						heading = Vector2.Up;
					} else if (Math.Abs(joyAngle) <= (pi / 4)) {
						// LEFT
						heading = Vector2.Left;
					} else if ((pi / 4) < 0 - joyAngle && 0 - joyAngle <= pi - (pi / 4)) {
						// DOWN
						heading = Vector2.Down;
					} else {
						// RIGHT
						heading = Vector2.Right;
					}
				} else {
					heading = Vector2.Zero;
				}
				
				// Determine new postion for visual
				Vector2 dir = (joyCenter.DirectionTo(mousePos) * joyDist).Clamped(64);
				joyInner.Position = dir; //new Vector2(46,46);
			}
			
			joyHeading = heading;
		}
	}
	
	private void _on_Joy_Area_input_event(object viewport, object @event, int shape_idx)
	{
		if (OS.HasTouchscreenUiHint()) {
			if (@event is InputEventScreenTouch touch && touch.Pressed)
			{
				isPressed = true;
			}
		}
	}
}



