using System.IO;
using System.Linq;
using Jenny;
using Entitas.CodeGeneration.Plugins;

namespace Entitas.Roslyn.CodeGeneration.Plugins; 

public class SystemsListGenerator : AbstractGenerator
{
    public override string Name => "Cleanup (System)";

    const string ADD_SYSTEM_LINE_TEMPLATE = "    Add(new ${SystemName}(contexts));";

    public override CodeGenFile[] Generate(CodeGeneratorData[] data) => data
        .OfType<SystemsListData>()
        .Select(generate)
        .ToArray();

    const string SYSTEMS_TEMPLATE =
        @"public partial class ${SystemsName} : Feature
{
  public ${SystemsName}(Contexts contexts) : base(nameof(${SystemsName}))
  {
${systemLists}
  }
}
";

    CodeGenFile generate(SystemsListData data) {
        var typeName = GetType().FullName;
        var systemLists = string.Join(
            "\n",
            data.types.Select(type => ADD_SYSTEM_LINE_TEMPLATE.Replace("${SystemName}", type.Name))
        );

        var fileContent = SYSTEMS_TEMPLATE
            .Replace("${SystemsName}", typeName)
            .Replace("${systemLists}", systemLists);

        return new CodeGenFile(
            "SystemsList" + Path.DirectorySeparatorChar +
            typeName + ".cs",
            fileContent,
            typeName
        );
    }
}