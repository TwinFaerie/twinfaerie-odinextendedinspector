using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
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
        private IEnumerable<Type> sourceData;

        protected IEnumerable<Type> GetSourceData()
        {
            var assemblies = TypeCollector.GetAllAssemblies();
            var data = TypeCollector.GetFilteredTypesFromAssemblies(assemblies);

            return data;
        }

        protected virtual void Refresh()
        {
            sourceData = GetSourceData();
            UpdateButtonContent();
        }


        protected override void Initialize()
        {
            Refresh();
        }

        private void UpdateButtonContent()
        {
            buttonContent.text = ValueEntry.SmartValue.ToString();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUILayout.BeginHorizontal();

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
                var selector = new TFTypeSelector(sourceData);
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

            if (SirenixEditorGUI.IconButton(EditorIcons.Refresh, SirenixGUIStyles.MiniButtonRight))
            {
                Refresh();
            }

            GUILayout.EndHorizontal();
        }
    }
}