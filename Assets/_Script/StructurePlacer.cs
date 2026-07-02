using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;


/// Script y objeto responsable de colocar GameObjects en el mapa y almacenar referencias a ellos.

public class StructurePlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> placedObjects = new List<GameObject>();

    [SerializeField]
    private float scalingDelay = 0.3f, destroyDelay = 0.1f;
    private int GetFreeIndex()
    {
        int indexOfNull = placedObjects.IndexOf(null);
        if (indexOfNull > -1)
        {
            return indexOfNull;
        }
        placedObjects.Add(null);
        return placedObjects.Count - 1;
    }

    public Quaternion GetObjectsRotation(int index)
    {
        Quaternion rotationToReturn = Quaternion.identity;
        if (index >= 0 && index < placedObjects.Count && placedObjects[index] != null)
            rotationToReturn = placedObjects[index].transform.GetChild(0).rotation;
        return rotationToReturn;
    }



    /// Cuando colocamos una estructura en el mapa, devolvemos su index (index de la lista de GO anterior)
    /// para que, si necesitamos eliminarla, usemos el index como referencia para eliminar el GO correcto.

    /// <param name="objectToPlace"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="yHeight"></param>
    /// <returns></returns>
    public int PlaceStructure(GameObject objectToPlace, Vector3 position, Quaternion rotation, float yHeight, bool animate = true)
    {
        int freeIndex = GetFreeIndex();
        GameObject newObject = Instantiate(objectToPlace);
        newObject.transform.SetParent(transform);
        Vector3 placementPosition = new Vector3(position.x, yHeight, position.z);
        newObject.transform.position = placementPosition;

        newObject.transform.GetChild(0).rotation = rotation;
        
        placedObjects[freeIndex] = newObject;
        if(animate)
        {
            newObject.transform.localScale = new Vector3(1, 0, 1);
            newObject.transform.DOScaleY(1, scalingDelay);
        }
        return freeIndex;
    }

    public void RemoveObjectAt(int index)
    {
        GameObject newObject = placedObjects[index];
        newObject.transform.DOKill();
        newObject.transform.DOScaleY(0, destroyDelay).OnComplete(()=> Destroy(newObject));
        placedObjects[index] = null;

    }

    private void OnDisable()
    {
        foreach (GameObject obj in placedObjects) 
        {
            //En lugar de eliminar los valores NULL de nuestra lista, la poblamos con nuevas estructuras.

            if (obj != null)
                obj.transform.DOComplete();
        }
    }
}
