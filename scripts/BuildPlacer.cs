
using Godot;
using GoblinTower.scripts.extensions;

public partial class BuildPlacer : Sprite2D
{
	private Viewport _vp;
	private LevelManager _levelManager;

	[Export] private TouchScreenButton confirmButton;
	[Export] private TouchScreenButton cancelButton;
	

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_vp = GetViewport();
		_levelManager = GetParent<LevelManager>();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventScreenTouch inputEventScreenTouch)
		{
			if(inputEventScreenTouch.IsPressed() && inputEventScreenTouch.Index == 0)
			{
				this.GlobalPosition = this.FromScreenToWorld(inputEventScreenTouch.Position);
				if(_levelManager.IsPositionValid(this.GlobalPosition))
				{
					
				}
				_vp.SetInputAsHandled();
			}
		}
		else if(@event is InputEventScreenDrag inputEventScreenDrag)
		{
			if(inputEventScreenDrag.Index == 0)
			{
				this.Position += inputEventScreenDrag.Relative;
				//_vp.SetInputAsHandled();
			}

		}
	}

	public void SetActive(bool enable)
	{
		this.Visible = enable;
		this.ProcessMode = enable ? ProcessModeEnum.Always : ProcessModeEnum.Disabled;
	}
}
