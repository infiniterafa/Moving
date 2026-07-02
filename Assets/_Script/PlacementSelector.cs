using System;
using UnityEngine;


/// Método de alto nivel que conecta los datos de selección ("Selection Data") con una selectionstrategy,
/// manteniendo al mismo tiempo las otras estrategias desconectadas del PlacementManager.

public class PlacementSelector
{
    SelectionData selectionData;

    SelectionStrategy selectionStrategy;
    public SelectionStrategy CurrentSelectionStrategy => selectionStrategy;
    public event Action<SelectionResult> OnSelectionChanged, OnSelectionFinished;

    public PlacementSelector(SelectionStrategy placementStrategy, SelectionData selectionData)
    {
        this.selectionStrategy = placementStrategy;
        this.selectionData = selectionData;

    }


    /// Envía un mensaje de inicio a SelectionStrategy ,
    ///   borrando también los datos de selección anteriores.
     
    /// <param name="mousePosition"></param>
    public void HandleSelectionStarted(Vector3 mousePosition)
    {
       
        selectionData.Clear();
        selectionStrategy.StartSelection(mousePosition, selectionData);
        SelectionResult data = selectionData.GetData();
        if (data.selectedGridPositions.Count > 0)
            OnSelectionChanged?.Invoke(data);
    }


    /// Asegura que los datos de selección finales se envíen a otras clases (PlacementManager).
    ///  restablece los datos de selección anteriores.

    public void HandleSelectionFinished()
    {
        SelectionResult data = selectionData.GetData();
        //Wall placement no coloca nada en la gridpositions list.
        // evita que se cree el proceso de placement si sus datos no son correctos.
        if (data.selectedGridPositions.Count > 0)
            OnSelectionFinished?.Invoke(data);
        ResetSelection();
    }


    /// Envía información de movimiento a la SelectonStrategy, al mismo tiempo que
    /// actualiza al PlacementManager sobre los resultados.

    /// <param name="mousePosition"></param>
    public void HandleMouseMovement(Vector3 mousePosition)
    {
        //Si no hemos modificado la selección, no enviamos ninguna actualización a otras clases.
        if (selectionStrategy.ModifySelection(mousePosition, selectionData))
        {
            SelectionResult data = selectionData.GetData();
            OnSelectionChanged?.Invoke(data);
        }

    }

    //Maneja la rotación cuando el player clickea , o .
    public void HandleRotation(Quaternion rotationAmount)
    {
        selectionData.Rotation *= rotationAmount;
        selectionData.Rotation = selectionStrategy.HandleRotation(selectionData.Rotation, selectionData);
        Refresh();
        Debug.Log(selectionData.Rotation.eulerAngles);
    }


    /// Permite actualizar la selección en caso de que la ubicación haya pasado a ser válida o inválida.

    internal void Refresh()
    {
        if (selectionData.GetSelectedGridPositions().Count <= 0)
            return;
        selectionStrategy.RefreshSelection(selectionData);
        OnSelectionChanged?.Invoke(selectionData.GetData());
    }


    /// Borra la selección e informa a la estrategia de que finalice.

    public void ResetSelection()
    {
        selectionData.Clear();
        selectionStrategy.FinishSelection(selectionData);
    }
}


