using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public static class CustomGameOptionsManager
{
    public static readonly List<CustomGameOption> CustomOptions = new ();
    
    public static readonly List<CustomNumberOption> CustomNumberOptions = new();

    public static readonly List<CustomToggleOption> CustomToggleOptions = new();
    
    public static NumberOption NumberOptionPrefab { get; set; }
    public static ToggleOption ToggleOptionPrefab { get; set; }
    public static StringOption StringOptionPrefab { get; set; }


    public static void RegisterOptions<T>()
    {
        var instance = Activator.CreateInstance<T>();
        foreach (var property in typeof(T).GetProperties())
        {
            var toggleAttribute = property.GetCustomAttribute<ToggleOptionAttribute>();
            if (toggleAttribute is not null && property.PropertyType == typeof(bool))
            {
                RegisterToggleOption(instance,property,toggleAttribute);
                continue;
            }

            var numberAttribute = property.GetCustomAttribute<NumberOptionAttribute>();
            if (numberAttribute is not null && (property.PropertyType == typeof(int)|| property.PropertyType == typeof(float)))
            {
                RegisterNumberOption(instance, property, numberAttribute);
                continue;
            }

        }
    }

    public static CustomToggleOption RegisterToggleOption<T>(T instance, PropertyInfo property, ToggleOptionAttribute attribute)
    {
        var setMethod = property.GetSetMethod();
                
        var parameterT = Expression.Parameter(typeof(T), "targetType");
        var parameterTProperty = Expression.Parameter(typeof(bool), "targetProperty");
        
        var newExpression = Expression.Lambda(Expression.Call(parameterT, setMethod, parameterTProperty), parameterT, parameterTProperty);

        return new CustomToggleOption(instance, attribute.Name, newExpression.Compile());
    }

    public static CustomNumberOption RegisterNumberOption<T>(T instance, PropertyInfo property, NumberOptionAttribute attribute)
    {
        var setMethod = property.GetSetMethod();
                
        var parameterT = Expression.Parameter(typeof(T), "targetType");
        var parameterTProperty = Expression.Parameter(typeof(float), "targetProperty");
        
        var newExpression = Expression.Lambda(Expression.Call(parameterT, setMethod, parameterTProperty), parameterT, parameterTProperty);

        return new CustomNumberOption(instance, newExpression.Compile(), attribute);
    }
    

    public static void RpcSyncOptions()
    {
        var toggles = CustomToggleOptions.Select(x => x.Value).ToArray();
        var numbers = CustomNumberOptions.Select(x => x.Value).ToArray();
        Rpc<SyncCustomGameOptionsRpc>.Instance.Send(new SyncCustomGameOptionsRpc.Data(toggles, numbers));
    }
    
    public static void ReadSyncOptions(bool[] toggles, float[] numbers)
    {
        for (var i = 0; i < toggles.Length; i++)
        {
            CustomToggleOptions[i].SetValue(toggles[i]);
        }
        for (var i = 0; i < numbers.Length; i++)
        {
            CustomNumberOptions[i].SetValue(numbers[i]);
        }
    }
    
    public static CustomNumberOption CreateNumberOption(string title, float defaultValue, float minValue, float maxValue, float increment, NumberSuffixes suffixType)
    {
        return CustomNumberOptions.FirstOrDefault(x => x.Title == title) ?? new CustomNumberOption(title, defaultValue, minValue, maxValue, increment, suffixType);
    }

    public static CustomToggleOption CreateToggleOption(string title, bool defaultValue)
    {
        return CustomToggleOptions.FirstOrDefault(x => x.Title == title) ?? new CustomToggleOption(title, defaultValue);
    }
    
}