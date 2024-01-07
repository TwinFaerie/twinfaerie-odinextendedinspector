using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public abstract class AssetViewerMenu<TObject, TCreatable> : OdinMenuEditorWindow where TObject : ScriptableObject where TCreatable : BaseCreatableSO<TObject>, new()
    {
        private List<(bool isItemList, string name)> menuList;
        
        private readonly List<TObject> menuItemList = new();
        protected int selectedMenuIndex;
        private TObject selectedItem;

        private TCreatable creatableCache;
        private PathInfo pathInfo;
        
        private string searchString = string.Empty;

        protected override void Initialize()
        {
            SetupCreateObject();
            SetupMenuList();
            RefreshData();
        }

        protected override void OnImGUI()
        {
            selectedItem = MenuTree?.Selection?.SelectedValue as TObject;

            var buttonResult = menuList[selectedMenuIndex].isItemList ? 
                TabMenuUtils.SelectButtonListItemList(ref selectedMenuIndex, menuList.Select(x => x.name).ToArray(), SelectData, RefreshData, DeleteData, ref searchString, ForceMenuTreeRebuild) : 
                TabMenuUtils.SelectButtonList(ref selectedMenuIndex, menuList.Select(x => x.name).ToArray(), RefreshData);
            
            if (buttonResult)
            {
                if (menuList[selectedMenuIndex].isItemList)
                {
                    RefreshData();
                }
                
                ForceMenuTreeRebuild();
            }
            
            base.OnImGUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            creatableCache?.Destroy();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var menuTree = new OdinMenuTree();
            
            switch (selectedMenuIndex)
            {
                case 0:
                {
                    GenerateAvailableSettingList(menuTree);
                    break;
                }
                case 1:
                {
                    GenerateCreateNewDataMenu(menuTree);
                    break;
                }
                default:
                {
                    GenerateAvailableSettingList(menuTree);
                    break;
                }
            }
            
            return menuTree;
        }

        protected void GenerateAvailableSettingList(OdinMenuTree menuTree)
        {
            foreach (var item in menuItemList)
            {
                if (item == null)
                { continue; }

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    if (!item.name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }
                
                menuTree.Add(item.name.Replace('_', '/'), item);
            }
        }

        protected void GenerateCreateNewDataMenu(OdinMenuTree menuTree)
        {
            menuTree.Add("Create New", creatableCache);
            menuTree.Add("Change Path", pathInfo);
        }

        private void RefreshData()
        {
            var filter = "t:" + typeof(TObject).Name;
            var allObjectGuids = AssetDatabase.FindAssets(filter);
                
            foreach (var guid in allObjectGuids)
            {
                var item = AssetDatabase.LoadAssetAtPath<TObject>(AssetDatabase.GUIDToAssetPath(guid));

                if (item is not null)
                {
                    menuItemList.Add(item);
                }
            }
        }

        private void SelectData()
        {
            if (selectedItem is null)
            { return; }
            
            Selection.activeObject = selectedItem;
        }

        private void DeleteData()
        {
            if (selectedItem is null)
            { return; }
            
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedItem));
            AssetDatabase.SaveAssets();
            
            RefreshData();
            ForceMenuTreeRebuild();
        }

        private void SetupMenuList()
        {
            menuList = GetMenuNameList();
        }

        private void SetupCreateObject()
        {
            AssetDatabase.Refresh();
            
            creatableCache = new TCreatable();
            
            var filter = "t:" + nameof(PathInfo);
            var foundGuidArray = AssetDatabase.FindAssets(filter);
            
            foundGuidArray = foundGuidArray.Where(guid => AssetDatabase.LoadAssetAtPath<PathInfo>(AssetDatabase.GUIDToAssetPath(guid)).type == GetType()).ToArray();

            while (foundGuidArray == null || foundGuidArray.Length <= 0)
            {
                AssetUtils.AutoCorrectPath(ref creatableCache.path);
                var pathInfoPath = creatableCache.path + "_Info/";
                AssetUtils.CreateDirectoryIfNeeded(pathInfoPath);

                var pathInfoInstance = CreateInstance<PathInfo>();
                pathInfoInstance.pathName = creatableCache.path;
                pathInfoInstance.type = GetType();
                pathInfoInstance.onSetPath = ChangePath;
                
                AssetDatabase.CreateAsset(pathInfoInstance, pathInfoPath + nameof(PathInfo) + ".asset");
                AssetDatabase.SaveAssets();
                
                foundGuidArray = AssetDatabase.FindAssets(filter);
            }
            
            var selectedObject = AssetDatabase.LoadAssetAtPath<PathInfo>(AssetDatabase.GUIDToAssetPath(foundGuidArray[0]));
            
            AssetUtils.AutoCorrectPath(ref selectedObject.pathName);

            if (selectedObject.name != "PathInfo")
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(selectedObject), "PathInfo");
            }

            var newPath = selectedObject.pathName + "_Info/" + nameof(PathInfo) + ".asset";
            var oldPath = AssetDatabase.GetAssetPath(selectedObject);
            
            if (newPath != oldPath)
            {
                ChangePath(oldPath, newPath, selectedObject.pathName);
            }

            if (foundGuidArray.Length > 1)
            {
                for (int i = 1; i < foundGuidArray.Length; i++)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(foundGuidArray[i]));
                }
            }
            
            AssetDatabase.SaveAssets();
            
            pathInfo = selectedObject;

            creatableCache.Setup(pathInfo);
        }

        private void ChangePath(string oldPath, string newPath, string contentPath)
        {
            creatableCache.path = contentPath;
            
            AssetUtils.CreateDirectoryIfNeeded(newPath);
            AssetDatabase.MoveAsset(oldPath, newPath);
            AssetDatabase.SaveAssets();
            
            AssetDatabase.Refresh();
            
            creatableCache.Destroy();
            creatableCache = new TCreatable();
            creatableCache.Setup(AssetDatabase.LoadAssetAtPath<PathInfo>(newPath));
        }

        protected virtual List<(bool isItemList, string name)> GetMenuNameList()
        {
            return new List<(bool isItemList, string name)>
            {
                new (true, "Available Items"),
                new (false, "Create New Item"),
            };
        }
    }
}
