using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementValidator
{
    public static bool CheckIfPositionsAreOccupied(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupied(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckIfPositionsAreFree(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceFree(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckIfPositionsAreValid(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceValid(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfNotCrossingMultiCellObject(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupiedByMultitileObject(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement))
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfNotCrossingEdgeObject(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupiedByEdgeObject(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement))
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfPositionsAreNearWall(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        int rotationEulerY = Mathf.RoundToInt(selectedPositionsRotation[0].eulerAngles.y);
        //Comprobar si no hay paredes donde queremos colocar el objeto
        
        HashSet<Edge> edges = new();
        foreach (Vector3Int pos in selectedPositions)
        {
            //Los valores son los mismos para cualquier PlacementGridData

            List<Vector3Int> cellsToOccupy = placementData.GetCellPositions(pos, objectSize, rotationEulerY);
            foreach (var cellPosition in cellsToOccupy)
            {
                //Algoritmo que obtiene todas las aristas que atraviesa el objeto colocado (posibles muros).
                Vector3Int offset = cellPosition - pos;
                if (rotationEulerY == 0)
                {
                    //Checar si es valido
                    if (offset.z == objectSize.y - 1 && placementData.IsCellAt(cellPosition + Vector3Int.forward))
                    {
                        //Necesitamos el borde para la celda de arriba.
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition + Vector3Int.forward, Vector2Int.one, 0));
                    }
                }
                else if (rotationEulerY == 90 && placementData.IsCellAt(cellPosition + Vector3Int.right))
                {
                    if (offset.x == objectSize.x - 1)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition + Vector3Int.right, Vector2Int.one, 270));
                    }
                }
                else if (rotationEulerY == 180)
                {
                    if (offset.z == 0)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition, Vector2Int.one, 0));
                    }
                }
                else
                {
                    if (offset.x == 0)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition, Vector2Int.one, 270));
                    }
                }
            }
        }
        // Se debería comprobar si la pared está cerca de todos los bordes del objeto (el borde depende de la rotación,
        // pero por defecto, con rotación 0, corresponde a la dirección superior o frontal).
        foreach (var edgePos in edges)
        {
            if (placementData.IsEdgeObjectAt(edgePos) == false)
            {
                return false;
            }
        }
        return true;
    }
}
