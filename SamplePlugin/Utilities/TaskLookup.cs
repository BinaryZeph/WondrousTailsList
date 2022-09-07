using System.Collections.Generic;
using System.Linq;
using WondrousTailsList.DataStructures;
using Dalamud.Logging;
using Lumina.Excel.GeneratedSheets;

namespace WondrousTailsList.Utilities;

internal static class TaskLookup
{
    public static dynamic GetInstanceDetailsFromID(uint id)
    {
        var name = GetInstanceNameFromID(id);
        return name;
    }

    private static string GetInstanceNameFromID(uint id)
    {
        //Get instance ID from Wondrous Tails ID
        var instanceContentData = Service.DataManager.GetExcelSheet<WeeklyBingoOrderData>()
                !.GetRow(id)
                !.Data;

        if (instanceContentData > 20000)
        {
            var name = Service.DataManager.GetExcelSheet<ContentFinderCondition>()
                .Where(r => r.Content == instanceContentData)
                .Select(r => r.TerritoryType.Value)
                .Select(r => r!.PlaceName.Value!.Name.ToString())
                .FirstOrDefault();

            return name;
        }

        switch (id)
        {
            case 1:
                return "Dungeons lvl 1-49";
            case 2:
                return "Dungeons lvl 50";
            case 3:
                return "Dungeons lvl 51-59";
            case 4:
                return "Dungeons lvl 60";
            case 59:
                return "Dungeons lvl 61-69";
            case 60:
                return "Dungeons lvl 70";
            case 85:
                return "Dungeons lvl 71-79";
            case 86:
                return "Dungeons lvl 80";
            case 108:
                return "Dungeons lvl 81-89";
            case 109:
                return "Dungeons lvl 90";
            case 53:
                return "POTD/HOH";
            case 46:
                return "Portal Maps";
            case 121:
                return "The Binding Coil of Bahamut";
            case 122:
                return "The Second Coil of Bahamut";
            case 123:
                return "The Final Coil of Bahamut";
            case 124:
                return "Alexander: Gordias";
            case 125:
                return "Alexander: Midas";
            case 126:
                return "Alexander: The Creator";
            case 127:
                return "Omega: Deltascape";
            case 128:
                return "Omega: Sigmascape";
            case 129:
                return "Omega: Alphascape";
            case 67:
                return "PVP: Rival Wings";
            default:
                return id.ToString();
        }
    }

    private static uint? TryGetFromDatabase(uint id)
    {
        var instanceContentData = Service.DataManager.GetExcelSheet<WeeklyBingoOrderData>()
            !.GetRow(id)
            !.Data;

        if (instanceContentData < 20000)
        {
            return null;
        }

        var data = Service.DataManager.GetExcelSheet<ContentFinderCondition>()
            !.Where(c => c.Content == instanceContentData)
            .Select(c => c.TerritoryType.Value!.RowId)
            .FirstOrDefault();

        return data;
    }
}