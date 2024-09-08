#if UNITY_TIMELINE
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    public class TimelineTrackAttribute : PropertyAttribute
    {
        public string TimelineName;

        public TimelineTrackAttribute(string timelineName)
        {
            TimelineName = timelineName;
        }
    }
}
#endif