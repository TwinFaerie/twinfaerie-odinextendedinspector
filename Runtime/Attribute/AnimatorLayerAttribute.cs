using System;
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AnimatorLayerAttribute : PropertyAttribute
    {
        public string AnimatorName { get; private set; }

        public AnimatorLayerAttribute(string animatorName)
        {
            AnimatorName = animatorName;
        }
    }
}