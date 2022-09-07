using Dalamud.Data;
using Dalamud.IoC;
using Dalamud.Plugin;


namespace WondrousTailsList;

internal class Service
{
    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static DataManager DataManager { get; private set; } = null!;

}