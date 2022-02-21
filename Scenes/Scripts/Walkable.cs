using Godot;
using System;

public class Walkable : RigidBody
{
	[Export]
	private float impactSpeed = 10;
	[Export]
	private float loftSpeed = (float)1;
	[Export]
	private bool shouldDropOnLevelFinish = true;
	
	private bool steppedOn = false;
	
	private float toleranceRange = (float)0.0005;
	private float top = (float)0;
	private float bottom = (float)-0.2;
	private float lerpTarget;
	private float lerpSpeed;
	
	public override void _Process(float delta)
	{
		if (steppedOn)
		{
			Vector3 pos = Translation;
			if (lerpTarget - toleranceRange <= pos.y && pos.y <= lerpTarget + toleranceRange)
			{
				if (lerpTarget == bottom)
				{
					lerpTarget = top;
					lerpSpeed = loftSpeed;
				} else {
					lerpTarget = bottom;
					lerpSpeed = impactSpeed;
					steppedOn = false;
					
					pos.y = top;
					Translation = pos;
				}
				
			} else {
				pos.y = lerpTarget;
				Translation = Translation.LinearInterpolate(pos, lerpSpeed * delta);
			}
		}
	}
	
	public void _steppedOn()
	{
		steppedOn = true;
		lerpTarget = bottom;
		lerpSpeed = impactSpeed;
	}
}
