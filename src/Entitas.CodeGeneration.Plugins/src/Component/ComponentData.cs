using System;
using Jenny;
using DesperateDevs.Extensions;

namespace Entitas.CodeGeneration.Plugins
{
    public class ComponentData : CodeGeneratorData
    {
        public ComponentData() { }

        public ComponentData(CodeGeneratorData data) : base(data) { }
    }

    public static class ComponentDataExtension
    {
        public static string ToComponentName(this string fullTypeName, bool ignoreNamespaces) => ignoreNamespaces
            ? fullTypeName.MyTypeName().RemoveComponentSuffix()
            : fullTypeName.RemoveDots().RemoveComponentSuffix();
        
        public static string MyTypeName(this string fullTypeName)
        {
            int startIndex = fullTypeName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            return fullTypeName.Substring(startIndex, fullTypeName.Length - startIndex);
        }
    }
}
