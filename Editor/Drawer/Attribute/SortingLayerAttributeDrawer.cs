using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TF.OdinExtendedInspector.Editor
{
    internal class SortingLayerAttributeDrawer : StringSelectorDrawer<SortingLayerAttribute>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }
    }

    internal class SortingLayerAttributeListDrawer : MultipleStringSelectorDrawer<SortingLayerAttribute, List<string>>
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

    internal class SortingLayerAttributeArrayDrawer : MultipleStringSelectorDrawer<SortingLayerAttribute, string[]>
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