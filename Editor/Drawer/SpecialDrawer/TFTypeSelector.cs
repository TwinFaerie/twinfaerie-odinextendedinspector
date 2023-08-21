using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public class TFTypeSelector : TypeSelector
    {
        private readonly TypeConstraintGrouping grouping;
        private bool isFlatten => grouping == TypeConstraintGrouping.None || FlattenTree;
        
        public TFTypeSelector(IEnumerable<Type> types) : this(types, TypeConstraintGrouping.ByNamespace) { }
        public TFTypeSelector(IEnumerable<Type> types, TypeConstraintGrouping grouping) : base(types, false)
        {
            this.grouping = grouping;
        }

        protected override void BuildSelectionTree(OdinMenuTree tree)
        {
            tree.Config.UseCachedExpandedStates = false;
            tree.DefaultMenuStyle.NotSelectedIconAlpha = 1f;
            tree.Config.SelectMenuItemsOnMouseDown = true;

            var baseClass = GetType().BaseType;
            var baseClassTypes = baseClass.GetField("types", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this) as IEnumerable<Type>;
            
            foreach (Type t in baseClassTypes)
            {
                var niceName = t.GetNiceName();
                var typeNamePath = GetCustomTypeNamePath(t, niceName);

                var odinMenuItem = tree.AddObjectAtPath(typeNamePath, t).Last();
                odinMenuItem.SearchString = ((niceName == typeNamePath) ? typeNamePath : (niceName + "|" + typeNamePath));
                
                if (!string.IsNullOrEmpty(t.Namespace) && !HideNamespaces)
                {
                    odinMenuItem.OnDrawItem = (Action<OdinMenuItem>)Delegate.Combine(odinMenuItem.OnDrawItem, (Action<OdinMenuItem>)delegate (OdinMenuItem x)
                    {
                        GUI.Label(x.Rect.Padding(10f, 0f).AlignCenterY(16f), t.Namespace, SirenixGUIStyles.RightAlignedGreyMiniLabel);
                    });
                }
            }

            tree.EnumerateTree((OdinMenuItem x) => x.Value != null, includeRootNode: false).AddThumbnailIcons();

            tree.Selection.SupportsMultiSelect = false;
            tree.Selection.SelectionChanged += delegate
            {
                var baseClassLastTypeField = baseClass.GetField("lastType", BindingFlags.NonPublic | BindingFlags.Instance);
                baseClassLastTypeField.SetValue(this, SelectionTree.Selection
                    .Select((OdinMenuItem x) => x.Value)
                    .OfType<Type>()
                    .LastOrDefault() 
                ?? baseClassLastTypeField.GetValue(this));
            };
        }

        private string GetCustomTypeNamePath(Type t, string niceName)
        {
            var text = niceName;

            if (isFlatten || HideNamespaces)
            {
                return text;
            }

            if (string.IsNullOrEmpty(t.Namespace))
            {
                text = string.Concat(str1: "/", str0: "_Ungrouped", str2: text);
            }
            else
            {
                var namespaceText = t.Namespace;
                if (grouping == TypeConstraintGrouping.ByNamespace)
                {
                    namespaceText = namespaceText.Replace(".", "/");
                }

                text = string.Concat(str1: "/", str0: namespaceText, str2: text);
            }

            return text;
        }
    }
}