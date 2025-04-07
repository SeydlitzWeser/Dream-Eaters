using System;
using RimWorld;
using Verse;

namespace DreamEaters
{
    public class CompAbilityEffect_Dreamweave : CompAbilityEffect
    {
        public new CompProperties_AbilityDreamweave Props =>
            (CompProperties_AbilityDreamweave)props;

        private bool IsPawnSleeping(Pawn pawn)
        {
            return pawn.CurJob != null
                && pawn.CurJob.def == JobDefOf.LayDown
                && pawn.jobs.curDriver.asleep;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn targetPawn = target.Pawn;

            if (targetPawn != null && IsPawnSleeping(targetPawn))
            {
                IntVec3 dropPos = targetPawn.PositionHeld;
                Thing candy = ThingMaker.MakeThing(DE_DefOf.HQC_DreamCandy);
                candy.stackCount = Props.candyAmount;
                GenPlace.TryPlaceThing(candy, dropPos, targetPawn.Map, ThingPlaceMode.Near);
            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest) => Valid(target);

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn targetPawn = target.Pawn;

            if (targetPawn == null)
            {
                if (throwMessages)
                    Messages.Message("MustTargetSleepingPawn".Translate(), MessageTypeDefOf.RejectInput);
                return false;
            }

            if (!IsPawnSleeping(targetPawn))
            {
                if (throwMessages)
                    Messages.Message("TargetMustBeSleeping".Translate(), MessageTypeDefOf.RejectInput);
                return false;
            }

            if (targetPawn.HostileTo(parent.pawn.Faction) && !targetPawn.Downed)
            {
                if (throwMessages)
                    Messages.Message("CannotExtractFromHostilePawns".Translate(), MessageTypeDefOf.RejectInput);
                return false;
            }

            return true;
        }

        public override string ExtraLabelMouseAttachment(LocalTargetInfo target) => null;

        public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction) => null;
    }
}
