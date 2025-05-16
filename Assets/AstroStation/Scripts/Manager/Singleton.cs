using UnityEngine;

// 싱글톤 : 씬에서 하나의 객체로만 사용될경우 이클래쓰를 상속받아서 사용
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static GameObject obj;
    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    if (obj == null)
                        obj = new GameObject(typeof(T).ToString());
                    _instance = obj.AddComponent<T>();
                    Debug.Log("Singleton " + typeof(T));
                    
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }
}
