using System;
using UnityEngine;


/// Nos ayuda a hacer que el código sea más eficiente al no actualizar la
/// posicion si es la misma que la anterior

public class LastDetectedPositon
{
    public Vector3Int? lastPosition;

    public Vector3Int GetPosition()
    {
        if (lastPosition.HasValue)
            return lastPosition.Value;
        throw new Exception("LastDetectedPositon position no fue actualizada. " +
            "checa si estas llamando a  TryUpdatingPositon en la instancia del LastDetectedPosition script.");
    }
    public bool TryUpdatingPositon(Vector3Int tempPos)
    {
        if (lastPosition.HasValue && lastPosition == tempPos)
            return false;

        lastPosition = tempPos;
        return true;
    }

    public void Reset() => lastPosition = null;
}
