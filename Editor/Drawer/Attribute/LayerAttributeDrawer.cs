using System.Collections.Generic;
using System.Linq;

namespace TF.OdinExtendedInspector.Editor
{
    internal class LayerAttributeDrawer : TFTagLayerSingleDrawer<LayerAttribute>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.layers;
        }
    }

    internal class LayerAttributeListDrawer : TFTagLayerMultipleDrawer<LayerAttribute, List<string>>
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

    internal class LayerAttributeArrayDrawer : TFTagLayerMultipleDrawer<LayerAttribute, string[]>
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