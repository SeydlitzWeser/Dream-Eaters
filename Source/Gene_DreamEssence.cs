using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DreamEaters
{
    public class Gene_DreamEssence : Gene_Resource, IGeneResourceDrain
    {
        public bool dreamCandiesAllowed = true;

        public Gene_Resource Resource => this;

        public Pawn Pawn => pawn;

        public bool CanOffset => Active;

        public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";

        public float ResourceLossPerDay => def.resourceLossPerDay;

        public override float InitialResourceMax => 1f;

        public override float MinLevelForAlert => 0.15f;

        public override float MaxLevelOffset => 0.1f;

        protected override Color BarColor => new ColorInt(138, 3, 138).ToColor;

        protected override Color BarHighlightColor => new ColorInt(145, 42, 145).ToColor;

        public override void PostAdd()
        {
            if (ModLister.CheckBiotech("Dream Essence"))
            {
                base.PostAdd();
                Reset();
            }
        }

        public override void Tick()
        {
            base.Tick();
            GeneResourceDrainUtility.TickResourceDrain(this);
        }

        public override void SetTargetValuePct(float val)
        {
            targetValue = Mathf.Clamp(val * Max, 0f, Max - MaxLevelOffset);
        }

        public bool ShouldConsumeDreamEssenceNow()
        {
            return Value < targetValue;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!Active)
            {
                yield break;
            }
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            foreach (Gizmo resourceDrainGizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
            {
                yield return resourceDrainGizmo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref dreamCandiesAllowed, "dreamCandiesAllowed", defaultValue: true);
        }
    }
}
