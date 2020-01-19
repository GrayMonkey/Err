using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(ContextToggle))]
public class ContextToggleEditor : UnityEditor.UI.ToggleEditor
{
    public override void OnInspectorGUI()
    {

        ContextToggle component = (ContextToggle)target;

        EditorUtility.SetDirty(target);
        base.OnInspectorGUI();

        component.helpToggle = (GameObject)EditorGUILayout.ObjectField("Help Panel", component.helpToggle, typeof(GameObject), true);
        component.keyTitle = EditorGUILayout.TextField("Help Title", component.keyTitle);
        component.keyDetails = EditorGUILayout.TextField("Help Details", component.keyDetails);
    }
}
