using UnityEditor;
using UnityEngine;

public static class CreateItemDataMenu
{
    [MenuItem("Tools/Create Default ItemData")]
    public static void CreateItemDataAsset()
    {
        var asset = ScriptableObject.CreateInstance<ItemData>();
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Items/NewItemData.asset");
        AssetDatabase.SaveAssets();
    }
}
