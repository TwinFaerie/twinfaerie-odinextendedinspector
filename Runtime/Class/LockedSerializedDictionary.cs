namespace TF.OdinExtendedInspector
{
    public class LockedSerializedDictionary<TK, TV> : SerializedDictionary<TK, TV>
    {
        public override void Add(TK key, TV value)
        {
            #if UNITY_EDITOR
            base.Add(key, value);
            #endif
        }

        public override bool Remove(TK key)
        {
            #if UNITY_EDITOR
            return base.Remove(key);
            #endif
            return false;
        }

        public override void Clear()
        {
            #if UNITY_EDITOR
            base.Clear();
            #endif
        }
    }
}