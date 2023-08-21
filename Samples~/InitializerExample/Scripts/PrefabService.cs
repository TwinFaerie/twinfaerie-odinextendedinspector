using TF.Initializer;
using UnityEngine;
using UnityEngine.Scripting;

namespace TF.Samples.Initializer
{
    public class PrefabService : MonoBehaviour, IServiceMono
    {
        public bool IsReady => true;

        public void Init()
        {
            Debug.Log("Prefab Hello");
        }

        [Preserve]
        private void Inject(PlainService plain, MonoService mono)
        {
            Debug.Log("Prefab Injected with plain C# with data " + plain.GetData());
            Debug.Log("Prefab Injected with mono with data " + mono.GetData());
        }

        [Preserve]
        private void DelayedInit()
        {
            Debug.Log("Prefab Hello but Delayed");
        }

        public string GetData()
        {
            return "I am Prefab";
        }
    }
}