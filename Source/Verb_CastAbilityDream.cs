using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace DreamEaters
{
    public class Verb_CastAbilityDream : Verb_CastAbility
    {
        public new AbilityDream Ability
        {
            get
            {
                return this.ability;
            }
            set
            {
                this.ability = value;
            }
        }
        public new static Color RadiusHighlightColor
        {
            get
            {
                return new Color(0.3f, 0.8f, 1f);
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<AbilityDream>(ref this.ability, "ability", false);
        }

        public new AbilityDream ability;
    }
}
