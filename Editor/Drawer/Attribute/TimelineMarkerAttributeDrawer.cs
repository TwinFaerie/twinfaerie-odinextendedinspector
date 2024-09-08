#if UNITY_TIMELINE
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using TF.Timeline;
using UnityEngine.Timeline;

namespace TF.OdinExtendedInspector.Editor
{
    public class TimelineMarkerAttributeDrawer : ItemSelectorAttributeDrawer<TimelineMarkerAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            if (Attribute.TimelineName is null) return null;

            var timelineReference = ValueResolver.Get<TimelineAsset>(Property, Attribute.TimelineName).GetValue();
            return timelineReference?.GetOutputTracks().SelectMany(track => track.GetMarkers().Where(marker => marker is GenericMarker).Select(marker => ((GenericMarker)marker).name));
        }
    }
}
#endif