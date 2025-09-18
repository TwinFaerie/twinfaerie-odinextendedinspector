#if UNITY_NETCODE

using Sirenix.OdinInspector.Editor;
using Unity.Netcode;
using UnityEditor;

namespace TF.OdinExtendedInspector.Editor
{
    [CustomEditor(typeof(NetworkBehaviour), true)]
    public class OdinNetworkBehaviourEditor : OdinEditor
    {
        
    }
}

#endif