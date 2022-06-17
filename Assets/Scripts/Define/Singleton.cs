using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
	private readonly static T _instance = new T();

	public static T Instance
	{
		get
		{
			return _instance;
		}
	}

	public abstract bool Init();
}