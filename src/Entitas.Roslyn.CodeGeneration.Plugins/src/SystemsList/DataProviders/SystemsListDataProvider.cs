using System;
using System.Collections.Generic;
using System.Linq;
using Jenny;
using Jenny.Plugins;
using DesperateDevs.Extensions;
using DesperateDevs.Roslyn;
using DesperateDevs.Serialization;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;
using Microsoft.CodeAnalysis;

namespace Entitas.Roslyn.CodeGeneration.Plugins; 

public class SystemsListDataProvider : IDataProvider, IConfigurable, ICachable
{
    public string Name => "SystemsList";
    public int Order => 0;
    public bool RunInDryMode => true;

    public Dictionary<string, string> DefaultProperties => _projectPathConfig.DefaultProperties;

    public Dictionary<string, object> ObjectCache { get; set; }

    readonly ProjectPathConfig _projectPathConfig = new ProjectPathConfig();
    readonly INamedTypeSymbol[] _types;

    Preferences _preferences;
    SystemsListDataProvider _systemsListDataProvider;

    public SystemsListDataProvider() : this(null) { }

    public SystemsListDataProvider(INamedTypeSymbol[] types)
    {
        _types = types;
    }

    public void Configure(Preferences preferences)
    {
        _preferences = preferences;
        _projectPathConfig.Configure(preferences);
    }

    public CodeGeneratorData[] GetData()
    {
        var types = _types ?? Jenny.Plugins.Roslyn.PluginUtil
            .GetCachedProjectParser(ObjectCache, _projectPathConfig.ProjectPath)
            .GetTypes();

        var componentInterface = typeof(IComponent).ToCompilableString();

        var systemsTypes = types
            .Where(type => type.AllInterfaces.Any(i => i.ToCompilableString() == componentInterface))
            .Where(type => !type.IsAbstract)
            .Where(type => type.GetAttribute<SystemsListAttribute>() != null)
            .ToArray();

        var systemsTypesLookup = systemsTypes.ToDictionary(
            type => type.ToCompilableString(),
            type => (Type[])type.GetAttribute<SystemsListAttribute>().ConstructorArguments[0].Value);

        _systemsListDataProvider = new SystemsListDataProvider(systemsTypes);
        _systemsListDataProvider.Configure(_preferences);

        return _systemsListDataProvider
            .GetData()
            .Where(data => !((ComponentData)data).GetTypeName().RemoveComponentSuffix().HasListenerSuffix())
            .Select(data => new SystemsListData(data) { types = systemsTypesLookup[((ComponentData)data).GetTypeName()] })
            .ToArray();
    }
}