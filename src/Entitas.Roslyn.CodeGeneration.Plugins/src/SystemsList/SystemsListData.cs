using System;
using Jenny;

namespace Entitas.Roslyn.CodeGeneration.Plugins; 

public class SystemsListData : CodeGeneratorData
{
    public const string TYPES_LIST = "Types.List";

    public Type[] types
    {
        get => (Type[])this[TYPES_LIST];
        init => this[TYPES_LIST] = value;
    }
    
    public SystemsListData(CodeGeneratorData data) : base(data) { }
}