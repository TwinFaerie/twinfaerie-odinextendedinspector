using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TF.OdinExtendedInspector.Editor
{
    internal static class TypeCollector
    {
        internal static readonly HashSet<string> _systemAssemblyNames = new() { "mscorlib", "System", "System.Core" };

        internal static IEnumerable<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        internal static bool IsSystemAssembly(Assembly assembly)
        {
            return IsSystemAssembly(assembly.GetName());
        }

        internal static bool IsSystemAssembly(AssemblyName assemblyName)
        {
            return _systemAssemblyNames.Any(x => 
            x == assemblyName.Name ||
            (x.Contains(".?") && HaveSameStringPattern(x.Remove(x.Length - 1), assemblyName.Name)));
        }
        
        internal static IEnumerable<Type> GetFilteredTypesFromAssemblies(IEnumerable<Assembly> assemblies, TypeOptionsAttribute filter)
        {
            return assemblies.Where(assembly => !IsSystemAssembly(assembly)).SelectMany(GetAllTypesFromAssembly).Where(type => type.IsVisible && FilterConstraintIsSatisfied(filter, type));
        }
        
        internal static IEnumerable<Type> GetFilteredTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.Where(assembly => !IsSystemAssembly(assembly)).SelectMany(GetAllTypesFromAssembly).Where(type => type.IsVisible && FilterConstraintIsSatisfied(type));
        }

        internal static IEnumerable<Type> GetFilteredTypesFromAssembly(Assembly assembly, TypeOptionsAttribute filter)
        {
            if (IsSystemAssembly(assembly))
            { return null; }
            
            var assemblyTypes = GetAllTypesFromAssembly(assembly);
            return assemblyTypes.Where(type => (!IsSystemAssembly(assembly) || type.IsVisible) && FilterConstraintIsSatisfied(filter, type));
        }

        internal static IEnumerable<Type> GetFilteredTypesFromAssembly(Assembly assembly)
        {
            if (IsSystemAssembly(assembly))
            { return null; }
            
            var assemblyTypes = GetAllTypesFromAssembly(assembly);
            return assemblyTypes.Where(type => (!IsSystemAssembly(assembly) || type.IsVisible) && FilterConstraintIsSatisfied(type));
        }

        internal static IEnumerable<Type> GetAllTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogError($"Types could not be extracted from assembly {assembly}: {e.Message}");
                return Enumerable.Empty<Type>();
            }
        }

        internal static bool FilterConstraintIsSatisfied(TypeOptionsAttribute filter, Type type)
        {
            if (type.FullName == null || type.FullName.Contains("<") || type.Name.Contains("<"))
                return false;

            return filter == null || filter.MatchesRequirements(type);
        }

        internal static bool FilterConstraintIsSatisfied(Type type)
        {
            if (type.FullName == null || type.FullName.Contains("<") || type.Name.Contains("<") || !type.IsPublic || !type.IsVisible || type.IsAbstract || type.IsInterface)
                return false;

            return true;
        }

        internal static Assembly TryLoadAssembly(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            { return null; }

            Assembly assembly = null;

            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError($"{assemblyName} was not found. It will not be added to dropdown.");
            }
            catch (FileLoadException)
            {
                Debug.LogError($"Failed to load {assemblyName}. It will not be added to dropdown.");
            }
            catch (BadImageFormatException)
            {
                Debug.LogError($"{assemblyName} is not a valid assembly. It will not be added to dropdown.");
            }

            return assembly;
        }

        private static bool HaveSameStringPattern(string source, string comparer)
        {
            if (source.Length > comparer.Length)
            { return false; }

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != comparer[i])
                { return false; }
            }

            return true;
        }
    }
}