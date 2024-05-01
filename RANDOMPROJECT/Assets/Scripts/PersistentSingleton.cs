using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();

				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					instance = gameObject.AddComponent<T>();
					DontDestroyOnLoad(gameObject);
				}
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}