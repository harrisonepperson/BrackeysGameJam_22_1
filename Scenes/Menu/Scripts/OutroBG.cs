using Godot;
using System;

public class OutroBG : Panel
{

    [Export]
    private PackedScene MenuScene;
    private ulong time_start = 0;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var newColor = this.Modulate;
        newColor.a = 0;
        this.Modulate = newColor;
        time_start = OS.GetUnixTime();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var newColor = this.Modulate;
        newColor.a = Mathf.Min(1, newColor.a + (delta / 10F));
        this.Modulate = newColor;


        var time_elapsed = OS.GetUnixTime() - time_start;

        if (time_elapsed > 10)
        {
            GetTree().ChangeSceneTo(MenuScene);
        }
    }
}
