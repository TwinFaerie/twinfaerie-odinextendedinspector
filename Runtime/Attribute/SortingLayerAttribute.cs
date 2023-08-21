using System;

namespace TF.OdinExtendedInspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SortingLayerAttribute : Attribute
    {

    }
}