using System.Collection.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DreamEaters
{
    public class CompProperties_AbilityDreamEssenceCost : CompProperties_AbilityEffect
    {
        public float dreamessenceCost;

        public CompProperties_AbilityDreamEssenceCost()
        {
            compClass = typeof(CompAbilityEffect_DreamEssenceCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return (string)("AbilityDreamEssenceCost".Translate() + ": ") + Mathf.RoundToInt(dreamessenceCost * 100f);
        }
    }
}