using System.Runtime.InteropServices;
using Godot;
using System;

public partial class GridManager : TileMap
{
    private Viewport _vp;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_vp = GetViewport();
	}


        public bool CanPlaceGobbo(Vector2 position)
    {
        var cell = LocalToMap(ToLocal(position));

        var terrainCell = this.GetCellTileData(0, cell);
        if (terrainCell != null && terrainCell.GetNavigationPolygon(0) == null)
        {
            var gobbo = this.GetCellTileData(1, cell);
            if (gobbo == null)
            {
               return true;
            }
        }
        return false;
    }

    public bool PlaceGobbo(Vector2 position)
    {
        var cell = LocalToMap(ToLocal(position));

        var terrainCell = this.GetCellTileData(0, cell);
        if (terrainCell != null && terrainCell.GetNavigationPolygon(0) == null)
        {
            var gobbo = this.GetCellTileData(1, cell);
            if (gobbo == null)
            {
                SetCell(2, cell, 1, Vector2I.Zero, 1);
            }
        }
        return false;
    }

}
