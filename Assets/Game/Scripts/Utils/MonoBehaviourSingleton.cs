using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static bool isShuttingDown = false; // Flag to check if we are quitting

    public static T Instance
    {
        get
        {
            // If the application is quitting, don't create a new instance. Just return null.
            if (isShuttingDown)
            {
                return null;
            }

            if (_instance == null)
            {
                // This logic is mostly the same as before
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    // Giving the auto-created object a name makes it easier to find in the hierarchy
                    obj.name = typeof(T).Name; 
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// This method is called by Unity when the application is quitting.
    /// We use it to set our flag.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        isShuttingDown = true;
    }
}