using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TF.OdinExtendedInspector.Editor
{
    public abstract class BaseCreatableSO<T> where T : ScriptableObject
    {
        [BoxGroup("Info")]
        public string name;
        [BoxGroup("Info")] [ReadOnly]
        public string path;
        [BoxGroup("Info")] [ValueDropdown("GetTypeNameList")] [OnValueChanged("OnTypeChanged")]
        public string type;

        [BoxGroup("Content")] [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)] [HideIf("isInvalidClass")]
        public T content;

        private string defaultName;
        private Dictionary<string, Type> typeDict;
        
        private bool isInvalidClass => content == null;
        private List<string> GetTypeNameList => typeDict.Keys.ToList();

        public BaseCreatableSO()
        {
            typeDict = GetTypeDict();
            if (!typeDict.Any()) return;
            
            type = typeDict.Keys.First();
            OnTypeChanged(type);
        }

        public void Setup(PathInfo pathInfo)
        {
            defaultName = name;
            path = pathInfo.pathName;
            AssetUtils.AutoCorrectPath(ref path);
        }

        [Button("Create New Item")] [DisableIf("isInvalidClass")]
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

        private static Dictionary<string, Type> GetTypeDict()
        {
            return AssemblyUtilities.GetTypes(AssemblyCategory.ProjectSpecific)
                .Where(t => t.IsSubclassOf(typeof(T)))
                .Prepend(typeof(T))
                .Where(t => !t.IsAbstract)
                .ToDictionary(t => t.Name);
        }

        private void OnTypeChanged(string typeName)
        {
            content = ScriptableObject.CreateInstance(typeDict[typeName]) as T;
        }
    }
}
