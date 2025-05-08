using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class RenderLayerAssigner : MonoBehaviour
{
    public string  layerName;    // The layer to assign to children
    [SerializeField]
    private List<Transform> parents = new List<Transform>();

    public void AssignLayers()
    {
        foreach (var parent in parents)
        {
            if (parent == null)
            {
                Debug.LogWarning("no parent is assigned in one of the elements");
                continue;
            }

            int layer =  LayerMask.NameToLayer( layerName);
            if (layer == -1)
            {
                Debug.LogError($"Layer '{ layerName}' does not exist!");
                continue;
            }

            // Use Transform to loop through children
            foreach (Transform child in parent)
            {
                child.gameObject.layer = layer;
                
            }
        }

        Debug.Log("Layer assignment complete!");
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(RenderLayerAssigner))]
public class LayerAssignerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RenderLayerAssigner script = (RenderLayerAssigner)target;
        if (GUILayout.Button("Assign Layers"))
        {
            script.AssignLayers();
        }
    }
}
#endif
