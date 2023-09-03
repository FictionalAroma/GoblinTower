using Godot;
using System;

public partial class Spawner : Node2D
{
	[Export] PackedScene thingToSpawn;
    private TileMap _mapToUse;
	private Vector2 _cachedDestination;
	private LevelManager _lm;
    public override void _Ready()
	{
		_mapToUse = GetParent<TileMap>();
		_lm = FindParent("LevelManager") as LevelManager;
	}
	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		var filterCellCoords = _mapToUse.GetUsedCellsById(2, 1, alternativeTile: 2);	

		// Now that the navigation map is no longer empty, set the movement target.
		_cachedDestination = _mapToUse.MapToLocal(filterCellCoords[0]);
	}

	public void OnSpawnTimer()
	{
		var newStab = thingToSpawn.Instantiate<Enemy>();
		AddSibling(newStab);
		newStab.GlobalPosition = this.GlobalPosition;
		newStab.Initialise(_cachedDestination, _lm);
	}

}
