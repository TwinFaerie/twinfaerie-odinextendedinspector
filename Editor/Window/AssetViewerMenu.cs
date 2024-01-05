using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public abstract class AssetViewerMenu<TObject, TCreatable> : OdinMenuEditorWindow where TObject : ScriptableObject where TCreatable : ICreatableSO, new()
    {
        private List<(bool isItemList, string name)> menuList;
        
        private readonly List<TObject> menuItemList = new();
        protected int selectedMenuIndex;
        private TObject selectedItem;

        private TCreatable creatableCache;
        private string searchString = string.Empty;

        protected override void Initialize()
        {
            SetupCreatableCache();
            SetupMenuList();
            RefreshData();
        }

        protected override void OnImGUI()
        {
            selectedItem = MenuTree?.Selection?.SelectedValue as TObject;

            var buttonResult = menuList[selectedMenuIndex].isItemList ? 
                TabMenuUtils.SelectButtonListItemList(ref selectedMenuIndex, menuList.Select(x => x.name).ToArray(), RefreshData, DeleteData, ref searchString, ForceMenuTreeRebuild) : 
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

            if (creatableCache is not null)
            {
                creatableCache.Destroy();
            }
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
                
                menuTree.Add(item.name, item);
            }
        }

        protected void GenerateCreateNewDataMenu(OdinMenuTree menuTree)
        {
            menuTree.Add("Create New", creatableCache);
        }

        private void RefreshData()
        {
            string filter = "t:" + typeof(TObject).Name;
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

        private void DeleteData()
        {
            if (selectedItem is null)
            { return; }
            
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedItem));
            
            RefreshData();
            ForceMenuTreeRebuild();
        }

        private void SetupMenuList()
        {
            menuList = GetMenuNameList();
        }

        private void SetupCreatableCache()
        {
            creatableCache = new TCreatable();
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
