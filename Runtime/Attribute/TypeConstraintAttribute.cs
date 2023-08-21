using Sirenix.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    public enum TypeConstraintGrouping
    {
        None,
        ByNamespace,
        ByNamespaceFlat
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TypeConstraintAttribute : TypeOptionsAttribute
    {
        protected Type[] BaseTypes;
        public bool IncludeBaseType = false;

        public TypeConstraintAttribute(Type[] baseTypes)
        {
            BaseTypes = baseTypes;
            Grouping = TypeConstraintGrouping.None;
        }

        public TypeConstraintAttribute(Type baseType, params Type[] additionalBaseTypes)
        {
            if (additionalBaseTypes == null || additionalBaseTypes.Length == 0)
            {
                BaseTypes = new[] { baseType };
            }
            else
            {
                BaseTypes = new Type[additionalBaseTypes.Length + 1];
                additionalBaseTypes.CopyTo(BaseTypes, 0);
                BaseTypes[additionalBaseTypes.Length] = baseType;
            }

            Grouping = TypeConstraintGrouping.None;
        }

        public override bool MatchesRequirements(Type type)
        {
            if (BaseTypes.Contains(type) && !IncludeBaseType)
            { return false; }

            if (BaseTypes.Contains(type))
            { return true; }

            return BaseTypes.All(type.InheritsFrom) && base.MatchesRequirements(type);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TypeOptionsAttribute : PropertyAttribute
    {
        public TypeConstraintGrouping Grouping = TypeConstraintGrouping.ByNamespace;
        public string[] Assemblies;
        public Type[] IncludeTypes;
        public Type[] ExcludeTypes;
        public bool AllowHidden = false;
        public bool HideNamespace = false;

        public virtual bool MatchesRequirements(Type type)
        {
            bool passesVisibleFilter = AllowHidden ||
            (
                type.IsVisible &&
                type.IsPublic &&
                !type.IsAbstract &&
                !type.IsInterface
            );

            bool passesExcludedFilter = !ExcludeTypes?.Contains(type) ?? true;

            return passesVisibleFilter
                && passesExcludedFilter;
        }
    }
}