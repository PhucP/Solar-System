using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConditionalShowAttribute : PropertyAttribute
{
    public string conditionalSourceField = "";
    public bool showInInspector = true;

    public ConditionalShowAttribute(string conditionalSourceField, bool showInInspector)
    {
        this.conditionalSourceField = conditionalSourceField;
        this.showInInspector = showInInspector;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalShowAttribute))]
public class ConditionalShowPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condHAtt = (ConditionalShowAttribute)attribute;
        bool enabled = GetConditionAttributeResult(condHAtt, property);

        if (enabled == condHAtt.showInInspector)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalShowAttribute condAtt = (ConditionalShowAttribute)attribute;
        bool enabled = GetConditionAttributeResult(condAtt, property);

        if (enabled == condAtt.showInInspector)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    bool GetConditionAttributeResult(ConditionalShowAttribute condAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath;
        string conditionalPath = propertyPath.Replace(property.name, condAtt.conditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionalPath);
        if (sourcePropertyValue != null)
        {
            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    enabled = sourcePropertyValue.boolValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    enabled = sourcePropertyValue.objectReferenceValue != null;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condAtt.conditionalSourceField);
        }

        return enabled;
    }
}
#endif