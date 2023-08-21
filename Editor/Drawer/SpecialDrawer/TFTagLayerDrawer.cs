using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal abstract class TFTagLayerSingleDrawer<A> : OdinAttributeDrawer<A, string> where A : Attribute
    {
        private readonly GUIContent buttonContent = new();
        private IEnumerable<string> sourceData;

        protected abstract IEnumerable<string> GetSourceData();

        protected override void Initialize()
        {
            sourceData = GetSourceData();
            UpdateButtonContent();
        }

        private void UpdateButtonContent()
        {
            buttonContent.text = ValueEntry.SmartValue;
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
                var selector = new GenericSelector<string>(sourceData);
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

    internal abstract class TFTagLayerMultipleDrawer<A, T> : OdinAttributeDrawer<A, T> where A : Attribute where T : IEnumerable<string>
    {

        private readonly GUIContent buttonContent = new();

        protected abstract IEnumerable<string> GetSourceData();
        protected abstract void UpdateValue(IEnumerable<string> x);

        protected override void Initialize()
        {
            UpdateButtonContent();
        }

        private void UpdateButtonContent()
        {
            if (ValueEntry.SmartValue.Count() == 0)
            {
                buttonContent.text = "Nothing";
            }
            else if (ValueEntry.SmartValue.Count() == GetSourceData().Count())
            {
                buttonContent.text = "All";
            }
            else if (ValueEntry.SmartValue.Count() > 3)
            {
                buttonContent.text = "Mixed";
            }
            else
            {
                buttonContent.text = string.Join(", ", ValueEntry.SmartValue);
            }

            buttonContent.tooltip = buttonContent.text;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect(label != null);

            if (label == null)
                rect = EditorGUI.IndentedRect(rect);
            else
                rect = EditorGUI.PrefixLabel(rect, label);

            if (EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive))
            {
                var selector = new TFTagLayerSelector(GetSourceData());

                rect.y += rect.height;

                selector.SetSelection(ValueEntry.SmartValue);
                selector.ShowInPopup(rect.position);

                selector.SelectionChanged += x =>
                {
                    ValueEntry.Property.Tree.DelayAction(() =>
                    {
                        UpdateValue(x);
                        UpdateButtonContent();
                    });
                };
            }
        }
    }

    internal class TFTagLayerSelector : GenericSelector<string>
    {
        private readonly FieldInfo tfRequestCheckboxUpdate;
        private readonly IEnumerable<string> tfSource;

        internal TFTagLayerSelector(IEnumerable<string> source) : base(source)
        {
            CheckboxToggle = true;
            tfSource = source;

            tfRequestCheckboxUpdate = typeof(GenericSelector<string>).GetField("requestCheckboxUpdate",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        protected override void DrawSelectionTree()
        {
            base.DrawSelectionTree();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("None"))
            {
                SetSelection(new List<string>());

                tfRequestCheckboxUpdate.SetValue(this, true);
                TriggerSelectionChanged();
            }

            if (GUILayout.Button("All"))
            {
                SetSelection(tfSource);

                tfRequestCheckboxUpdate.SetValue(this, true);
                TriggerSelectionChanged();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}