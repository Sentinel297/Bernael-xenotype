using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bernael_Xenotype
{
    public class Gene_Nihilism : Gene
    {
        public bool canDoubleResistance = true;

        public override void Tick()
        {
            base.Tick();
            if (!pawn.IsHashIntervalTick(180)) return;

            if (!pawn.IsPrisoner && !canDoubleResistance)
                canDoubleResistance = true;

            if (!pawn.IsPrisoner || !canDoubleResistance) return;

            var pawnRes = pawn.guest;
            if (pawnRes != null)
                pawnRes.resistance *= 2;

            canDoubleResistance = false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref canDoubleResistance, "BX_CanDoubleResistance");
        }
    }
}
