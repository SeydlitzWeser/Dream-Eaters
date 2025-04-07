using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DreamEaters
{
    public class CompProperties_AbilityDreamweave : CompProperties_AbilityEffect
    {
        public int candyAmount = 5;

        public CompProperties_AbilityDreamweave()
        {
            compClass = typeof(CompAbilityEffect_Dreamweave);
        }
    }
}