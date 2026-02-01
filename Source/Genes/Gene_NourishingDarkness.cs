using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bernael_Xenotype
{
    public class Gene_AddHediff : DefModExtension
    {
       public List<HediffDef> hediffsToAdd = new List<HediffDef>();
    }

    public class Gene_Hediff : Gene
    {
        Gene_AddHediff ModExt => def.GetModExtension<Gene_AddHediff>();

        public override void PostAdd()
        {
            base.PostAdd();
            AddHediffs();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode != LoadSaveMode.PostLoadInit) return;
            AddHediffs();
        }

        public void AddHediffs()
        {
            if (pawn.health == null || pawn.health.hediffSet == null) return;
            foreach (HediffDef hediffDef in ModExt.hediffsToAdd)
            {
                pawn.health.GetOrAddHediff(hediffDef);
            }
        }

        public override void PostRemove()
        {
            base.PostRemove();
            if (pawn.health == null || pawn.health.hediffSet == null) return;
            foreach (HediffDef hediffDef in ModExt.hediffsToAdd)
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                if (hediff == null) continue;
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
