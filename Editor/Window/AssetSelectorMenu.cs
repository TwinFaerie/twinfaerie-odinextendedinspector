using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public abstract class AssetSelectorMenu<T, U> : OdinMenuEditorWindow where T : ScriptableObject where U : ICreatableSO, new()
    {
        private readonly List<T> menuItemList = new();
        private int selectedMenuIndex;
        private T selectedItem;

        protected abstract ScriptableObject ActiveMenuSetting { get; }

        protected override void Initialize()
        {
            RefreshData();
        }

        protected override void OnImGUI()
        {
            selectedItem = MenuTree?.Selection?.SelectedValue as T;
            bool buttonResult = false;
            
            if (selectedItem is not null)
            {
                buttonResult = TabMenuUtils.SelectButtonListWithDelete(ref selectedMenuIndex, GetMenuTypeNameArray(), RefreshData, DeleteData);
            }
            else
            {
                buttonResult = TabMenuUtils.SelectButtonList(ref selectedMenuIndex, GetMenuTypeNameArray(), RefreshData);
            }
            
            if (buttonResult)
            {
                if ((MenuType)selectedMenuIndex == MenuType.Available_Setting)
                {
                    RefreshData();
                }
                
                ForceMenuTreeRebuild();
            }
            
            base.OnImGUI();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var menuTree = new OdinMenuTree();
            
            if (selectedMenuIndex < 0 || selectedMenuIndex >= GetMenuTypeNameArray().Length)
            {
                GenerateActiveSetting(menuTree);
                return menuTree;
            }
            
            switch ((MenuType)selectedMenuIndex)
            {
                case MenuType.Active_Setting:
                {
                    GenerateActiveSetting(menuTree);
                    break;
                }
                case MenuType.Available_Setting:
                {
                    GenerateAvailableSettingList(menuTree);
                    break;
                }
                case MenuType.Create_New:
                {
                    GenerateCreateNewDataMenu(menuTree);
                    break;
                }
                default:
                {
                    GenerateActiveSetting(menuTree);
                    break;
                }
            }
            
            return menuTree;
        }

        private void GenerateActiveSetting(OdinMenuTree menuTree)
        {
            menuTree.Add("Active Setting", ActiveMenuSetting);
        }

        private void GenerateAvailableSettingList(OdinMenuTree menuTree)
        {
            foreach (var item in menuItemList)
            {
                if (item == null)
                { continue; }
                
                menuTree.Add(item.name, item);
            }
        }

        private void GenerateCreateNewDataMenu(OdinMenuTree menuTree)
        {
            menuTree.Add("Create New", new U());
        }

        private void RefreshData()
        {
            string filter = "t:" + typeof(T).Name;
            var allObjectGuids = AssetDatabase.FindAssets(filter);
                
            foreach (var guid in allObjectGuids)
            {
                var item = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));

                if (item is not null)
                {
                    menuItemList.Add(item);
                }
            }
        }

        private void DeleteData()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedItem));
            
            RefreshData();
            ForceMenuTreeRebuild();
        }

        private string[] GetMenuTypeNameArray()
        {
            return Enum.GetNames(typeof(MenuType)).Select(x => x.Replace("_", " ")).ToArray();
        }
    }
}
