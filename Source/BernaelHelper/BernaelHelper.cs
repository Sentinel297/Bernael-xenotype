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
    public static class BernaelUtility
    {

        public static void MorbOut(this Pawn pawn, XenotypeDef xenotypeDef)
        {
            for (int i = pawn.genes.GenesListForReading.Count - 1; i >= 0; i--)
            {
                var gene = pawn.genes.GenesListForReading[i];
                pawn.genes.RemoveGene(gene);
            }

            if (pawn.Map != null)
                FleckMaker.Static(pawn.Position, pawn.Map, FleckDefOf.PsycastSkipInnerExit);

            pawn.genes.SetXenotype(xenotypeDef);
        }

        public static IngestionOutcomeDoer_OffsetResource GetGraceOutcomeDoer(this ThingDef thingDef)
        {
            if (thingDef.ingestible == null || thingDef.ingestible.outcomeDoers.NullOrEmpty()) return null;
            IngestionOutcomeDoer_OffsetResource ingestionOutcome = null;
            foreach (IngestionOutcomeDoer ingestionOutcomeDoer in thingDef.ingestible.outcomeDoers)
            {
                IngestionOutcomeDoer_OffsetResource warden = ingestionOutcomeDoer as IngestionOutcomeDoer_OffsetResource;
                if (warden == null || warden.mainResourceGene != BernaelDefOf.GS_Grace_New) continue;
                ingestionOutcome = (IngestionOutcomeDoer_OffsetResource)ingestionOutcomeDoer;
                break;
            }
            return ingestionOutcome;
        }
    }
}
