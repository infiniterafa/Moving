
using System;
using System.Numerics;
using UnityEngine;
/// <summary>
/// Stores all the data about the objects that are on our map
/// </summary>
public class GridData
{
    /// <summary>
    /// I have decided to reuse a singel structure.
    /// I bet you could condence it to a bigger data class.
    /// On the other hand this way it is easy to add new placement type
    /// ex OnWallObjects - paintings etc
    /// </summary>
    public PlacementGridData WallPlacementData { private set; get; }
    public PlacementGridData FloorPlacementData { private set; get;}
    public PlacementGridData ObjectPlacementData { private set; get;}
    public PlacementGridData InWallPlacementData { private set; get;}

    public GridData(Vector2Int gridSizeInCells)
    {
        // Since we're now offsetting in GridManager, the grid cells start at (0,0)
        // For a 3x3 grid: cells go from 0 to 2
        // For a 10x10 grid: cells go from 0 to 9
        int maxX = gridSizeInCells.x - 1;
        int maxY = gridSizeInCells.y - 1;

        //Because of how we place walls (we assign them to belong to the cells right or bottom edge)
        //
        //   |_|_|_
        //   |_|_|_ <-- cell placement ends in the middle tile but to place wall we need the +1 in both X and Z axis
        //   |_|_|_
        //
        //We need to access the cell outside the grid visible area to assign them
        //That is why we add +1 to the max bounds of wall placement (not to floor or cell)
        FloorPlacementData = new(0, maxX, 0, maxY);
        WallPlacementData = new(0, maxX + 1, 0, maxY + 1);
        ObjectPlacementData = new(0, maxX, 0, maxY);
        InWallPlacementData = new(0, maxX + 1, 0, maxY + 1);
    }

    public GridSaveData GetSaveData()
    {
        return new()
        {
            floorData = FloorPlacementData.GetSaveData(),
            wallData = WallPlacementData.GetSaveData(),
            objectData = ObjectPlacementData.GetSaveData(),
            inWallData = InWallPlacementData.GetSaveData()
        };
    }
}

[Serializable]
public struct GridSaveData
{
    public PlacementGridSaveData floorData, wallData, objectData, inWallData;
}
