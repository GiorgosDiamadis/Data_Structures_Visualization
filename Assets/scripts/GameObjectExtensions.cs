using UnityEngine;

public static class GameObjectExtensions
{
    public static void Destroy_Object(this GameObject queried)
    {
        queried.transform.SetParent(null);
        UnityEngine.Object.Destroy(queried);
    }
}
