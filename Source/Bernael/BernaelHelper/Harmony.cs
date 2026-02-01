using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using EBSGFramework;
using RimWorld;
using System.Reflection;
using JetBrains.Annotations;

namespace Bernael_Xenotype
{
    [StaticConstructorOnStartup]
    public static class BernaelGraceHarmony
    {
        static BernaelGraceHarmony()
        {
            Harmony harmony = new Harmony("BernaelXenotype.Harmony.SentinelPatches");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ResourceGene), nameof(ResourceGene.Tick))]
        public static class Patch_Grace
        {
            [UsedImplicitly]
            private static bool Prepare()
            {
                return ModsConfig.IsActive("Sov.Nephilim");
            }

            public static void Postfix(ref ResourceGene __instance)
            {
                Pawn pawn = __instance.pawn;
                if (!__instance.pawn.IsHashIntervalTick(180) || __instance.def != BernaelDefOf.GS_Grace_New || __instance.Resource.Value > 0 || !pawn.InMentalState) return;

                pawn.MorbOut(BernaelDefOf.BX_Bernael);
            }
        }

        [HarmonyPatch(typeof(IngestionOutcomeDoer), nameof(IngestionOutcomeDoer.DoIngestionOutcome))]
        public static class Patch_GraceIngestion
        {
            [UsedImplicitly]
            private static bool Prepare()
            {
                return ModsConfig.IsActive("Sov.Nephilim");
            }

            public static void Postfix(Pawn pawn, Thing ingested)
            {
                if (pawn.Map == null || ingested.def.GetGraceOutcomeDoer() == null) return;
                foreach (Pawn otherPawn in pawn.Map.mapPawns.AllPawnsSpawned)
                {
                    if (otherPawn.genes == null || otherPawn.relations == null) continue;
                    bool hasHateGene = otherPawn.genes.GetGene(BernaelDefOf.BX_Grace_Galling) != null;
                    if (!hasHateGene) continue;

                    ThoughtHandler memory = otherPawn.needs.mood.thoughts;
                    if (memory == null) continue;

                    TaleRecorder.RecordTale(BernaelDefOf.BX_ConsumedGrace, pawn, otherPawn);
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.TryInteractWith))]
        public static class Patch_WeirdSpeech
        {
            public static void Postfix(Pawn_InteractionsTracker __instance, ref bool __result, Pawn recipient)
            {
                if (!__result ) return;
                Pawn instigator = __instance.pawn;
                if (instigator.genes?.GetGene(BernaelDefOf.BX_DarkSpeech) == null) return;
                if (recipient.needs.mood.thoughts == null || recipient.genes == null || recipient.genes.GetGene(BernaelDefOf.BX_DarkSpeech) != null) return;
                recipient.needs.mood.thoughts.memories.TryGainMemory(BernaelDefOf.BX_HeardDarkSpeech);
            }
        }
    }
}
