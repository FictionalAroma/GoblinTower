using Godot;
using System;

public partial class UIManager : Node
{
	// Called when the node enters the scene tree for the first time.
	[Export] Label _livesDisplay;
	[Export] Label _shineysDisplay;

	public void UpdateLives(int number)
	{
		_livesDisplay.Text = number.ToString();
	}

	public void UpdateShineys(int number)
	{
		_shineysDisplay.Text = number.ToString();
	}

	
}
