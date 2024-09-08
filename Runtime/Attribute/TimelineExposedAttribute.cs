#if UNITY_TIMELINE
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    public class TimelineExposedAttribute : PropertyAttribute
    {
        public string TimelineName;

        public TimelineExposedAttribute(string timelineName)
        {
            TimelineName = timelineName;
        }
    }
}
#endif