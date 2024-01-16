using System.Collections.Generic;
using System.Linq;

namespace TF.OdinExtendedInspector.Editor
{
    internal class TagAttributeDrawer : ItemSelectorAttributeDrawer<TagAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }
    }

    internal class TagAttributeListDrawer : MultipleItemSelectorAttributeDrawer<TagAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToList();
        }
    }

    internal class TagAttributeArrayDrawer : MultipleItemSelectorAttributeDrawer<TagAttribute, IEnumerable<string>, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }

        protected override void UpdateValue(IEnumerable<string> x)
        {
            ValueEntry.SmartValue = x.ToArray();
        }
    }
}