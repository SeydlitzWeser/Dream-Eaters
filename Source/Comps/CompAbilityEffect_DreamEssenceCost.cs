using RimWorld;
using Verse;

namespace DreamEaters
{
    public class CompAbilityEffect_DreamEssenceCost : CompAbilityEffect
    {
        public new CompProperties_AbilityDreamEssenceCost Props => (CompProperties_AbilityDreamEssenceCost)props;

        private bool HasEnoughDreamEssence
        {
            get
            {
                Gene_DreamEssence gene_DreamEssence = parent.pawn.genes?.GetFirstGeneOfType<Gene_DreamEssence>();
                if (gene_DreamEssence == null || gene_DreamEssence.Value < Props.dreamEssenceCost)
                {
                    return false;
                }
                return true;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            DreamGeneUtility.OffsetDreamEsssence(parent.pawn, 0f - Props.dreamEssenceCost);
        }

        public override bool GizmoDisabled(out string reason)
        {
            Gene_DreamEssence gene_DreamEssence = parent.pawn.genes?.GetFirstGeneOfType<Gene_DreamEssence>();
            if (gene_DreamEssence == null)
            {
                reason = "AbilityDisabledNoDreamEssenceGene".Translate(parent.pawn);
                return true;
            }
            if (gene_DreamEssence.Value < Props.dreamEssenceCost)
            {
                reason = "AbilityDisabledNoDreamEssence".Translate(parent.pawn);
                return true;
            }
            float num = TotalDreamEssenceCostOfQueuedAbilities();
            float num2 = Props.dreamEssenceCost + num;
            if (Props.dreamEssenceCost > float.Epsilon && num2 > gene_DreamEssence.Value)
            {
                reason = "AbilityDisabledNoDreamEssence".Translate(parent.pawn);
                return true;
            }
            reason = null;
            return false;
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return HasEnoughDreamEssence;
        }

        private float TotalDreamEssenceCostOfQueuedAbilities()
        {
            float num = ((parent.pawn.jobs?.curJob?.verbToUse is not Verb_CastAbilityDream verb_CastAbility) ? 0f : (verb_CastAbility.ability?.DreamEssenceCost() ?? 0f));
            if (parent.pawn.jobs != null)
            {
                for (int i = 0; i < parent.pawn.jobs.jobQueue.Count; i++)
                {
                    if (parent.pawn.jobs.jobQueue[i].job.verbToUse is Verb_CastAbilityDream verb_CastAbility2)
                    {
                        num += verb_CastAbility2.ability?.DreamEssenceCost() ?? 0f;
                    }
                }
            }
            return num;
        }
    }
}