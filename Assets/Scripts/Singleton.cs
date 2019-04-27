using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : class {

	static private T _instance;

	static public T Instance
	{
		get
		{
			return _instance;
		}
		protected set
		{
			_instance = value;
		}
	}

	protected virtual void Awake()
	{
		Debug.Log(name + " Singelton Awake()");

		if(_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else{
			_instance = this as T;
			this.OnAwake();
		}
	}

	protected abstract void OnAwake();
}
