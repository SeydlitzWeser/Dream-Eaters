using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamEaters
{
    public class AbilityDream : Ability
    {
        public float DreamEssenceCost()
        {
            if (this.comps != null)
            {
                using List<AbilityComp>.Enumerator enumerator = this.comps.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    CompAbilityEffect_DreamEssenceCost compAbilityEffect_DreamEssenceCost;
                    if ((compAbilityEffect_DreamEssenceCost = (enumerator.Current as CompAbilityEffect_DreamEssenceCost)) != null)
                    {
                        return compAbilityEffect_DreamEssenceCost.Props.dreamEssenceCost;
                    }
                }
            }
            return 0f;
        }
    }
}
