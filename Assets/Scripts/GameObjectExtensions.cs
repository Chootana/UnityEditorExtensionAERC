using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// 深い階層まで子オブジェクトを名前で検索して GameObject 型で取得します
    /// </summary>
    /// <param name="self">GameObject 型のインスタンス</param>
    /// <param name="name">検索するオブジェクトの名前</param>
    /// <param name="includeInactive">非アクティブなオブジェクトも検索する場合 true</param>
    /// <returns>子オブジェクト</returns>

    public static GameObject FindDeep(this GameObject self, string name, bool includeInactive = false)
    {
        Transform[] children = self.GetComponentsInChildren<Transform>(includeInactive);
        foreach (Transform transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }
}

