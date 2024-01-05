using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    public class PathInfo : ScriptableObject
    {
        [ReadOnly] [ShowInInspector]
        public TypeRef type;
        
        [FolderPath]
        public string pathName;

        public Action<string, string, string> onSetPath;

        [Button("Set Path")]
        private void SetPath()
        {
            AssetUtils.AutoCorrectPath(ref pathName);
            onSetPath?.Invoke(AssetDatabase.GetAssetPath(this), pathName + "_Info/" + name + ".asset", pathName);
        }
    }
}
