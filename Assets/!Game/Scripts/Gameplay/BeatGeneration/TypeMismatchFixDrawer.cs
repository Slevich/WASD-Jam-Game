#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeMismatchFixAttribute))]
public class TypeMismatchFixDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, typeof(Object), true);

        EditorGUI.EndProperty();
    }
}
#endif