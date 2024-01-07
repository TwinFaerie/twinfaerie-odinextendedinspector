using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    using Object = UnityEngine.Object;
    
    public abstract class BaseCreatableSO<T> where T : ScriptableObject
    {
        [BoxGroup("Info")]
        public string name;
        [BoxGroup("Info")] [ReadOnly]
        public string path;

        [BoxGroup("Content")] [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public T content;

        private string defaultName;

        public BaseCreatableSO()
        {
            content = ScriptableObject.CreateInstance<T>();
        }

        public void Setup(PathInfo pathInfo)
        {
            defaultName = name;
            path = pathInfo.pathName;
            AssetUtils.AutoCorrectPath(ref path);
        }

        [Button("Create New Item")]
        public virtual void CreateNewData()
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name))
            {
                Debug.LogWarning("Please input Info correctly");
                return;
            }
            
            AssetUtils.AutoCorrectPath(ref path);
            AssetUtils.CreateDirectoryIfNeeded(path);
            
            AssetDatabase.CreateAsset(content, path + name + ".asset");
            AssetDatabase.SaveAssets();
            
            ResetData();
        }

        private void ResetData()
        {
            name = defaultName;
            content = ScriptableObject.CreateInstance<T>();
        }
        
        public virtual void Destroy()
        {
            Object.DestroyImmediate(content);
        }
    }
}
