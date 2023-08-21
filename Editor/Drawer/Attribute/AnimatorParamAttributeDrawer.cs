using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public class AnimatorParamAttributeDrawer : OdinAttributeDrawer<AnimatorParamAttribute, string>
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

            var animatorController = animatorResolver.GetValue().runtimeAnimatorController as AnimatorController;
            if (animatorController is null)
            { return null; }

            if (Attribute.AnimatorParamType is null)
            {
                return animatorController.parameters.Select(x => x.name);
            }
            else
            {
                return animatorController.parameters
                    .Where(x => x.type == Attribute.AnimatorParamType)
                    .Select(x => x.name);
            }
        }

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
}
