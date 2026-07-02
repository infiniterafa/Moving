using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Permite mostrar la preview de los objetos que se planea colocar.
/// Muestra los objetos de forma semitransparente y los tiñe de rojo si la colocación no está permitida.

public class PlacementPreview : MonoBehaviour
{
    [SerializeField]
    private Material transparentMaterial;
    private Material transparentMaterialInstance;

    private List<GameObject> previewObjects = new();
    private GameObject cursorObject = null;
    private GameObject previewTemplate;
    private Renderer previewObjectRenderer;

    [SerializeField]
    private float yOffset = 0.05f;

    [SerializeField]
    private Color destroyColor;

    Color defautlColor;

    private void Start()
    { 
        defautlColor = transparentMaterial.color;
        transparentMaterialInstance = new Material(transparentMaterial);
    }


    /// Actualiza las posiciones de los prefabs de vista previa.

    /// <param name="positions"></param>
    /// <param name="rotation"></param>
    public void MovePreview(List<Vector3> positions, List<Quaternion> rotation)
    {
        if(previewObjects.Count > positions.Count)
        {
            for (int i = previewObjects.Count - 1; i >= positions.Count; i--)
            {
                Destroy(previewObjects[i]);
                previewObjects.RemoveAt(i);
            }
        }
        for (int i = 0; i < positions.Count; i++)
        {
            if(previewObjects.Count == i)
            {
                previewObjects.Add(Instantiate(previewTemplate));
            }
            Vector3 pos = positions[i];
            pos.y += yOffset;
            previewObjects[i].transform.position = pos;
            previewObjects[i].transform.localScale = Vector3.one*1.02f;
            if (previewObjects[i].transform.childCount != 0)
                previewObjects[i].transform.GetChild(0).rotation = rotation[i];
            else
                previewObjects[i].transform.rotation = rotation[i];
        }
    }


    /// Desactiva los preview objects

    public void StopShowingPreview()
    {
        previewObjectRenderer = null;
        transparentMaterialInstance.color = defautlColor;
        foreach (var item in previewObjects)
        {
            Destroy(item);
        }
        if(cursorObject != null)
            Destroy(cursorObject);
        previewTemplate = null;
        previewObjects.Clear();
    }


    /// Visualiza los preview objects.
 
    /// <param name="placedObject"></param>
    /// <param name="keepMaterial">For removing objects state we change the preview to a red transparent square and set this to True</param>
    public void StartShowingPreview(GameObject placedObject, bool keepMaterial = false)
    {
        if (keepMaterial)
        {
            previewTemplate = Instantiate(placedObject, transform);
        }
        else
        {
            previewTemplate = CreatePreviewObject(placedObject);
        }
        
        previewObjects.Clear();
        previewObjects.Add(previewTemplate);
    }


    /// Crea un preview object a partir del prefab del objeto y cambia su material por uno transparente.

    /// <param name="placedObject"></param>
    /// <returns></returns>
    private GameObject CreatePreviewObject(GameObject placedObject)
    {
        GameObject preview = Instantiate(placedObject, transform);
        preview.transform.position = Vector3.zero;
        previewObjectRenderer = preview.GetComponent<Renderer>();
        if(previewObjectRenderer == null)
            foreach (var renderer in preview.GetComponentsInChildren<Renderer>())
            {
                PreparePreviewPrefab(renderer);
            }
        else
            PreparePreviewPrefab(previewObjectRenderer);
        return preview;
    }


    /// Asigna un material transparente a los objetos de previsualización
    /// para que podamos darles el color que queramos (blanco y rojo en este caso)

    /// <param name="renderer"></param>
    private void PreparePreviewPrefab(Renderer renderer)
    {
        previewObjectRenderer = renderer;
        Material[] newMaterialArray = new Material[previewObjectRenderer.materials.Length];
        for (int i = 0; i < newMaterialArray.Length; i++)
        {
            newMaterialArray[i] = transparentMaterialInstance;
        }
        previewObjectRenderer.materials = newMaterialArray;
    }


    /// Cambia el material del preview a blanco (correcto) a rojo (equivocando)

    /// <param name="val"></param>
    public void ShowPlacementFeedback(bool val)
    {
        if (val)
            PlacementFeedbackPositive();
        else
            PlacementFeedbackNegative();
    }

    private void PlacementFeedbackPositive()
    {
        transparentMaterialInstance.color = defautlColor;
    }

    private void PlacementFeedbackNegative()
    {
        Color c = Color.red;
        c.a = defautlColor.a;
        transparentMaterialInstance.color = c;
        //previewObject.GetComponent<Renderer>().material.color = c;
    }
}
