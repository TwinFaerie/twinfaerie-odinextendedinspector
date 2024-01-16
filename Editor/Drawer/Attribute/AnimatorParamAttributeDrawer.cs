using Sirenix.OdinInspector.Editor.ValueResolvers;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal class AnimatorParamAttributeDrawer : ItemSelectorAttributeDrawer<AnimatorParamAttribute, string>
    {
        private IEnumerable<string> sourceData;

        protected override IEnumerable<string> GetSourceData()
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
    }
}
