using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TF.OdinExtendedInspector.Editor
{
    internal class SortingLayerAttributeDrawer : ItemSelectorAttributeDrawer<SortingLayerAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }
    }

    internal class SortingLayerAttributeListDrawer : MultipleItemSelectorAttributeDrawer<SortingLayerAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToList();
        }
    }

    internal class SortingLayerAttributeArrayDrawer : MultipleItemSelectorAttributeDrawer<SortingLayerAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToArray();
        }
    }
}