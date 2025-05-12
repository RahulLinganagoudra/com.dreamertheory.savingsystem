using UnityEditor;
using UnityEngine;
using SavingSystem;

[CustomEditor(typeof(EntitySaver))]
public class EntitySaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EntitySaver entitySaver = (EntitySaver)target;

        if (GUILayout.Button("Generate ID"))
        {
            // Call the private method GenerateID via reflection
            var method = typeof(EntitySaver).GetMethod("GenerateID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(entitySaver, null);

            // Mark the object as dirty so the change is saved
            EditorUtility.SetDirty(entitySaver);
        }
    }
}
