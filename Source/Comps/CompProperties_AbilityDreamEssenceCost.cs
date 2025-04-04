using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DreamEaters
{
    public class CompProperties_AbilityDreamEssenceCost : CompProperties_AbilityEffect
    {
        public float dreamEssenceCost;

        public CompProperties_AbilityDreamEssenceCost()
        {
            compClass = typeof(CompAbilityEffect_DreamEssenceCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return (string)("AbilityDreamEssenceCost".Translate() + ": ") + Mathf.RoundToInt(dreamEssenceCost * 100f);
        }
    }
}