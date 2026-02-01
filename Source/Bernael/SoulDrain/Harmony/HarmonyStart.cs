using HarmonyLib;
using Verse;
namespace Bernael_Xenotype
{
    [StaticConstructorOnStartup]
    public class HarmonyStart
    {
        static HarmonyStart()
        {
            Harmony harmony = new Harmony(id: "bernael");
            harmony.PatchAll();
            Log.Message("Bernael: Harmony patches applied");
        }
    }
}
