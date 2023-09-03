using Godot;
public partial class Enemy : Node2D
{
	[Export]private float moveSpeed = 10.0f;
	[Export] private Node2D aimAtPosition;

	public Node2D GetAimAtPosition => aimAtPosition;

	private NavigationAgent2D _pather;

	[Export] private int _maxHp = 3;
	private int _currentHP;

	[Signal] public delegate void DestinationReachedEventHandler(Enemy enemy);
	[Signal] public delegate void HPZeroEventHandler(Enemy enemy);

    private Vector2 _destination;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_pather = GetNodeOrNull<NavigationAgent2D>("Pather");
		_currentHP = _maxHp;
	}

	public void Initialise(Vector2 destinationCoords, LevelManager levelManager)
	{
		_destination = destinationCoords;

		DestinationReached += levelManager.OnEnemyReachedGate;
		HPZero += levelManager.OnEnemyDead;

	}


	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		_pather.MaxSpeed = moveSpeed;

		_pather.TargetPosition = _destination;
	}
	
    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);

		if(_pather.IsNavigationFinished())
		{ 
			return;
		}
		

		Vector2 currentAgentPosition = GlobalPosition;
        Vector2 nextPathPosition = _pather.GetNextPathPosition();

        Vector2 newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
        newVelocity *= (float)(moveSpeed * delta);
		GlobalPosition += newVelocity;
		 
    }

	public void OnNavReached()
	{
		EmitSignal(SignalName.DestinationReached, this);
	}

	private void OnAreaEntered(Area2D collision)
	{
		var projectile = collision.GetParent<Arrow>();
		TakeDamage(projectile.Damage);
	}

    private void TakeDamage(int damage)
    {
        _currentHP -= damage;
		if(_currentHP <= 0)
		{
			EmitSignal(SignalName.HPZero, this);
		}
    }

	public float DistanceToDestination =>_pather.DistanceToTarget();

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
