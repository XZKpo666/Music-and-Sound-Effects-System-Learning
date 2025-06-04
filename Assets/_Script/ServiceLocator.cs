using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; private set; }

    private readonly Dictionary<Type, IGameService> _serviceDictionary = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterService<T>(T service, bool isAbstract = true) where T : IGameService
    {
        if (isAbstract)
        {
            Type baseType = typeof(T).BaseType;
            if(baseType != null)
            _serviceDictionary.Add(baseType, service);
            else Debug.Log($"Service {typeof(T).Name} does not have a base type to register.");
        }
        else
        {
            _serviceDictionary.Add(typeof(T), service);
        }
    }

    public void RemoveService<T>(bool isAbstract = true) where T : IGameService
    {
        if (isAbstract)
        {
            Type baseType = typeof(T).BaseType;
            if(baseType != null)
            _serviceDictionary.Remove(baseType);
            else Debug.Log($"Service {typeof(T).Name} does not have a base type to remove.");
        }
        else
        {
            _serviceDictionary.Remove(typeof(T));
        }
    }

    public T GetService<T>() where T : IGameService
    {
        if (_serviceDictionary.TryGetValue(typeof(T), out IGameService service))
        {
            service = _serviceDictionary[typeof(T)];
            return (T)service;
        }
        Debug.Log($"Service of type {typeof(T)} not found!");
        return default;
    }

    public bool TryGetService<T>(out T service) where T : IGameService
    {
        service = default;
        if (_serviceDictionary.ContainsKey(typeof(T)))
        {
            service = (T)_serviceDictionary[typeof(T)];
            return true;
        }
  
        return false;
    }

}
