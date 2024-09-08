#if UNITY_TIMELINE
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UnityEngine.Timeline;

namespace TF.OdinExtendedInspector.Editor
{
    public class TimelineTrackAttributeDrawer : ItemSelectorAttributeDrawer<TimelineTrackAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            if (Attribute.TimelineName is null) return null;
            
            var timelineReference = ValueResolver.Get<TimelineAsset>(Property, Attribute.TimelineName).GetValue();
            return timelineReference == null ? null : timelineReference.GetOutputTracks().Select(track => track.name);
        }
    }
}
#endif