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
        
        public static bool SelectButtonListItemList(ref int index, string[] items, Action onSelect, Action onRefresh, Action onDelete, ref string searchString, Action onSearch)
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
                    
                    if (GUILayout.Button(EditorIcons.Folder.Raw, style))
                    {
                        onSelect?.Invoke();
                        return true;
                    }
                    
                    if (GUILayout.Button(EditorIcons.Refresh.Raw, style))
                    {
                        onRefresh?.Invoke();
                        return true;
                    }
                    
                    if (GUILayout.Button(EditorIcons.X.Raw, style))
                    {
                        onDelete?.Invoke();
                        return true;
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal(new GUIStyle { fixedHeight = 25, stretchHeight = true });
                {
                    GUILayout.Space(5);
                    GUILayout.Label(EditorIcons.MagnifyingGlass.Raw, new GUIStyle { fixedHeight = 15, fixedWidth = 15, contentOffset = new Vector2(2,3)});
                    GUILayout.Space(5);

                    var newSearchString = GUILayout.TextField(searchString);
                    if (searchString != newSearchString)
                    {
                        searchString = newSearchString;
                        onSearch?.Invoke();
                    }

                    GUILayout.Space(5);
                    if (GUILayout.Button("Clear", new GUIStyle(EditorStyles.miniButtonMid) { fixedWidth = 80, fixedHeight = 18 }))
                    {
                        searchString = string.Empty;
                        onSearch?.Invoke();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            return false;
        }
    }
}