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


    public override void _Process(float delta)
    {
        if (isMouseOn)
        {
            if (Input.IsActionJustPressed("Primary_Click"))
            {
                GD.PrintS("Clicked!");
                GetNode<Goal>("../../Goal").playAnimation();
                fade = true;

            }
        }

        if (fade)
        {
            var logo = GetNode<Sprite>("../../CanvasLayer/menu/Center/Logo");
            var newLogoColor = logo.Modulate;
            newLogoColor.a = newLogoColor.a - (delta / 1.5F);
            logo.Modulate = newLogoColor;

            var text = GetNode<Label>("../../CanvasLayer/menu/Center/Label");
            var newTextColor = text.Modulate;
            newTextColor.a = newTextColor.a - (delta * 1.2F);
            text.Modulate = newTextColor;
        }
    }

    private void _on_Area_mouse_entered()
    {
        isMouseOn = true;
    }

    private void _on_Area_mouse_exited()
    {
        isMouseOn = false;
    }
}







