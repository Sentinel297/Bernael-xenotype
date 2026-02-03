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
                Gene gene = pawn.genes.GenesListForReading[i];
                pawn.genes.RemoveGene(gene);
            }

            if (pawn.Map != null)
            {
                FleckMaker.Static(pawn.Position, pawn.Map, FleckDefOf.PsycastSkipInnerExit);
            }

            pawn.genes.SetXenotype(xenotypeDef);
        }

        public static IngestionOutcomeDoer_OffsetResource GetGraceOutcomeDoer(this ThingDef thingDef)
        {
            if (thingDef.ingestible == null || thingDef.ingestible.outcomeDoers.NullOrEmpty()) return null;
            IngestionOutcomeDoer_OffsetResource ingestionOutcome = null;
            foreach (IngestionOutcomeDoer ingestionOutcomeDoer in thingDef.ingestible.outcomeDoers)
            {
                if (ingestionOutcomeDoer is not IngestionOutcomeDoer_OffsetResource warden || warden.mainResourceGene != BernaelDefOf.GS_Grace_New) continue;
                ingestionOutcome = warden;
                break;
            }
            return ingestionOutcome;
        }

        public static Gamecomponent_PsychicSight GetGamePsychicSightComp(this Game game)
        {

            var comp = game.GetComponent<Gamecomponent_PsychicSight>();

            if (comp == null)
            {
                comp = new Gamecomponent_PsychicSight(game);
                game.components.Add(comp);
            }

            return comp;
        }
    }
}
