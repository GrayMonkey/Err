using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(ContextButton))]
public class ContextButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {

        ContextButton component = (ContextButton)target;

        EditorUtility.SetDirty(target);
        base.OnInspectorGUI();

        component.helpButton = (GameObject)EditorGUILayout.ObjectField("Help Panel", component.helpButton, typeof(GameObject), true);
        component.keyTitle = EditorGUILayout.TextField("Help Title", component.keyTitle);
        component.keyDetails = EditorGUILayout.TextField("Help Details", component.keyDetails);
    }
}
