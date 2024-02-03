#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CreateFolders : EditorWindow
    {
        private static string s_projectName;

        [MenuItem("Folders", menuItem = "Folder Manager/Create Default Folders")]
        private static void SetUpFolders()
        {
            var window = CreateInstance<CreateFolders>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
            window.ShowPopup();
        }

        private static void CreateAllFolders()
        {
            // Main folders
            var folders = new List<string>
            {
                "Art",
                "Audio",
                "Code",
                "Docs",
                "Level",
                "ThirdParty"
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists("Assets/" + folder))
                    Directory.CreateDirectory("Assets/" + s_projectName + "/" + folder);
            }

            // Subfolders
            var artFolders = new List<string>
            {
                "Materials",
                "Models",
                "Textures"
            };

            var audioFolders = new List<string>
            {
                "Music",
                "Sound",
            };

            var codeFolders = new List<string>
            {
                "Scripts",
                "Shaders",
            };

            var levelFolders = new List<string>
            {
                "Prefabs",
                "Scenes",
                "UI",
            };
            
            foreach (string subfolder in artFolders)
            {
                if (!Directory.Exists("Assets/" + s_projectName + "/Art/" + subfolder))
                    Directory.CreateDirectory("Assets/" + s_projectName + "/Art/" + subfolder);
            }
            
            foreach (string subfolder in audioFolders)
            {
                if (!Directory.Exists("Assets/" + s_projectName + "/Audio/" + subfolder))
                    Directory.CreateDirectory("Assets/" + s_projectName + "/Audio/" + subfolder);
            }

            foreach (string subfolder in codeFolders)
            {
                if (!Directory.Exists("Assets/" + s_projectName + "/Code/" + subfolder))
                    Directory.CreateDirectory("Assets/" + s_projectName + "/Code/" + subfolder);
            }
            
            foreach (string subfolder in levelFolders)
            {
                if (!Directory.Exists("Assets/" + s_projectName + "/Level/" + subfolder))
                    Directory.CreateDirectory("Assets/" + s_projectName + "/Level/" + subfolder);
            }
            
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Insert the Project name as the root folder");
            
            GUILayout.Space(10);
            Repaint();
            
            s_projectName = EditorGUILayout.TextField("Project Name: ", s_projectName);
            
            Repaint();
            GUILayout.Space(30);

            if (GUILayout.Button("Generate Folders..."))
            {
                CreateAllFolders();
                Close();
                return;
            }
            
            GUILayout.Space(5);
            Repaint();
            
            if (GUILayout.Button("Close"))
                Close();
        }
    }
}
#endif