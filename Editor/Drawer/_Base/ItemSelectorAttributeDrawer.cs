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
    public abstract class ItemSelectorAttributeDrawer<A, T> : OdinAttributeDrawer<A, T> where A : Attribute
    {
        private readonly GUIContent buttonContent = new();
        private IEnumerable<T> sourceData;

        protected abstract IEnumerable<T> GetSourceData();

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
                var selector = new GenericSelector<T>(sourceData);
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

    public abstract class MultipleItemSelectorAttributeDrawer<A, TD, T> : OdinAttributeDrawer<A, TD>
        where A : Attribute where TD : IEnumerable<T>
    {

        private readonly GUIContent buttonContent = new();
        private IEnumerable<T> sourceData;

        protected abstract IEnumerable<T> GetSourceData();
        protected abstract void UpdateValue(IEnumerable<T> x);

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
            if (!ValueEntry.SmartValue.Any())
            {
                buttonContent.text = "Nothing";
            }
            else if (ValueEntry.SmartValue.Count() == sourceData.Count())
            {
                buttonContent.text = "All";
            }
            else if (ValueEntry.SmartValue.Count() > 3)
            {
                buttonContent.text = "Mixed";
            }
            else
            {
                buttonContent.text = string.Join(", ", ValueEntry.SmartValue.ToString());
            }

            buttonContent.tooltip = buttonContent.text;
        }
    }

    public abstract class ItemSelectorDrawer<T> : OdinValueDrawer<T>
    {
        private readonly GUIContent buttonContent = new();
        private IEnumerable<T> sourceData;

        protected abstract IEnumerable<T> GetSourceData();

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
                var selector = new GenericSelector<T>(sourceData);
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

    public abstract class MultipleItemSelectorDrawer<TD, T> : OdinValueDrawer<TD> where TD : IEnumerable<T>
    {

        private readonly GUIContent buttonContent = new();
        private IEnumerable<T> sourceData;

        protected abstract IEnumerable<T> GetSourceData();
        protected abstract void UpdateValue(IEnumerable<T> x);

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
            if (!ValueEntry.SmartValue.Any())
            {
                buttonContent.text = "Nothing";
            }
            else if (ValueEntry.SmartValue.Count() == sourceData.Count())
            {
                buttonContent.text = "All";
            }
            else if (ValueEntry.SmartValue.Count() > 3)
            {
                buttonContent.text = "Mixed";
            }
            else
            {
                buttonContent.text = string.Join(", ", ValueEntry.SmartValue.ToString());
            }

            buttonContent.tooltip = buttonContent.text;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUILayout.BeginHorizontal();

            var rect = EditorGUILayout.GetControlRect(label != null);

            if (label == null)
                rect = EditorGUI.IndentedRect(rect);
            else
                rect = EditorGUI.PrefixLabel(rect, label);

            if (EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive))
            {
                var selector = new MultipleItemSelector<T>(sourceData);

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

            if (SirenixEditorGUI.IconButton(EditorIcons.Refresh, SirenixGUIStyles.MiniButtonRight))
            {
                Refresh();
            }

            GUILayout.EndHorizontal();
        }
    }

    public class MultipleItemSelector<T> : GenericSelector<T>
    {
        private readonly FieldInfo tfRequestCheckboxUpdate;
        private readonly IEnumerable<T> tfSource;

        internal MultipleItemSelector(IEnumerable<T> source) : base(source)
        {
            CheckboxToggle = true;
            tfSource = source;

            tfRequestCheckboxUpdate = typeof(GenericSelector<T>).GetField("requestCheckboxUpdate",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        protected override void DrawSelectionTree()
        {
            base.DrawSelectionTree();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("None"))
            {
                SetSelection(new List<T>());

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