#if UNITY_TIMELINE
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UnityEngine;
using UnityEngine.Timeline;

namespace TF.OdinExtendedInspector.Editor
{
    public class TimelineExposedAttributeDrawer : ItemSelectorAttributeDrawer<TimelineExposedAttribute, string>
    {
        protected override IEnumerable<string> GetSourceData()
        {
            if (Attribute.TimelineName is null) return null;

            var timelineReference = ValueResolver.Get<TimelineAsset>(Property, Attribute.TimelineName).GetValue();
            return timelineReference?.GetOutputTracks().SelectMany(track => track.GetClips().SelectMany(FindExposedReferenceNameList)).Distinct();
        }
        
        private List<string> FindExposedReferenceNameList(TimelineClip clip)
        {
            var asset = clip.asset;
            
            // Use reflection to find all fields of type ExposedReference<T>
            var fieldInfoList = asset.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            return fieldInfoList
                .Where(field => field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(ExposedReference<>))
                .Select(field => field.GetValue(asset))
                .Where(exposedRef => exposedRef.GetType().GetField("exposedName") != null && exposedRef.GetType().GetField("exposedName").GetValue(exposedRef) is PropertyName)
                .Select(exposedRef => ((PropertyName)exposedRef.GetType().GetField("exposedName").GetValue(exposedRef)).ToString().Split(":")[0])
                .ToList();
        }
    }
}
#endif