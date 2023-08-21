using Sirenix.OdinInspector.Editor;
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

            foreach (var item in assemblies)
            {
                data.AddRange(TypeCollector.GetFilteredTypesFromAssembly(item, Attribute));
            }

            data.AddRange(GetIncludedTypes());

            return data;
        }

        protected override void Initialize()
        {
            sourceData = GetSourceData();
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