using Godot;
using System;

public partial class LevelManager : Node2D
{
	Camera2D _cam;
    private BuildPlacer _placer;
   	private UIManager _uiManager;
    private GridManager _levelGrid;
    private bool _buildMode;


	[Export] private int _startingLives;
	[Export] private int _startingShineys;

    private int currentShineys;
    private int currentLives;
    public int CurrentLives { get => currentLives; private set {currentLives = value; _uiManager.UpdateLives(currentLives);} }
    public int CurrentShineys { 
		get => currentShineys; 
		private set {currentShineys = value; _uiManager.UpdateShineys(currentShineys);}}

    public bool IsPaused {get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_placer = GetNode<BuildPlacer>("BuildPlacer");
		
		_uiManager = GetNode<UIManager>("../UIManager");
		_levelGrid = GetNode<GridManager>("LevelGrid");
		_cam = GetViewport().GetCamera2D();
		LevelSetup();
	}
	public void LevelSetup()
	{
		_buildMode = false;
		CurrentLives = _startingLives;

	}
    public void OnEnemyDead(Enemy enemy)
	{
		GD.Print("Enemy Dead");
		enemy.QueueFree();

		CurrentShineys += 5;
		_uiManager.UpdateShineys(CurrentShineys);
	}

	public void OnEnemyReachedGate(Enemy enemy)
	{
		GD.Print("Enemy Reached Gate");
		enemy.QueueFree();

		CurrentLives--;
		_uiManager.UpdateLives(CurrentLives);
	}

	public void ToggleBuildMode()
	{
		_buildMode = !_buildMode;
		_placer.SetActive(_buildMode);
	}

	public void TogglePause()
	{
		this.GetTree().Paused = !this.GetTree().Paused;
	}

    public bool IsPositionValid(Vector2 globalPosition) => _levelGrid.CanPlaceGobbo(globalPosition);


}
