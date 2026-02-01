using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSGFramework;
using RimWorld;
using Verse;

namespace Bernael_Xenotype
{
    public class Gene_Pendulum : Gene
    {
        public override void Notify_IngestedThing(Thing thing, int numTaken)
        {
            base.Notify_IngestedThing(thing, numTaken);
            if (!ModsConfig.IsActive("Sov.Nephilim")) return;

            IngestionOutcomeDoer_OffsetResource outcomeDoer_OffsetResource = thing.def.GetGraceOutcomeDoer();
            if (outcomeDoer_OffsetResource == null) return;

            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(BernaelDefOf.BX_Mutation_Hediff);
            if (hediff != null) return;

            Hediff_BernaelToNephilim Mutation = (Hediff_BernaelToNephilim)HediffMaker.MakeHediff(BernaelDefOf.BX_Mutation_Hediff, pawn);
            pawn.health.AddHediff(Mutation);

            Mutation.daysTillTransform -= 1;
            Mutation.cooldown = 60000;
            Mutation.available = false;

            var memory = pawn.needs.mood.thoughts.memories;
            if (memory != null)
                memory.TryGainMemory(BernaelDefOf.BX_ConsumedGraceThoughtMood);

            if (pawn.Map != null)
                FleckMaker.Static(pawn.DrawPos, pawn.Map, FleckDefOf.PsycastAreaEffect);
        }
    }
}
