using UnityEngine;


/// Clase auxiliar que conecta al componente grid y componente grid shader y permite que otros scripts puedan accesar a los datos de la grid 

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Renderer gridRenderer;

    private Vector3 gridCellSize;
    private Vector3 halfGridCellSize;

    [SerializeField, Tooltip("Cuántas celdas deben mostrarse en el plano de la cuadrícula. Modifica este valor para aumentar o reducir el tamaño de las celdas. El valor mínimo es 0.")]
    private Vector2Int gridSizeInCells = new Vector2Int(10, 10);

    //  propiedad que regresa el tamaño de la grid en sus celdas
    public Vector2Int GridSize => gridSizeInCells;

    [SerializeField]
    private string cellSizeParameter = "_GridSize", gridDimensionsParameter = "_DefaultScale";

    [SerializeField, Tooltip("Default Unity 3d Plane esta hecho por 10x10 quads. Es por esto que el valor es 10 y el min es 0")]
    private Vector2Int gridPlaneSize = new Vector2Int(10, 10); // previo defaultSize

    private void Start()
    {
        if (gridPlaneSize.x <= 0 || gridPlaneSize.y <= 0 || gridSizeInCells.x <= 0 || gridSizeInCells.y <= 0)
            Debug.LogError("GridSizeInCells o GridPlaneSize tiene x o y <= 0 lo cual no esta permitido");

        // Calcula el tamaño de la celda en función del tamaño del plano y del número de celdas deseado
        gridCellSize = new Vector3(
            gridPlaneSize.x / (float)gridSizeInCells.x,
            0.5f, // Valor default Y para la altura de la celda,esto solo funciona para una cuadrícula 2D
            gridPlaneSize.y / (float)gridSizeInCells.y
        );

        grid.cellSize = gridCellSize;
        halfGridCellSize = gridCellSize / 2f;

        
        UpdateShaderParameters();
    }
    private void UpdateShaderParameters()
    {
        // Pasa el tamaño de celda al shader (para dibujar las líneas de la cuadrícula)
        gridRenderer.material.SetVector(cellSizeParameter, new Vector2(1f / gridCellSize.x, 1f / gridCellSize.z));

        // Pasa el tamaño del grid plane al shader (debería ser igual al gridPlaneSize)
        gridRenderer.material.SetVector(gridDimensionsParameter, new Vector2(gridPlaneSize.x, gridPlaneSize.y));

    }

    public Vector3Int GetCellPosition(Vector3 worldPosition, PlacementType placementType)
    {
        // Offset del world position al centro del plano
        // El plano va de -5 to 5, pero las celdas de la grid empiezan en 0,0
        worldPosition.x += gridPlaneSize.x / 2f;
        worldPosition.z += gridPlaneSize.y / 2f;

        if (placementType.IsEdgePlacement())
            worldPosition += halfGridCellSize;
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GetWorldPosition(Vector3Int cellPosition)
    {
        Vector3 worldPos = grid.CellToWorld(cellPosition);
        // Offset de regreso al centro de las coordenadas
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


public enum PlacementType
{
    None,
    Floor,
    Wall,
    InWalls,
    NearWallObject,
    FreePlacedObject
}


/// Debido a las limitaciones de los *enums*, el resultado es que se requieren métodos de extensión, ya que no es sencillo añadir más datos a un *enum*.
/// De esta forma, puedo acceder a la información adicional de manera fiable sin tener que revisar
/// cada sentencia `if` o `switch` donde haya utilizado dicho *enum*.


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