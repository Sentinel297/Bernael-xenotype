using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
namespace Bernael_Xenotype
{
    public class Utility
    {
        public static void OffsetSoul(Pawn pawn, float offset, bool applyStatFactor = true)
        {
            if (!ModsConfig.BiotechActive)
            {
                return;
            }
            if (offset > 0f && applyStatFactor)
            {
                offset *= pawn.GetStatValue(BernaelDefOf.BX_SoulGainFactor);
            }
            Pawn_GeneTracker genes = pawn.genes;
            Gene_SoulDrain gene_SoulDrain = genes?.GetFirstGeneOfType<Gene_SoulDrain>();
            if (gene_SoulDrain != null)
            {
                GeneResourceDrainUtility.OffsetResource(gene_SoulDrain, offset);
                return;
            }
            Pawn_GeneTracker genes2 = pawn.genes;
            Gene_Soul gene_Soul = genes2?.GetFirstGeneOfType<Gene_Soul>();
            if (gene_Soul != null)
            {
                gene_Soul.Value += offset;
            }
        }

        public static void DoDrain(Pawn biter, Pawn victim, float targetSoulGain, float victimResistanceGain, ThoughtDef thoughtDefToGiveTarget = null, ThoughtDef opinionThoughtToGiveTarget = null)
        {
            if (!ModLister.CheckBiotech("Sanguophage bite"))
            {
                return;
            }
            float num2 = targetSoulGain * victim.BodySize;
            OffsetSoul(biter, num2);
            OffsetSoul(victim, -num2);
            Pawn_NeedsTracker needs = biter.needs;
            if (thoughtDefToGiveTarget != null)
            {
                Pawn_NeedsTracker needs2 = victim.needs;
                if (needs2 != null)
                {
                    Need_Mood mood = needs2.mood;
                    if (mood != null)
                    {
                        ThoughtHandler thoughts = mood.thoughts;
                        if (thoughts != null)
                        {
                            MemoryThoughtHandler memories = thoughts.memories;
                            if (memories != null)
                            {
                                memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(thoughtDefToGiveTarget), biter);
                            }
                        }
                    }
                }
            }
            if (opinionThoughtToGiveTarget != null)
            {
                Pawn_NeedsTracker needs3 = victim.needs;
                Need_Mood mood2 = needs3?.mood;
                ThoughtHandler thoughts2 = mood2?.thoughts;
                MemoryThoughtHandler memories2 = thoughts2?.memories;
                memories2?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(opinionThoughtToGiveTarget), biter);
            }
            //if (targetBloodLoss > 0f)
            //{
            //    victim.health.AddHediff(HediffDefOf.BloodfeederMark, ExecutionUtility.ExecuteCutPart(victim), null, null);
            //    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, victim, null);
            //    hediff.Severity = targetBloodLoss;
            //    victim.health.AddHediff(hediff, null, null, null);
            //}
            if (victim.IsPrisoner && victimResistanceGain > 0f)
            {
                victim.guest.resistance = Mathf.Min(victim.guest.resistance + victimResistanceGain, victim.kindDef.initialResistanceRange.Value.TrueMax);
            }
        }
        public static void ConvertBaby(Pawn drainer, Pawn victim)
        {
            DevelopmentalStage? developmentStage = victim.ageTracker?.CurLifeStage?.developmentalStage;
            if (developmentStage is not (DevelopmentalStage.Baby or DevelopmentalStage.Newborn)) return;

            XenotypeDef targetXenotype = drainer.genes.xenotype;
            victim.genes.xenotypeName = drainer.genes.xenotypeName;
            victim.genes.iconDef = drainer.genes.iconDef;
            victim.genes.SetXenotypeDirect(targetXenotype);
            victim.genes.ClearXenogenes();
            foreach (GeneDef gene in targetXenotype.genes)
            {
                victim.genes.AddGene(gene, xenogene: true);
            }

            List<DirectPawnRelation> relationList = victim.relations.directRelations;
            foreach (DirectPawnRelation relations in relationList)
            {
                if (relations.def.familyByBloodRelation)
                {
                    MemoryThoughtHandler memories = relations.otherPawn.needs.mood.thoughts.memories;
                    ThoughtDef thoughtDef = relations.def.GetGenderSpecificDiedThought(victim);
                    if (thoughtDef != null)
                    {
                        memories.RemoveMemoriesOfDefWhereOtherPawnIs(thoughtDef, victim);
                    }
                    victim.relations.directRelations.Remove(relations);
                }
            }
            victim.relations.AddDirectRelation(PawnRelationDefOf.Parent, drainer);
        }
    }
}
