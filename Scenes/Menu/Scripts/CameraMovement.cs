using Godot;
using System;

public class CameraMovement : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Vector3 initialRotation = new Vector3();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        initialRotation = this.RotationDegrees;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 newMousePos = GetViewport().GetMousePosition();
        Vector2 screenSize = OS.WindowSize;
        Vector2 offsetFromCenter = ((screenSize / 2) - newMousePos) / screenSize.x * 1200;

        var rotationAmount = 0.01;
        var targetRotation = new Vector3(initialRotation);
        targetRotation.y += (float)(offsetFromCenter.x * rotationAmount);
        targetRotation.x += (float)(offsetFromCenter.y * rotationAmount);

        this.RotationDegrees = this.RotationDegrees.LinearInterpolate(targetRotation, 1F * delta);
    }
}
