using Sirenix.OdinInspector.Editor.ValueResolvers;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal class AnimatorLayerAttributeDrawer : ItemSelectorAttributeDrawer<AnimatorLayerAttribute, string>
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

            return animatorController.layers.Select(x => x.name);
        }
    }
}
