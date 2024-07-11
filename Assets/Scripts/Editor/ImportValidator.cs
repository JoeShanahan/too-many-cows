#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using TooManyCows.DataObjects;

namespace TooManyCows.EditorScripts
{
    [CustomEditor(typeof(LevelLoader))]
    public class ImportValidator : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            LevelLoader myScript = (LevelLoader)target;
            if(GUILayout.Button("Validate Levels"))
            {   
                var hadError = false;
                
                foreach(var txtObj in myScript.levelList)
                {
                    LevelData level = null;

                    try
                    {
                        level = LevelData.CreateFromJSON(txtObj.text);
                    }
                    catch
                    {
                        Debug.LogError("[LevelValidator] Invalid JSON: '" + txtObj.name + "'");
                        hadError = true;
                    }

                    if(level == null)
                        continue;

                    // TODO check the paths are valid
                    // TODO output the result below the button
                }

                if(!hadError)
                    Debug.Log("[LevelValidator] Everything is good!");
            }
        }
    }
    #endif
}