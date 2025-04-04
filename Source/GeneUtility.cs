using Verse;

namespace DreamEaters
{
    public static void OffsetDreamEsssence(Pawn pawn, float offset, bool applyStatFactor = true)
    {
        if (!ModsConfig.BiotechActive)
        {
            return;
        }
        if (offset > 0f && applyStatFactor)
        {
            offset *= pawn.GetStatValue(DE_DefOf.DreamEssenceGainFactor);
        }
        Gene_DreamEssenceLoss gene_DreamEssenceLoss = pawn.genes?.GetFirstGeneOfType<Gene_DreamEssenceLoss>();
        if (gene_DreamEssenceLoss != null)
        {
            GeneResourceDrainUtility.OffsetResource(gene_DreamEssenceLoss, offset);
            return;
        }
        Gene_DreamEssence gene_DreamEssence = pawn.genes?.GetFirstGeneOfType<Gene_DreamEssence>();
        if (gene_DreamEssence != null)
        {
            gene_DreamEssence.Value += offset;
        }
    }
}
