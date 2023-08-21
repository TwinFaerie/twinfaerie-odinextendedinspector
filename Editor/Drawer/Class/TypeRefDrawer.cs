using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal class TypeRefDrawer : OdinValueDrawer<TypeRef>
    {
        private readonly GUIContent buttonContent = new();

        protected IEnumerable<Type> GetSourceData()
        {
            var assemblies = TypeCollector.GetAllAssemblies();
            var data = TypeCollector.GetFilteredTypesFromAssemblies(assemblies);

            return data;
        }

        protected override void Initialize()
        {
            UpdateButtonContent();
        }

        private void UpdateButtonContent()
        {
            buttonContent.text = ValueEntry.SmartValue.ToString();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect(label != null);

            if (label == null)
            {
                rect = EditorGUI.IndentedRect(rect);
            }
            else
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            if (EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive))
            {
                var selector = new TFTypeSelector(GetSourceData());
                selector.SetSelection(ValueEntry.SmartValue);
                selector.ShowInPopup(rect.position);

                selector.SelectionChanged += x =>
                {
                    ValueEntry.Property.Tree.DelayAction(() =>
                    {
                        ValueEntry.SmartValue = x.FirstOrDefault();
                        UpdateButtonContent();
                    });
                };
            }
        }
    }
}