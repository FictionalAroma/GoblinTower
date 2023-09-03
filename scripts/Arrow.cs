using Godot;
using System;

public partial class Arrow : Node2D
{
    private float _speed;
    private Node2D _target;
    public int Damage {get; private set;}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	public void Fire(float speed, Node2D target, int damage)
	{
		_speed = speed;
		_target = target;
		Damage = damage;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!IsInstanceValid(_target))
		{
			QueueFree();
			return;
		}

		var moveVector = (_target.GlobalPosition - this.GlobalPosition ).Normalized() * _speed;
		this.Position += moveVector;
		this.LookAt(_target.GlobalPosition);
	}

	public void OnAreaEntered(Area2D collision)
	{
		QueueFree();
	}
}
