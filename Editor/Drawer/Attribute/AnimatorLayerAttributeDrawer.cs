using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public class AnimatorLayerAttributeDrawer : OdinAttributeDrawer<AnimatorLayerAttribute, string>
    {
        private readonly GUIContent buttonContent = new();
        private IEnumerable<string> sourceData;

        private IEnumerable<string> GetSourceData()
        {
            if (Attribute.AnimatorName is null)
            { return null; }

            var animatorResolver = ValueResolver.Get<Animator>(Property, Attribute.AnimatorName);
            if (animatorResolver is null)
            { return null; }

            var runtimeAnimatorController = animatorResolver.GetValue().runtimeAnimatorController;
            if (runtimeAnimatorController == null)
            { return null; }

            var animatorController = runtimeAnimatorController as AnimatorController;
            if (animatorController == null)
            {
                var animatorOverrideController = runtimeAnimatorController as AnimatorOverrideController;
                animatorController = animatorOverrideController.runtimeAnimatorController as AnimatorController;

                if (animatorController == null) 
                { return null; }
            }

            return animatorController.layers.Select(x => x.name);
        }

        private void Refresh()
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
            buttonContent.text = ValueEntry.SmartValue;
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

            if (EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive, SirenixGUIStyles.DropDownMiniButton))
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

            if (SirenixEditorGUI.IconButton(EditorIcons.Refresh, SirenixGUIStyles.MiniButtonRight))
            {
                Refresh();
            }

            GUILayout.EndHorizontal();
        }
    }
}
