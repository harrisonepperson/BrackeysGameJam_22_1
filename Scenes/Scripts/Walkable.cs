using Godot;
using System;

public class Walkable : RigidBody
{
	[Export]
	private float impactSpeed = 10F;
	[Export]
	private float loftSpeed = 1F;
	[Export]
	private bool shouldDropOnLevelFinish = true;
	
	private bool steppedOn = false;
	
	private float toleranceRange = 0.0005F;
	private float top = 0F;
	private float bottom = -0.2F;
	private float lerpTarget;
	private float lerpSpeed;
	
	private float shouldKill = 0;
	private float tick = 0;
	
	private Random rand = new Random();
	
	Particles crumbles;
	Particles dust;
	
	public override void _Ready()
	{
		GetNode<Particles>("Dust").Visible = true;
		GetNode<Particles>("Crumbles").Visible = true;
		
		GetNode<MeshInstance>("Visual1").Visible = false;
		crumbles = GetNode<Particles>("Crumbles");
		dust = GetNode<Particles>("Dust");
		
		string[] scenes = {"Visual1", "Visual2", "Visual3"};
		string sceneSelection = scenes[rand.Next(0,3)];
		
		Vector3 rot = new Vector3 (0,0,0);
		rot.y = rand.Next(0,4) * 90;
		
		MeshInstance rock = GetNode<MeshInstance>(sceneSelection);
		rock.RotationDegrees = rot;
		rock.Visible = true;
		
		Vector3 pos = Translation;
		pos.x += ((float)rand.Next(-100, 100) / 500);
		pos.z += ((float)rand.Next(-100, 100) / 500);
		Translation = pos;
	}
	
	public override void _Process(float delta)
	{
		if (shouldKill > 0)
		{
			steppedOn = false;
			if (tick < shouldKill)
			{
				tick ++;
			} else {
				Sleeping = false;
				GravityScale = 1.0F;
			}
		}
		
		if (steppedOn)
		{
			Vector3 pos = Translation;
			if (Mathf.IsEqualApprox(pos.y, lerpTarget, toleranceRange))
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
		
		dust.Emitting = true;
		crumbles.Emitting = true;
	}
	
	public void _levelCompleted(Vector3 goalPos)
	{
		if (shouldDropOnLevelFinish)
		{
			Random rnd = new Random();
			
			Vector3 pos = Translation;
			float distSqr = pos.DistanceSquaredTo(goalPos);
			distSqr = rnd.Next(0, 6) * distSqr;
			
			shouldKill = (float)Math.Max(0.0000001, 100 * Math.Pow(1/1.003, distSqr));
		}
	}
}
