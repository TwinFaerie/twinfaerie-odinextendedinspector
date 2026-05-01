namespace TF.OdinExtendedInspector
{
    public class LockedSerializedRefDictionary<TK, TV> : SerializedRefDictionary<TK, TV>
    {
        public override void Add(TK key, TV value)
        {
            #if UNITY_EDITOR
            if (UnityEngine.Application.isPlaying) return;
            base.Add(key, value);
            #endif
        }

        public override bool Remove(TK key)
        {
            #if UNITY_EDITOR
            if (UnityEngine.Application.isPlaying) return false;
            return base.Remove(key);
            #endif
            return false;
        }

        public override void Clear()
        {
            #if UNITY_EDITOR
            if (UnityEngine.Application.isPlaying) return;
            base.Clear();
            #endif
        }
    }
}