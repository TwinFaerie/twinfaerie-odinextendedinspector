using System;
using UnityEngine;

namespace TF.OdinExtendedInspector
{
    [Serializable]
    public class TypeRef : ISerializationCallbackReceiver
    {
        [SerializeField] private string typeNameAndAssembly;
        private Type type;

        public Type Type
        {
            get
            {
                if (type == null && !string.IsNullOrEmpty(typeNameAndAssembly))
                    type = Type.GetType(typeNameAndAssembly);

                return type;
            }
        }

        public TypeRef(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(typeNameAndAssembly))
            {
                type = null;
                return;
            }

            type = Type.GetType(typeNameAndAssembly);
        }

        public void OnBeforeSerialize()
        {
            typeNameAndAssembly = type?.AssemblyQualifiedName;
        }

        public override string ToString()
        {
            if (Type == null) return "NULL";
            return Type.Name;
        }

        public static implicit operator Type(TypeRef t) => t?.Type;
        public static implicit operator TypeRef(Type t) => new(t);
    }
}