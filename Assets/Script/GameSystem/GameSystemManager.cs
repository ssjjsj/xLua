using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[XLua.LuaCallCSharp]
public class GameSystemManager : BehaviorBase
{
	private Dictionary<Type, GameSystemBase> _gameSystemMap;

	protected override void Awake()
	{
		base.Awake();
		_gameSystemMap = new Dictionary<Type, GameSystemBase>();
	}

	private void Update()
	{
		foreach (var system in _gameSystemMap.Values)
		{
			system.Update();
		}
	}

	protected override void OnDestroy()
	{
		foreach (var system in _gameSystemMap.Values)
		{
			system.Destroy();
		}
		_gameSystemMap.Clear();
	}
	public T AddGameSystem<T>() where T : GameSystemBase, new()
	{
		var s = new T();
		s.Init();
		_gameSystemMap[typeof(T)] = s;
		return s;
	}

	public GameSystemBase GetGameSystem(Type systemType)
	{
		GameSystemBase s;
		if (_gameSystemMap.TryGetValue(systemType, out s))
		{
			return s;
		}

		return null;
	}

	public T GetGameSystem<T>() where T : GameSystemBase
	{
		GameSystemBase s;
		if (_gameSystemMap.TryGetValue(typeof(T), out s))
		{
			return (T)s;
		}

		return null;
	}

	public void RemoveGameSystem<T>() where T : GameSystemBase
	{
		GameSystemBase s;
		if (_gameSystemMap.TryGetValue(typeof(T), out s))
		{
			s.Destroy();
			_gameSystemMap.Remove(typeof(T));
		}
	}
}
