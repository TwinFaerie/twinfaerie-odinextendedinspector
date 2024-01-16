using System.Collections.Generic;
using System.Linq;

namespace TF.OdinExtendedInspector.Editor
{
    internal class LayerAttributeDrawer : ItemSelectorAttributeDrawer<LayerAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.layers;
        }
    }

    internal class LayerAttributeListDrawer : MultipleItemSelectorAttributeDrawer<LayerAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.layers;
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToList();
        }
    }

    internal class LayerAttributeArrayDrawer : MultipleItemSelectorAttributeDrawer<LayerAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.layers;
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToArray();
        }
    }
}