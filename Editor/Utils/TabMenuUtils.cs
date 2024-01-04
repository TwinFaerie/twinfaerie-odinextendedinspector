using System;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities.Editor;

namespace TF.OdinExtendedInspector.Editor
{
    public static class TabMenuUtils
    {
        public static bool SelectButtonList(ref int index, string[] items, Action onRefresh)
        {
            var style = new GUIStyle(EditorStyles.miniButtonMid);
            style.stretchHeight = true;
            style.fixedHeight = 25;
            
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    var selectedIndex = GUILayout.Toolbar(index, items, style);
                    if (index != selectedIndex)
                    {
                        index = selectedIndex;
                        return true;
                    }
                    
                    GUILayout.Space(10);
                    
                    style.fixedWidth = 25;
                    if (GUILayout.Button(EditorIcons.Refresh.Raw, style))
                    {
                        onRefresh?.Invoke();
                        return true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            return false;
        }
        
        public static bool SelectButtonListWithDelete(ref int index, string[] items, Action onRefresh, Action onDelete)
        {
            var style = new GUIStyle(EditorStyles.miniButtonMid);
            style.stretchHeight = true;
            style.fixedHeight = 25;
            
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    var selectedIndex = GUILayout.Toolbar(index, items, style);
                    if (index != selectedIndex)
                    {
                        index = selectedIndex;
                        return true;
                    }
                    
                    GUILayout.Space(10);
                    
                    style.fixedWidth = 25;
                    if (GUILayout.Button(EditorIcons.Refresh.Raw, style))
                    {
                        onRefresh?.Invoke();
                        return true;
                    }
                    
                    style.fixedWidth = 25;
                    if (GUILayout.Button(EditorIcons.X.Raw, style))
                    {
                        onDelete?.Invoke();
                        return true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            return false;
        }
    }
}