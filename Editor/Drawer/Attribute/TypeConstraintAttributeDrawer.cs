using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal class TypeConstraintAttributeDrawer : TypeAttributeDrawer<TypeConstraintAttribute>
    {

    }

    internal class TypeOptionsAttributeDrawer : TypeAttributeDrawer<TypeOptionsAttribute>
    {

    }

    internal class TypeAttributeDrawer<T> : OdinAttributeDrawer<T, TypeRef> where T : TypeOptionsAttribute
    {
        private readonly GUIContent buttonContent = new();
        private IEnumerable<Type> sourceData;

        protected virtual IEnumerable<Type> GetSourceData()
        {
            var assemblies = GetAssemblies();

            var data = new List<Type>();

            data.AddRange(TypeCollector.GetFilteredTypesFromAssemblies(assemblies, Attribute));
            data.AddRange(GetIncludedTypes());

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
                var selector = new TFTypeSelector(sourceData, Attribute.Grouping);
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

        private IEnumerable<Type> GetIncludedTypes()
        {
            if (Attribute.IncludeTypes is null)
            { return Array.Empty<Type>(); }

            return Attribute.IncludeTypes.Where(x => x is not null);
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            if (Attribute.Assemblies is null)
            { return TypeCollector.GetAllAssemblies(); }

            return Attribute.Assemblies.Select(TypeCollector.TryLoadAssembly).Where(x => x is not null);
        }
    }
}