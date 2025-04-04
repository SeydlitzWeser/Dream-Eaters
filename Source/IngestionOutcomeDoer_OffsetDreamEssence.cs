using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DreamEaters
{
    public class IngestionOutcomeDoer_OffsetDreamEssence : IngestionOutcomeDoer
    {
        public float offset;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            DreamGeneUtility.OffsetDreamEsssence(pawn, offset * (float)ingestedCount);
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (ModsConfig.BiotechActive)
            {
                string text = ((offset >= 0f) ? "+" : string.Empty);
                yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "DreamEssence".Translate().CapitalizeFirst(), text + Mathf.RoundToInt(offset * 100f), "DreamEssenceDesc".Translate(), 1000);
            }
        }
    }
}