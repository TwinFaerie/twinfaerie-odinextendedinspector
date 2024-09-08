#if UNITY_TIMELINE && TF_TIMELINE
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    public class TimelineMarkerAttribute : PropertyAttribute
    {
        public string TimelineName;

        public TimelineMarkerAttribute(string timelineName)
        {
            TimelineName = timelineName;
        }
    }
}
#endif