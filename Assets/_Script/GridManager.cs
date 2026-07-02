using UnityEngine;

/// <summary>
/// Helper class that connects the Grid component and Grid shader and allows other scripts to access the data from the grid
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Renderer gridRenderer;

    //[SerializeField]
    private Vector3 gridCellSize;
    private Vector3 halfGridCellSize;

    [SerializeField, Tooltip("How many cell should be shown on the grid plane. To make the cells larger or smaller change this. Min value is 0.")]
    private Vector2Int gridSizeInCells = new Vector2Int(10, 10);

    // Public property that returns the grid size in cells
    public Vector2Int GridSize => gridSizeInCells;

    [SerializeField]
    private string cellSizeParameter = "_GridSize", gridDimensionsParameter = "_DefaultScale";

    [SerializeField, Tooltip("Default Unity 3d Plane is made out of 10x10 quads. That is why default value is 10. Min value is 0.")]
    private Vector2Int gridPlaneSize = new Vector2Int(10, 10); // previously defaultSize

    private void Start()
    {
        if (gridPlaneSize.x <= 0 || gridPlaneSize.y <= 0 || gridSizeInCells.x <= 0 || gridSizeInCells.y <= 0)
            Debug.LogError("GridSizeInCells or GridPlaneSize has x or y <= 0 which is not allowed.");

        // Calculate cell size based on plane size and desired number of cells
        gridCellSize = new Vector3(
            gridPlaneSize.x / (float)gridSizeInCells.x,
            0.5f, // Default Y value for cell height since right now this only works for 2D grid
            gridPlaneSize.y / (float)gridSizeInCells.y
        );

        grid.cellSize = gridCellSize;
        halfGridCellSize = gridCellSize / 2f;

        // Update shader parameters
        UpdateShaderParameters();
    }
    private void UpdateShaderParameters()
    {
        // Pass the cell size to the shader(for drawing grid lines)
            gridRenderer.material.SetVector(cellSizeParameter, new Vector2(1f / gridCellSize.x, 1f / gridCellSize.z));

        // Pass the grid plane size to the shader (this should equal gridPlaneSize)
        gridRenderer.material.SetVector(gridDimensionsParameter, new Vector2(gridPlaneSize.x, gridPlaneSize.y));

    }

    public Vector3Int GetCellPosition(Vector3 worldPosition, PlacementType placementType)
    {
        // Offset the world position to account for centered plane
        // The plane goes from -5 to 5, but grid cells start at 0,0
        worldPosition.x += gridPlaneSize.x / 2f;
        worldPosition.z += gridPlaneSize.y / 2f;

        if (placementType.IsEdgePlacement())
            worldPosition += halfGridCellSize;
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GetWorldPosition(Vector3Int cellPosition)
    {
        Vector3 worldPos = grid.CellToWorld(cellPosition);
        // Offset back to centered coordinates
        worldPos.x -= gridPlaneSize.x / 2f;
        worldPos.z -= gridPlaneSize.y / 2f;
        return worldPos;
    }

    public Vector3 GetCenterPositionForCell(Vector3Int cellPosition)
    {
        return GetWorldPosition(cellPosition) + halfGridCellSize;
    }

    public void ToggleGrid(bool value)
    {
        gridRenderer.gameObject.SetActive(value);
    }
}

/// <summary>
/// Placement types. A better idea would be to try to create objects from it ex using ScriptableObjects.
/// Still enum works well for a prototype.
/// </summary>
public enum PlacementType
{
    None,
    Floor,
    Wall,
    InWalls,
    NearWallObject,
    FreePlacedObject
}

/// <summary>
/// Because of the limitation of using enum the end result is that you need extensions methods
/// since you can't easily add more data to an enum. This way I can reliably access the additional data
/// without having to check each if / switch statement where I have used the enum.
/// </summary>
public static class PlacementTypeExtensions
{
    public static bool IsEdgePlacement(this PlacementType placementType)
    => placementType switch
    {
        PlacementType.Wall => true,
        PlacementType.InWalls => true,
        _ => false
    };
    public static bool IsObjectPlacement(this PlacementType placementType)
    => placementType switch
    {
        PlacementType.FreePlacedObject => true,
        PlacementType.NearWallObject => true,
        _ => false
    };
}