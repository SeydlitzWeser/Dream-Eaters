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

        protected override Color BarColor
        {
            get
            {
                float fillPercent = Value / Max;

                Color color100 = new Color(0.976f, 0.945f, 0.941f); // #f9f1f0
                Color color75 = new Color(0.980f, 0.863f, 0.851f);  // #fadcd9
                Color color50 = new Color(0.973f, 0.686f, 0.651f);  // #f8afa6
                Color color25 = new Color(0.969f, 0.580f, 0.537f);  // #f79489

                if (fillPercent >= 0.75f)
                {
                    return Color.Lerp(color75, color100, (fillPercent - 0.75f) / 0.25f);
                }
                else if (fillPercent >= 0.5f)
                {
                    return Color.Lerp(color50, color75, (fillPercent - 0.5f) / 0.25f);
                }
                else if (fillPercent >= 0.25f)
                {
                    return Color.Lerp(color25, color50, (fillPercent - 0.25f) / 0.25f);
                }
                else
                {
                    return color25;
                }
            }
        }

        protected override Color BarHighlightColor
        {
            get
            {
                Color baseColor = BarColor;
                return new Color(
                    Mathf.Min(baseColor.r * 1.15f, 1f),
                    Mathf.Min(baseColor.g * 1.15f, 1f),
                    Mathf.Min(baseColor.b * 1.15f, 1f)
                );
            }
        }

        public override void PostAdd()
        {
            if (ModLister.CheckBiotech("DreamEssence"))
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
