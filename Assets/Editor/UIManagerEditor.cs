using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add all UIElements in Scene"))
        {
            UIElement[] elm = Resources.FindObjectsOfTypeAll<UIElement>();
            UIManager sceneInstance = FindObjectOfType<UIManager>();
            sceneInstance.UIElements.Clear();
            foreach(UIElement e in elm)
            {
                sceneInstance.UIElements.Add(e);
            }
        }

        if (GUILayout.Button("Print Dictionary"))
        {
            UIManager sceneInstance = FindObjectOfType<UIManager>();
            sceneInstance._UIElements.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Debug.Log);

        }
    }
}
