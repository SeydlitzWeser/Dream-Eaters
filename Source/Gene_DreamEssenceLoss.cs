using RimWorld;
using Verse;
using System.Collections.Generic;

namespace DreamEaters
{
    public class Gene_DreamEssenceLoss : Gene, IGeneResourceDrain
    {
        private Gene_DreamEssence cachedDreamEssenceGene;

        private const float MinAgeForDrain = 13f;

        public bool CanOffset
        {
            get
            {
                return this.Active;
            }
        }
        public Gene_Resource Resource
        {
            get
            {
                if (cachedDreamEssenceGene == null || !cachedDreamEssenceGene.Active)
                {
                    cachedDreamEssenceGene = pawn.genes.GetFirstGeneOfType<Gene_DreamEssence>();
                }
                return cachedDreamEssenceGene;
            }
        }

        public float ResourceLossPerDay => def.resourceLossPerDay;

        public Pawn Pawn => pawn;

        public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";

        public override void Tick()
        {
            base.Tick();
            GeneResourceDrainUtility.TickResourceDrain(this);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!Active)
            {
                yield break;
            }
            foreach (Gizmo resourceDrainGizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
            {
                yield return resourceDrainGizmo;
            }
        }
    }
}
