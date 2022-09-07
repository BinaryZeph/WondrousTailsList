using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Utility.Signatures;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.GeneratedSheets;
using WondrousTailsList.Utilities;

namespace WondrousTailsList.DataStructures
{
    internal unsafe class WondrousTailsBook
    {
        private delegate int WondrousTailsGetDeadlineDelegate(int* deadlineIndex);

        [Signature("88 05 ?? ?? ?? ?? 8B 43 18", ScanType = ScanType.StaticAddress)]
        private readonly WondrousTailsStruct* wondrousTails = null;

        [Signature("8B 81 ?? ?? ?? ?? C1 E8 04 25")]
        private readonly WondrousTailsGetDeadlineDelegate wondrousTailsGetDeadline = null!;

        [Signature("48 8D 0D ?? ?? ?? ?? 48 89 BD ?? ?? ?? ?? E8 ?? ?? ?? ?? 44 8B C0", ScanType = ScanType.StaticAddress)]
        private readonly int* wondrousTailsDeadlineIndex = null;

        private const int WondrousTailsBookItemID = 2002023;

        public WondrousTailsBook()
        {
            SignatureHelper.Initialise(this);
        }

        public int GetNumStickers() => wondrousTails->Stickers;
        public int GetNumSecondChance() => wondrousTails->SecondChance;
        public bool PlayerHasBook() => InventoryManager.Instance()->GetInventoryItemCount(WondrousTailsBookItemID) > 0;
        public bool NewBookAvailable() => DateTime.Now > GetDeadline() - TimeSpan.FromDays(7);
        public DateTime GetDeadline() => DateTimeOffset.FromUnixTimeSeconds(wondrousTailsGetDeadline(wondrousTailsDeadlineIndex)).ToLocalTime().DateTime;

        public IEnumerable<WondrousTailsTask> GetAllTaskData()
        {
            var result = new List<WondrousTailsTask>();

            for (var i = 0; i < 16; ++i)
            {
                var TaskState = wondrousTails->TaskStatus(i);
                var TaskName = TaskLookup.GetInstanceDetailsFromID(wondrousTails->Tasks[i]);
                PluginLog.Information($"Task: {string.Join(", ", TaskName)}");

                 result.Add(new WondrousTailsTask(TaskState, TaskName));
            }

            return result;
        }
    }
}
