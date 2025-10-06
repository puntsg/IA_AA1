#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(MovingType))]
public class MovingTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var movementProp = property.FindPropertyRelative("movement");
        var weightProp = property.FindPropertyRelative("weight"); // si existe
        var behaviourProp = property.FindPropertyRelative("movementBehaviour");

        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;
        var r = new Rect(position.x, position.y, position.width, line);

        // Título del bloque
        label = EditorGUI.BeginProperty(r, label, property);
        EditorGUI.LabelField(r, label);
        EditorGUI.EndProperty();
        r.y += line + space;

        // Enum
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(r, movementProp);
        bool enumChanged = EditorGUI.EndChangeCheck();
        r.y += line + space;

        // Weight (si lo tienes)
        if (weightProp != null)
        {
            EditorGUI.PropertyField(r, weightProp);
            r.y += line + space;
        }

        // Si ha cambiado el enum, asigna el tipo correcto
        if (enumChanged)
        {
            property.serializedObject.ApplyModifiedProperties(); // actualiza enum antes de tocar managedReferenceValue
            var current = (MovingType.EmovementBehaviour)movementProp.enumValueIndex;
            behaviourProp.managedReferenceValue = CreateBehaviourInstance(current);
        }

        // Dibuja el bloque de propiedades del comportamiento concreto
        if (behaviourProp != null)
        {
            EditorGUI.indentLevel++;
            var h = EditorGUI.GetPropertyHeight(behaviourProp, true);
            var rr = new Rect(r.x, r.y, r.width, h);
            EditorGUI.PropertyField(rr, behaviourProp, true);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3; // título + enum + weight (aprox.)
        var behaviourProp = property.FindPropertyRelative("movementBehaviour");
        if (behaviourProp != null)
            h += EditorGUI.GetPropertyHeight(behaviourProp, true);
        return h;
    }

    private object CreateBehaviourInstance(MovingType.EmovementBehaviour em)
    {
        switch (em)
        {
            case MovingType.EmovementBehaviour.SEEK: return new Seek();
            case MovingType.EmovementBehaviour.ARRIVE: return new Arrive();
            case MovingType.EmovementBehaviour.PURSUE: return new Pursue();
            case MovingType.EmovementBehaviour.WANDER: return new Wander();
            default: return null;
        }
    }
}
#endif

