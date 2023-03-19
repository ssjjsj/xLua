using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[XLua.LuaCallCSharp]
public class Singleton<T> where T : new()
{
	private static T singleton;

	public Singleton()
	{

	}

	public static T GetInstance()
	{
		if (singleton == null)
			singleton = new T();
		return singleton;
	}
}

[XLua.LuaCallCSharp]
public class BehaviorBase : MonoBehaviour
{
	private static BehaviorBase singleton;

	public static BehaviorBase GetInstance()
	{
		return singleton;
	}
	
	protected virtual void Awake()
	{
		if (singleton != null)
			Debug.LogError(string.Format("singleton behavior {0} already add one", name));
		singleton = this;
	}

	protected virtual void OnDestroy()
	{

	}
}
