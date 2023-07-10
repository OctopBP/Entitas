using System;

namespace Entitas.CodeGeneration.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemsListAttribute : Attribute
    {
        public readonly Type[] types;

        public SystemsListAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}