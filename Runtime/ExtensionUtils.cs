using System.Collections.Generic;

namespace TF.OdinExtendedInspector
{
    public static class ExtensionUtils
    {
        public static LockedSerializedDictionary<TKey, TValue> ToLockedDictionary<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            var result = new LockedSerializedDictionary<TKey, TValue>();
            foreach (var kvp in source)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}