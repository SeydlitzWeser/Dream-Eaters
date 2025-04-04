using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DreamEaters
{
    public class GeneGizmo_ResourceDreamEssence : GeneGizmo_Resource
    {
        private static readonly Texture2D DreamEssenceCostTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

        private const float TotalPulsateTime = 0.85f;

        private List<Pair<IGeneResourceDrain, float>> tmpDrainGenes = new List<Pair<IGeneResourceDrain, float>>();

        public GeneGizmo_ResourceDreamEssence(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barHighlightColor)
            : base(gene, drainGenes, barColor, barHighlightColor)
        {
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
            float num = Mathf.Repeat(Time.time, 0.85f);
            float num2 = 1f;
            if (num < 0.1f)
            {
                num2 = num / 0.1f;
            }
            else if (num >= 0.25f)
            {
                num2 = 1f - (num - 0.25f) / 0.6f;
            }
            if (((MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow)?.LastMouseoverGizmo is Command_Ability command_Ability && gene.Max != 0f)
            {
                foreach (CompAbilityEffect effectComp in command_Ability.Ability.EffectComps)
                {
                    if (effectComp is CompAbilityEffect_DreamEssenceCost compAbilityEffect_DreamEssenceCost && compAbilityEffect_DreamEssenceCost.Props.dreamessenceCost > float.Epsilon)
                    {
                        Rect rect = barRect.ContractedBy(3f);
                        float width = rect.width;
                        float num3 = gene.Value / gene.Max;
                        rect.xMax = rect.xMin + width * num3;
                        float num4 = Mathf.Min(compAbilityEffect_DreamEssenceCost.Props.dreamessenceCost / gene.Max, 1f);
                        rect.xMin = Mathf.Max(rect.xMin, rect.xMax - width * num4);
                        GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
                        GenUI.DrawTextureWithMaterial(rect, DreamEssenceCostTex, null);
                        GUI.color = Color.white;
                        break;
                    }
                }
            }
            return result;
        }

        protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
        {
            Gene_DreamEssence dreamessenceGene;
            if (IsDraggable && (dreamessenceGene = gene as Gene_DreamEssence) != null)
            {
                headerRect.xMax -= 24f;
                Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
                Widgets.DefIcon(rect, DE_DefOf.HQC_DreamCandy, null, 1f, null, drawPlaceholder: false, null, null, null);
                GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), dreamessenceGene.dreamCandiesAllowed ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
                if (Widgets.ButtonInvisible(rect))
                {
                    dreamessenceGene.dreamCandiesAllowed = !dreamessenceGene.dreamCandiesAllowed;
                    if (dreamessenceGene.dreamCandiesAllowed)
                    {
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                    else
                    {
                        SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                    }
                }
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    string onOff = (dreamessenceGene.dreamCandiesAllowed ? "On" : "Off").Translate().ToString().UncapitalizeFirst();
                    TooltipHandler.TipRegion(rect, () => "AutoTakeDreamEssenceDesc".Translate(gene.pawn.Named("PAWN"), dreamessenceGene.PostProcessValue(dreamessenceGene.targetValue).Named("MIN"), onOff.Named("ONOFF")).Resolve(), 828267373);
                    mouseOverElement = true;
                }
            }
            base.DrawHeader(headerRect, ref mouseOverElement);
        }

        protected override string GetTooltip()
        {
            tmpDrainGenes.Clear();
            string text = $"{gene.ResourceLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor)}: {gene.ValueForDisplay} / {gene.MaxForDisplay}\n";
            if (gene.pawn.IsColonistPlayerControlled || gene.pawn.IsPrisonerOfColony)
            {
                text = ((!(gene.targetValue <= 0f)) ? (text + (string)("ConsumeDreamEssenceBelow".Translate() + ": ") + gene.PostProcessValue(gene.targetValue)) : (text + "NeverConsumeDreamEssence".Translate().ToString()));
            }
            if (!drainGenes.NullOrEmpty())
            {
                float num = 0f;
                foreach (IGeneResourceDrain drainGene in drainGenes)
                {
                    if (drainGene.CanOffset)
                    {
                        tmpDrainGenes.Add(new Pair<IGeneResourceDrain, float>(drainGene, drainGene.ResourceLossPerDay));
                        num += drainGene.ResourceLossPerDay;
                    }
                }
                if (num != 0f)
                {
                    string text2 = ((num < 0f) ? "RegenerationRate".Translate() : "DrainRate".Translate());
                    text = text + "\n\n" + text2 + ": " + "PerDay".Translate(Mathf.Abs(gene.PostProcessValue(num))).Resolve();
                    foreach (Pair<IGeneResourceDrain, float> tmpDrainGene in tmpDrainGenes)
                    {
                        text = text + "\n  - " + tmpDrainGene.First.DisplayLabel.CapitalizeFirst() + ": " + "PerDay".Translate(gene.PostProcessValue(0f - tmpDrainGene.Second).ToStringWithSign()).Resolve();
                    }
                }
            }
            if (!gene.def.resourceDescription.NullOrEmpty())
            {
                text = text + "\n\n" + gene.def.resourceDescription.Formatted(gene.pawn.Named("PAWN")).Resolve();
            }
            return text;
        }
    }
}