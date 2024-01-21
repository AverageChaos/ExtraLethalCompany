using System.Collections.Generic;
using System.Linq;

namespace ExtraLethalCompany.Helpers
{
    internal readonly struct HarmonyParams(object instance, object result, object[] args, string[] paramNames)
    {
        private readonly object _Instance = instance;
        internal T Instance<T>() => (T)_Instance;

        private readonly object _Result = result;
        internal T Result<T>() => (T)_Result;

        private readonly Dictionary<string, object> _Args = Enumerable.Range(0, args.Length).ToDictionary(i => paramNames[i], i => args[i]);
        internal T Arg<T>(string name)
        { 
            _Args.TryGetValue(name, out object val); 
            return (T) val; 
        }
    }
}
