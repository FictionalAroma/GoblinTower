using Godot;
using System;

public partial class CameraControl : Camera2D
{
    [Export]private float _srollSpeed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventScreenDrag screenDrag)
		{
			MoveViewport(screenDrag.Relative);
			//GetViewport().SetInputAsHandled();	
		}
	}

	private void MoveViewport(Vector2 velocity)
    {
		Position += velocity * _srollSpeed;
    }



}
