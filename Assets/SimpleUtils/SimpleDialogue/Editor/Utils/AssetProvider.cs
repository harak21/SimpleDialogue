using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleDialogue.Editor.Utils
{
    internal static class AssetProvider
    {
        private static readonly Dictionary<string, string> Paths = new();
        
        public static T LoadAssetAtAssetName<T>(string assetName) where T : Object
        {
            if (Paths.TryGetValue(assetName, out string savedPath))
            {
                return AssetDatabase.LoadAssetAtPath<T>(savedPath);
            }
            
            var assets = AssetDatabase.FindAssets(assetName);

            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var split = path.Split('/');
                var lastSplit = split[^1];

                var extension = lastSplit.Split(".").Last();
                if (extension.Equals("cs"))
                    continue;

                var fileName = lastSplit.Replace($".{extension}", "");
                if (fileName == assetName)
                {
                    Paths[assetName] = path;
                    return AssetDatabase.LoadAssetAtPath<T>(path);
                }
            }

            throw new FileNotFoundException();
        }

        public static T FindSingleAsset<T>() where T : Object
        {
            var name = typeof(T).Name;
            var assetGuid = AssetDatabase.FindAssets($"t: {name}").First();
            if (string.IsNullOrEmpty(assetGuid))
                throw new FileNotFoundException();

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static List<T> FindAllAssets<T>() where T : Object
        {
            var name = typeof(T).Name;
            var guids = AssetDatabase.FindAssets($"t: {name}");
            if (guids.Length == 0)
                throw new FileNotFoundException();

            var assets = new List<T>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                assets.Add(AssetDatabase.LoadAssetAtPath<T>(assetPath));
            }

            return assets;
        }
    }
}