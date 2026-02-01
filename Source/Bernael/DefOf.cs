using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Bernael_Xenotype
{
    [DefOf]
    public class BernaelDefOf
    {
        public BernaelDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BernaelDefOf));
        }

        public static GeneDef BX_Grace_Galling;
        public static GeneDef BX_GracePendulum;
        public static GeneDef BX_DarkSpeech;
        public static TaleDef BX_ConsumedGrace;
        public static XenotypeDef BX_Bernael;
        public static HediffDef BX_Mutation_Hediff;
        public static ThoughtDef BX_ConsumedGraceThoughtMood;
        public static ThoughtDef BX_HeardDarkSpeech;

        public static StatDef BX_SoulGainFactor;
        public static GeneDef BX_SoulStarved;

        public static ThingDef BX_BottledSoul;
        public static RecipeDef BX_ExtractSoul;

        [MayRequire("Sov.Nephilim")]
        public static GeneDef GS_Grace_New;
        [MayRequire("Sov.Nephilim")]
        public static XenotypeDef GS_Nephilim;
    }
}
