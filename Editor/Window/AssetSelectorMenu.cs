using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public abstract class AssetSelectorMenu<TObject, TCreatable> : AssetViewerMenu<TObject, TCreatable> where TObject : ScriptableObject where TCreatable : ICreatableSO, new()
    {
        protected abstract ScriptableObject ActiveMenuSetting { get; }

        protected override OdinMenuTree BuildMenuTree()
        {
            var menuTree = new OdinMenuTree();
            
            switch (selectedMenuIndex)
            {
                case 0:
                {
                    GenerateActiveSetting(menuTree);
                    break;
                }
                case 1:
                {
                    GenerateAvailableSettingList(menuTree);
                    break;
                }
                case 2:
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


        protected override List<(bool, string)> GetMenuNameList()
        {
            return new List<(bool isItemList, string name)>
            {
                new (false, "Active Setting"),
                new (true, "Available Settings"),
                new (false, "Create New Setting"),
            };
        }
    }
}
