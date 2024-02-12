using System;
using System.Linq;
using Farm;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(ResourcesGatherer))]
    public class ResourcesGathererEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ResourcesGatherer gatherer = (ResourcesGatherer)target;

            // Use SerializedObject to interact with the private serialized fields
            SerializedProperty toolTypeNameProp = serializedObject.FindProperty("requiredToolTypeName");

            // Draw the default inspector options
            DrawDefaultInspector();

            // Begin a change check here to track changes to serialized properties
            EditorGUI.BeginChangeCheck();

            // Get all subclasses of Tool
            var toolTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(Farm.Tool.Tool).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass)
                .ToList();


            // Convert tool types to a string array for displaying in a dropdown
            string[] toolTypeNames = toolTypes.Select(t => t.Name).ToArray();
            int currentIndex = Array.IndexOf(toolTypeNames, toolTypeNameProp.stringValue);

            // Create a dropdown for tool types
            int selectedIndex = EditorGUILayout.Popup("Required Tool Type", currentIndex, toolTypeNames);

            if (selectedIndex >= 0 && selectedIndex < toolTypeNames.Length)
            {
                toolTypeNameProp.stringValue = toolTypes[selectedIndex].AssemblyQualifiedName; // Use AssemblyQualifiedName if types might come from various assemblies
                serializedObject.ApplyModifiedProperties();
            }

            // Apply changes to all serialized properties
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}