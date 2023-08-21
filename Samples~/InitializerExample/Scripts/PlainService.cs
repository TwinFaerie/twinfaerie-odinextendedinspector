using TF.Initializer;
using UnityEngine.Scripting;

namespace TF.Samples.Initializer
{
    public class PlainService : IService
    {
        public bool IsReady => true;

        public void Init()
        {
            UnityEngine.Debug.Log("Plain C# Hello");
        }

        [Preserve]
        private void Inject(MonoService mono, PrefabService prefab)
        {
            UnityEngine.Debug.Log("Plain C# Injected with mono with data " + mono.GetData());
            UnityEngine.Debug.Log("Plain C# Injected with prefab with data " + prefab.GetData());
        }

        [Preserve]
        private void DelayedInit()
        {
            UnityEngine.Debug.Log("Plain C# Hello but Delayed");
        }

        public string GetData()
        {
            return "I am Plain C#";
        }
    }
}