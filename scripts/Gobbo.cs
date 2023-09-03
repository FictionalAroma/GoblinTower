using System;
using System.Linq;
using Godot;

public partial class Gobbo : Node2D
{
	private Enemy _target = null;

	private Timer _shotTimer = null;
    private Area2D _area;
    [Export]
	private float _fireTimer = 5.0f;

	[Export] int _damage = 1;

	[Export]
	private PackedScene _toShoot = null;
    private bool _canFire;

	private bool _upgrade;

	[Signal]
	public delegate void FindTargetEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_shotTimer = GetNode<Timer>("Timer");
		_area = GetNode<Area2D>("TowerRange");
		_shotTimer.WaitTime = _fireTimer;

		FindTarget += UpdateTarget;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_target != null)
		{
			if(_canFire)
			{
				Shoot();
			}
		}
	}

    public override void _Draw()
    {
        base._Draw();

		if(_target != null)
		{	
			DrawLine(this.Position, _target.Position, Color.Color8(255,0,0), 10f);
		}
    }

	public void OnAreaEntered(Area2D collision)
	{
		GD.Print("Collision Happed");
		QueueUpdateTarget();	
	}

	public void OnAreaExited(Area2D collision)
	{
		GD.Print("Collision Un-happened");
		QueueUpdateTarget();	
	}

    private void QueueUpdateTarget()
    {
        EmitSignal(SignalName.FindTarget);
    }

    public void OnShotTimer()
	{
		if(_target == null)
		{ 
			_shotTimer.Stop();
		}
		_canFire = true;
	}

	public void Shoot()
	{
		var shot = _toShoot.Instantiate<Arrow>();
		AddSibling(shot);

		shot.GlobalPosition = this.GlobalPosition;
		shot.Fire(20, _target.GetAimAtPosition, _damage);
		_canFire = false;
	}

	public async void UpdateTarget()
	{

		_target = null;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		_target = PickEnemy();
		QueueRedraw();

		if(_target != null)
		{
			if(_shotTimer.IsStopped())
			{
				_shotTimer.Start();
			}
		}
	}

	public Enemy PickEnemy()
	{
		var enemiesInRange = _area.GetOverlappingAreas().Select(area => area.GetParent<Enemy>());

		if(enemiesInRange.Count() <= 1)
		{
			return enemiesInRange.FirstOrDefault();
		}
		Enemy closetToGoal = enemiesInRange.FirstOrDefault();
		float minDistance = closetToGoal.DistanceToDestination;
		foreach (var item in enemiesInRange)
		{
			var thisDistance = item.DistanceToDestination;

			if(thisDistance < minDistance )
			{
				closetToGoal = item;
				minDistance = thisDistance;
			}
		}
		return closetToGoal;
	}
}
