﻿using UnityEngine;
using Verse;

namespace GaussWeapons
{
    public class Projectile_GaussGrenade : Projectile
    {
        private int ticksToDetonation;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
        }
        public override void Tick()
        {
            base.Tick();
            if (this.ticksToDetonation > 0)
            {
                this.ticksToDetonation--;
                if (this.ticksToDetonation <= 0)
                {
                    this.Explode();
                }
            }
        }
        protected override void Impact(Thing hitThing)
        {
            if (this.def.projectile.explosionDelay == 0)
            {
                this.Explode();
                return;
            }
            this.landed = true;
            this.ticksToDetonation = this.def.projectile.explosionDelay;
        }
        protected virtual void Explode()
        {
            this.Destroy(DestroyMode.Vanish);
            GenExplosion.DoExplosion(base.Position, this.def.projectile.explosionRadius, this.def.projectile.damageDef, this.launcher, this.def.projectile.soundExplode, this.def, this.equipmentDef, this.def.projectile.postExplosionSpawnThingDef, this.def.projectile.explosionSpawnChance, false);
            for (int i = 0; i < 4; i++)
            {
                ThrowSmokeBlue(Position.ToVector3Shifted() + Gen.RandomHorizontalVector(this.def.projectile.explosionRadius * 0.7f), this.def.projectile.explosionRadius * 0.6f);
                ThrowMicroSparksBlue(Position.ToVector3Shifted() + Gen.RandomHorizontalVector(this.def.projectile.explosionRadius * 0.7f));
            }
        }
        public static void ThrowSmokeBlue(Vector3 loc, float size)
        {
            if (!loc.ShouldSpawnMotesAt() || MoteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("Mote_SmokeBlue"), null);
            moteThrown.ScaleUniform = Rand.Range(1.5f, 2.5f) * size;
            moteThrown.exactRotationRate = Rand.Range(-0.5f, 0.5f);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocityAngleSpeed((float)Rand.Range(30, 40), Rand.Range(0.008f, 0.012f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3());
        }
        public static void ThrowMicroSparksBlue(Vector3 loc)
        {
            if (!loc.ShouldSpawnMotesAt() || MoteCounter.Saturated)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("Mote_MicroSparksBlue"), null);
            moteThrown.ScaleUniform = Rand.Range(0.8f, 1.2f);
            moteThrown.exactRotationRate = Rand.Range(-0.2f, 0.2f);
            moteThrown.exactPosition = loc;
            moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
            moteThrown.SetVelocityAngleSpeed((float)Rand.Range(35, 45), Rand.Range(0.02f, 0.02f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3());
        }
    }
}
