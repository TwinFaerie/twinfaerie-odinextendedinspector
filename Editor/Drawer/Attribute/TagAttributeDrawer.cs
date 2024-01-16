using System.Collections.Generic;
using System.Linq;

namespace TF.OdinExtendedInspector.Editor
{
    internal class TagAttributeDrawer : StringSelectorDrawer<TagAttribute>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }
    }

    internal class TagAttributeListDrawer : MultipleStringSelectorDrawer<TagAttribute, List<string>>
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

    internal class TagAttributeArrayDrawer : MultipleStringSelectorDrawer<TagAttribute, string[]>
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