using UnityEngine;
using UnityEngine.Events;



/// Controlador para evitar que el placement system(cualquiera que sea) llame directamente a los elementos de la interfaz de usuario (botones).
/// Reutiliza la interfaz sin tener que importar el PlacementManager ni todas sus dependencias.

public class UIPlacementController : MonoBehaviour
{
    public UnityEvent<int> OnObjectSelected;
    public UnityEvent OnUndoRequested, OnMoveRequest, OnResetMovementButton, OnCancelPlacement, OnMovementStateEntered;

    public void SelectObjectWithIndex(int index)
        => OnObjectSelected?.Invoke(index); 

    public void RequestUndo() 
        => OnUndoRequested?.Invoke();

    public void MoveRequest()
        => OnMoveRequest?.Invoke();

    public void ResetMovementButton()
        => OnResetMovementButton?.Invoke();

    public void CancelPlacementRequested()
        => OnCancelPlacement?.Invoke();

    public void EnterMovementState()
        => OnMovementStateEntered?.Invoke();
}
