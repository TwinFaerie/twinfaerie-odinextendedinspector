using TF.Initializer;
using UnityEngine;
using UnityEngine.Scripting;

namespace TF.Samples.Initializer
{
    public class MonoService : MonoBehaviour, IServiceMono
    {
        public bool IsReady => true;

        public void Init()
        {
            Debug.Log("Mono Hello");
        }

        [Preserve]
        private void Inject(PlainService plain, PrefabService prefab)
        {
            Debug.Log("Mono Injected with plain C# with data " + plain.GetData());
            Debug.Log("Mono Injected with prefab with data " + prefab.GetData());
        }

        [Preserve]
        private void DelayedInit()
        {
            Debug.Log("Mono Hello but Delayed");
        }

        public string GetData()
        {
            return "I am mono";
        }
    }
}
