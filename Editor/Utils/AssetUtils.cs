using System.IO;
using System.Linq;
using UnityEditor;

namespace TF.OdinExtendedInspector.Editor
{
    internal static class AssetUtils
    {
        public static void CreateDirectoryIfNeeded(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            
            if (Directory.Exists(directoryPath))
            {
                return;
            }
            
            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
        }

        public static void AutoCorrectPath(ref string path)
        {
            while (path.Last() == ' ')
            {
                path.Remove(path.Length - 1);
            }
            if (path.Last() != '/')
            {
                path += '/';
            }
        }
    }
}
