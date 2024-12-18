﻿
using System;
using System.Linq;
using System.Numerics;
using GW2EIEvtcParser.EIData;
using GW2EIEvtcParser.Exceptions;
using GW2EIEvtcParser.ParsedData;
using GW2EIEvtcParser.ParserHelpers;
using static GW2EIEvtcParser.EncounterLogic.EncounterCategory;
using static GW2EIEvtcParser.EncounterLogic.EncounterImages;
using static GW2EIEvtcParser.EncounterLogic.EncounterLogicPhaseUtils;
using static GW2EIEvtcParser.ParserHelper;
using static GW2EIEvtcParser.SkillIDs;

namespace GW2EIEvtcParser.EncounterLogic;

internal class GreerTheBlightbringer : MountBalrior
{
    private readonly long[] ReflectableProjectiles = [BlobOfBlight, BlobOfBlight2, ScatteringSporeblast, RainOfSpores];
    public GreerTheBlightbringer(int triggerID) : base(triggerID)
    {
        MechanicList.AddRange(new List<Mechanic>()
        {
            new PlayerSrcHitMechanic(ReflectableProjectiles, "Reflected Projectiles", new MechanicPlotlySetting(Symbols.YDown, Colors.Pink), "ProjRefl.Greer.H", "Reflected projectiles have hit Greer", "Reflected Projectile Hit (Greer)", 0).UsingChecker((hde, log) => hde.To.IsSpecies(ArcDPSEnums.TargetID.Greer)),
            new PlayerSrcHitMechanic(ReflectableProjectiles, "Reflected Projectiles", new MechanicPlotlySetting(Symbols.YDown, Colors.Purple), "ProjRefl.Reeg.H", "Reflected projectiles have hit Reeg", "Reflected Projectile Hit (Reeg)", 0).UsingChecker((hde, log) => hde.To.IsSpecies(ArcDPSEnums.TrashID.Reeg)),
            new PlayerSrcHitMechanic(ReflectableProjectiles, "Reflected Projectiles", new MechanicPlotlySetting(Symbols.YDown, Colors.LightPurple), "ProjRefl.Gree.H", "Reflected projectiles have hit Gree", "Reflected Projectile Hit (Gree)", 0).UsingChecker((hde, log) => hde.To.IsSpecies(ArcDPSEnums.TrashID.Gree)),
            new PlayerDstHitMechanic(RotTheWorld, "Rot the World", new MechanicPlotlySetting(Symbols.Star, Colors.Teal), "RotWorld.H", "Hit by Rot the World (Breakbar AoEs)", "Rot the World Hit", 0),
            new PlayerDstHitMechanic(WaveOfCorruption, "Wave of Corruption", new MechanicPlotlySetting(Symbols.HourglassOpen, Colors.LightRed), "WaveCor.H", "Hit by Wave of Corruption", "Wave of Corruption Hit", 0),
            new PlayerDstHitMechanic([RipplesOfRot, RipplesOfRot2], "Ripples of Rot", new MechanicPlotlySetting(Symbols.StarSquareOpenDot, Colors.Chocolate), "RippRot.H", "Hit by Ripples of Rot", "Ripples of Rot Hit", 0),
            new PlayerDstHitMechanic([RainOfSpores, RainOfSpores2], "Rain of Spores", new MechanicPlotlySetting(Symbols.Hexagon, Colors.Green), "RainSpore.H", "Hit by Rain of Spores", "Rain of Spores Hit", 0),
            new PlayerDstHitMechanic([EnfeeblingMiasma, EnfeeblingMiasma2], "Enfeebling Miasma", new MechanicPlotlySetting(Symbols.TriangleDown, Colors.LightPurple), "EnfMiasma.H", "Hit by Enfeebling Miasma", "Enfeebling Miasma Hit", 0),
            new PlayerDstHitMechanic([ScatteringSporeblast, ScatteringSporeblast2], "Scattering Sporeblast", new MechanicPlotlySetting(Symbols.SquareOpen, Colors.GreenishYellow), "ScatSpore.H", "Hit by Scattering Sporeblast", "Scattering Sporeblast Hit", 0),
            new PlayerDstHitMechanic([AuraOfCorruptionDamage, AuraOfCorruptionDamage2], "Aura of Corruption", new MechanicPlotlySetting(Symbols.CircleOpenDot, Colors.Purple), "AuraCorr.H", "Hit by Aura of Corruption (Hitbox)", "Aura of Corruption Hit", 0),
            new PlayerDstHitMechanic([RakeTheRot, RakeTheRot2, RakeTheRot3], "Rake the Rot", new MechanicPlotlySetting(Symbols.PentagonOpen, Colors.LightBlue), "Rake.H", "Hit by Rake the Rot", "Rake the Rot Hit", 0),
            new PlayerDstHitMechanic([CageOfDecay, CageOfDecay2, CageOfDecay3], "Cage of Decay", new MechanicPlotlySetting(Symbols.Hourglass, Colors.LightPurple), "Cage.H", "Hit by Cage of Decay", "Cage of Decay Hit", 0),
            new PlayerDstHitMechanic([SweepTheMold, SweepTheMold2, SweepTheMold3], "Sweep the Mold", new MechanicPlotlySetting(Symbols.PentagonOpen, Colors.Blue), "Sweep.H", "Hit by Sweep the Mold", "Sweep the Mold Hit", 0),
            new PlayerDstHitMechanic([BlobOfBlight, BlobOfBlight2, BlobOfBlight3], "Blob of Blight", new MechanicPlotlySetting(Symbols.Star, Colors.CobaltBlue), "BlobBlight.H", "Hit by Blob of Blight", "Blob of Blight Hit", 0),
            new PlayerDstHitMechanic([EruptionOfRot, EruptionOfRot2, EruptionOfRot3], "Eruption of Rot", new MechanicPlotlySetting(Symbols.Hexagram, Colors.GreenishYellow), "ErupRot.H", "Hit by Eruption of Rot", "Eruption of Rot Hit", 0),
            new PlayerDstHitMechanic([StompTheGrowth, StompTheGrowth2, StompTheGrowth3], "Stomp the Growth", new MechanicPlotlySetting(Symbols.CircleOpen, Colors.LightOrange), "Stomp.H", "Hit by Stomp the Growth", "Stomp the Growth Hit", 0),
            new PlayerDstBuffApplyMechanic(TargetBuff, "Target", new MechanicPlotlySetting(Symbols.CircleXOpen, Colors.LightBlue), "BlobBlight.T", "Targeted by Blob of Blight", "Blob of Blight Target", 0),
            new PlayerDstBuffApplyMechanic(InfectiousRotBuff, "Infectious Rot", new MechanicPlotlySetting(Symbols.CircleX, Colors.Red), "InfRot.T", "Targeted by Infectious Rot (Hit by Eruption of Rot)", "Infectious Rot Target", 0),
            new PlayerDstEffectMechanic([EffectGUIDs.GreerNoxiousBlightGreen, EffectGUIDs.GreerNoxiousBlightGreen2], "Noxious Blight", new MechanicPlotlySetting(Symbols.Circle, Colors.Green), "NoxBlight.T", "Targeted by Noxious Blight (Green)", "Noxious Blight (Green)", 0),
            new EnemyDstBuffApplyMechanic(EmpoweredGreer, "Empowered", new MechanicPlotlySetting(Symbols.YUp, Colors.Red), "Empowered", "Gained Empowered", "Empowered", 0),
        });
        Extension = "greer";
        Icon = EncounterIconGreer;
        EncounterCategoryInformation.InSubCategoryOrder = 0;
        EncounterID |= 0x000001;
    }
    protected override CombatReplayMap GetCombatMapInternal(ParsedEvtcLog log)
    {
        return new CombatReplayMap(CombatReplayGreerTheBlightbringer,
                        (1912, 1845),
                        (11300, -10621, 18374, -3794));
    }
    protected override ReadOnlySpan<int> GetTargetsIDs()
    {
        return
        [
            (int)ArcDPSEnums.TargetID.Greer,
            (int)ArcDPSEnums.TrashID.Gree,
            (int)ArcDPSEnums.TrashID.Reeg,
        ];
    }

    protected override ReadOnlySpan<int> GetUniqueNPCIDs()
    {
        return
        [
            (int)ArcDPSEnums.TargetID.Greer,
            (int)ArcDPSEnums.TrashID.Gree,
            (int)ArcDPSEnums.TrashID.Reeg,
        ];
    }

    protected override Dictionary<int, int> GetTargetsSortIDs()
    {
        return new Dictionary<int, int>()
        {
            {(int)ArcDPSEnums.TargetID.Greer, 0 },
            {(int)ArcDPSEnums.TrashID.Gree, 1 },
            {(int)ArcDPSEnums.TrashID.Reeg, 1 },
        };
    }

    protected override List<ArcDPSEnums.TrashID> GetTrashMobsIDs()
    {
        return
        [
            ArcDPSEnums.TrashID.EmpoweringBeast,
        ];
    }


    internal override List<PhaseData> GetPhases(ParsedEvtcLog log, bool requirePhases)
    {
        List<PhaseData> phases = GetInitialPhase(log);
        SingleActor greer = Targets.FirstOrDefault(x => x.IsSpecies(ArcDPSEnums.TargetID.Greer)) ?? throw new MissingKeyActorsException("Greer not found");
        phases[0].AddTarget(greer);
        var subTitanIDs = new List<int>
        {
            (int) ArcDPSEnums.TrashID.Reeg,
            (int) ArcDPSEnums.TrashID.Gree,
        };
        var subTitans = Targets.Where(x => x.IsAnySpecies(subTitanIDs));
        phases[0].AddSecondaryTargets(subTitans);
        if (!requirePhases)
        {
            return phases;
        }
        // Invul check
        phases.AddRange(GetPhasesByCast(log, InvulnerableBarrier, greer, true, true));
        for (int i = 1; i < phases.Count; i++)
        {
            PhaseData phase = phases[i];
            if (i % 2 == 0)
            {
                phase.Name = "Split " + (i) / 2;
                phase.AddTargets(subTitans);
            }
            else
            {
                phase.Name = "Phase " + (i + 1) / 2;
                phase.AddTarget(greer);
                phase.AddSecondaryTargets(subTitans);
            }
        }
        return phases;
    }

    internal override void ComputeNPCCombatReplayActors(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        base.ComputeNPCCombatReplayActors(target, log, replay);

        switch (target.ID)
        {
            case (int)ArcDPSEnums.TargetID.Greer:
                AddSweepTheMoldRakeTheRot(target, log, replay, 750);
                AddStompTheGrowth(target, log, replay, 800);
                AddScatteringSporeblast(target, log, replay);
                AddEnfeeblingMiasma(target, log, replay);
                AddRainOfSpores(target, log, replay);
                AddRipplesOfRot(target, log, replay);
                AddBlobOfBlight(target, log, replay);
                AddCageOfDecayOrEruptionOfRot(target, log, replay);

                // Getting breakbar times to filter some effects of different sizes appearing at the end of it.
                var breakbars = target.GetBuffStatus(log, DamageImmunity, log.FightData.FightStart, log.FightData.FightEnd).Where(x => x.Value > 0);
                foreach (var breakbar in breakbars)
                {
                    // Rot the World
                    if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRotTheWorld, out var rotTheWorld))
                    {
                        foreach (EffectEvent effect in rotTheWorld.Where(x => x.Time >= breakbar.Start && x.Time <= breakbar.End))
                        {
                            (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                            replay.Decorations.AddWithBorder(new CircleDecoration(240, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)), Colors.LightOrange, 0.2);
                        }
                    }
                }

                // Invulnerable Barrier
                if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerInvulnerableBarrier, out var invulnerableBarriers))
                {
                    foreach (EffectEvent effect in invulnerableBarriers)
                    {
                        (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                        replay.Decorations.AddWithBorder(new CircleDecoration(600, lifespan, Colors.GreenishYellow, 0.2, new AgentConnector(target)), Colors.GreenishYellow, 0.4);
                    }
                }
                break;
            case (int)ArcDPSEnums.TrashID.Reeg:
                AddScatteringSporeblast(target, log, replay);
                AddRainOfSpores(target, log, replay);
                AddBlobOfBlight(target, log, replay);
                AddCageOfDecayOrEruptionOfRot(target, log, replay);
                break;
            case (int)ArcDPSEnums.TrashID.Gree:
                AddSweepTheMoldRakeTheRot(target, log, replay, 550);
                AddStompTheGrowth(target, log, replay, 600);
                AddRipplesOfRot(target, log, replay);
                AddEnfeeblingMiasma(target, log, replay);
                AddCageOfDecayOrEruptionOfRot(target, log, replay);
                break;
            case (int)ArcDPSEnums.TrashID.EmpoweringBeast:
                // Blighting Stab - Indicator
                if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerBlightingStabIndicator, out var blightingStabIndicator))
                {
                    foreach (EffectEvent effect in blightingStabIndicator)
                    {
                        // Duration too long by 500ms, use damage effect as end time
                        (long start, long end) lifespan = effect.ComputeLifespanWithSecondaryEffectAndPosition(log, EffectGUIDs.GreerBlightingStabDamage);
                        replay.Decorations.Add(new CircleDecoration(300, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)));
                    }
                }

                // Blighting Stab - Damage
                if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerBlightingStabDamage, out var blightingStabDamage))
                {
                    foreach (EffectEvent effect in blightingStabDamage)
                    {
                        (long start, long end) lifespan = effect.ComputeLifespan(log, 1000);
                        replay.Decorations.Add(new CircleDecoration(300, lifespan, Colors.GreenishYellow, 0.2, new PositionConnector(effect.Position)));
                    }
                }
                break;
            default:
                break;
        }
    }

    internal override void ComputePlayerCombatReplayActors(PlayerActor player, ParsedEvtcLog log, CombatReplay replay)
    {
        base.ComputePlayerCombatReplayActors(player, log, replay);

        // Noxious Blight - Green AoE
        if (log.CombatData.TryGetEffectEventsByDstWithGUIDs(player.AgentItem, [EffectGUIDs.GreerNoxiousBlightGreen, EffectGUIDs.GreerNoxiousBlightGreen2], out var noxiousBlight))
        {
            foreach (EffectEvent effect in noxiousBlight)
            {
                long duration = effect.GUIDEvent.ContentGUID == EffectGUIDs.GreerNoxiousBlightGreen ? 10000 : 8000;
                long growing = effect.Time + duration;
                (long start, long end) lifespan = effect.ComputeLifespan(log, duration);
                var circle = new CircleDecoration(240, lifespan, Colors.DarkGreen, 0.1, new AgentConnector(player));
                replay.Decorations.AddWithGrowing(circle, growing, true);
            }
        }

        // Infectious Rot - Failed Green AoE
        var infectiousRot = player.GetBuffStatus(log, InfectiousRotBuff, log.FightData.LogStart, log.FightData.LogEnd).Where(x => x.Value > 0);
        foreach (var segment in infectiousRot)
        {
            replay.Decorations.Add(new CircleDecoration(200, segment.TimeSpan, Colors.Red, 0.2, new AgentConnector(player)));
        }

    }

    internal override void ComputeEnvironmentCombatReplayDecorations(ParsedEvtcLog log)
    {
        base.ComputeEnvironmentCombatReplayDecorations(log);

        // Wave of Corruption
        if (log.CombatData.TryGetEffectEventsByGUID(EffectGUIDs.GreerWaveOfCorruption1, out var shockwaves))
        {
            foreach (EffectEvent effect in shockwaves)
            {
                (long start, long end) lifespan = (effect.Time, effect.Time + 3000);
                EnvironmentDecorations.AddShockwave(new PositionConnector(effect.Position), lifespan, Colors.Purple, 0.2, 1500);
            }
        }
    }

    private static void AddScatteringSporeblast(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Scattering Sporeblast - Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerScatteringSporeblastIndicator, out var sporeblasts))
        {
            foreach (EffectEvent effect in sporeblasts)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var indicator = new CircleDecoration(100, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position));
                replay.Decorations.Add(indicator);
            }
        }
    }

    private static void AddSweepTheMoldRakeTheRot(NPC target, ParsedEvtcLog log, CombatReplay replay, uint radius)
    {
        // Swepp the Mold / Rake the Rot - Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerSweepTheMoldRakeTheRotIndicator, out var indicators))
        {
            foreach (EffectEvent effect in indicators)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var cone = new PieDecoration(radius, 120, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z + 90));
                replay.Decorations.Add(cone);
            }
        }
    }

    private static void AddEnfeeblingMiasma(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Enfeebling Miasma - Cone Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerEnfeeblingMiasma, out var miasmaIndicator))
        {
            foreach (EffectEvent effect in miasmaIndicator)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 6000);
                var cone = new PieDecoration(2000, 60, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z + 90));
                replay.Decorations.Add(cone);
            }
        }

        // Enfeebling Miasma - Gas Circles
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerEnfeeblingMiasmaGasMoving, out var miasmaAnimation) &&
            log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerEnfeeblingMiasmaGasClouds, out var miasmaClouds))
        {
            foreach (EffectEvent animation in miasmaAnimation)
            {
                foreach (EffectEvent cloud in miasmaClouds.Where(x => x.Time > animation.Time && x.Time < animation.Time + 6000))
                {
                    (long start, long end) lifespan = cloud.ComputeLifespan(log, 12000);
                    var circle = new CircleDecoration(150, lifespan, Colors.Purple, 0.2, new PositionConnector(cloud.Position));
                    replay.Decorations.Add(circle);
                    replay.Decorations.AddProjectile(animation.Position, cloud.Position, (animation.Time, cloud.Time), Colors.Purple, 0.2, 150);
                }
            }
        }
    }

    private static void AddCageOfDecayOrEruptionOfRot(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Cage of Decay - Arrow Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayArrowIndicator, out var cageOfDecayArrows))
        {
            foreach (EffectEvent effect in cageOfDecayArrows)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 5000);
                var offset = new Vector3(700, 0, 0);
                var arrow = new RectangleDecoration(1400, 50, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position).WithOffset(offset, true)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z - 90));
                replay.Decorations.Add(arrow);
            }
        }

        // Cage of Decay - Circle Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayCircleIndicator, out var cageOfDecayCirclesIndicators))
        {
            foreach (EffectEvent effect in cageOfDecayCirclesIndicators)
            {
                long duration = 5000;
                long growing = effect.Time + duration;
                (long start, long end) lifespan = effect.ComputeLifespan(log, duration);
                var circle = new CircleDecoration(360, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position));
                replay.Decorations.AddWithGrowing(circle, growing);
            }
        }

        // Cage of Decay + Eruption of Rot - Roots
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayRoots, out var cageOfDecayRoots))
        {
            foreach (EffectEvent effect in cageOfDecayRoots)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 2000);
                var roots = (RectangleDecoration)new RectangleDecoration(50, 150, lifespan, Colors.GreenishYellow, 0.3, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z));
                replay.Decorations.AddWithBorder(roots, Colors.Purple, 0.2);
            }
        }

        // Cage of Decay + Eruption of Rot - Circle Damage
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayCircleDamage, out var cageOfDecayCirclesDamage))
        {
            foreach (EffectEvent effect in cageOfDecayCirclesDamage)
            {
                // Durations: Cage of Decay - 23000 | Eruption of Rot - 8000
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var circle = new CircleDecoration(360, lifespan, Colors.LightPurple, 0.3, new PositionConnector(effect.Position));
                replay.Decorations.AddWithBorder(circle, Colors.GreenishYellow, 0.3);
            }
        }

        // Cage of Decay - Moving roots walls
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayMovingWalls, out var cageOfDecayWalls))
        {
            foreach (EffectEvent effect in cageOfDecayWalls)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 2000);
                var wall = (RectangleDecoration)new RectangleDecoration(100, 50, lifespan, Colors.GreenishYellow, 0.3, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z));
                replay.Decorations.AddWithBorder(wall, Colors.Purple, 0.2);
            }
        }

        // Cage of Decay - Roots walls around the circle of damage
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerCageOfDecayCircleWalls, out var cageOfDecayCircleWalls))
        {
            foreach (EffectEvent effect in cageOfDecayCircleWalls)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 23000);
                var wall = (RectangleDecoration)new RectangleDecoration(200, 100, lifespan, Colors.GreenishYellow, 0.3, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z));
                replay.Decorations.AddWithBorder(wall, Colors.Purple, 0.2);
            }
        }
    }

    private static void AddRainOfSpores(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Rain of Spores - Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRainOfSporesIndicator, out var rainOfSpores))
        {
            foreach (EffectEvent effect in rainOfSpores)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                replay.Decorations.Add(new CircleDecoration(200, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)));
            }
        }
    }

    private static void AddStompTheGrowth(NPC target, ParsedEvtcLog log, CombatReplay replay, uint radius)
    {
        // Stomp the Growth - Circle indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerStompTheGrowth, out var stompTheGrowth))
        {
            foreach (EffectEvent effect in stompTheGrowth)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var circle = new CircleDecoration(radius, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position));
                replay.Decorations.AddWithGrowing(circle, effect.Time + effect.Duration);
            }
        }
    }

    private static void AddRipplesOfRot(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Ripples of Rot - Inner Circle
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRipplesOfRotIndicator1, out var ripplesOfRotIndicator1))
        {
            foreach (EffectEvent effect in ripplesOfRotIndicator1)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var innerCircle = new CircleDecoration(240, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position));
                replay.Decorations.AddWithGrowing(innerCircle, effect.Time + effect.Duration);
            }
        }

        // Ripples of Rot - Outer Circle
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRipplesOfRotIndicator2, out var ripplesOfRotIndicator2))
        {
            foreach (EffectEvent effect in ripplesOfRotIndicator2)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, effect.Duration);
                var outerCircle = new CircleDecoration(800, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position));
                replay.Decorations.AddWithBorder(outerCircle, Colors.LightOrange, 0.2);
            }
        }

        // Ripples of Rot - Moving roots walls
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRipplesOfRotMovingWalls, out var movingWalls))
        {
            foreach (EffectEvent effect in movingWalls)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 2000);
                var wall = (RectangleDecoration)new RectangleDecoration(100, 50, lifespan, Colors.GreenishYellow, 0.3, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z));
                replay.Decorations.AddWithBorder(wall, Colors.Purple, 0.2);
            }
        }

        // Ripples of Rot - Roots Walls
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerRipplesOfRotWalls, out var walls))
        {
            foreach (EffectEvent effect in walls)
            {
                (long start, long end) lifespan = effect.ComputeLifespan(log, 23000);
                var wall = (RectangleDecoration)new RectangleDecoration(200, 100, lifespan, Colors.GreenishYellow, 0.3, new PositionConnector(effect.Position)).UsingRotationConnector(new AngleConnector(effect.Rotation.Z));
                replay.Decorations.AddWithBorder(wall, Colors.Purple, 0.2);
            }
        }
    }

    private static void AddBlobOfBlight(NPC target, ParsedEvtcLog log, CombatReplay replay)
    {
        // Blob of Blight - AoE Indicator
        if (log.CombatData.TryGetEffectEventsBySrcWithGUID(target.AgentItem, EffectGUIDs.GreerBlobOfBlightIndicator, out var blobOfBlightIndicator))
        {
            foreach (EffectEvent effect in blobOfBlightIndicator)
            {
                // The effect has 0 duration for some reason
                (long start, long end) lifespan = effect.ComputeLifespanWithSecondaryEffectAndPosition(log, EffectGUIDs.GreerBlobofBlight, 1000);
                replay.Decorations.AddWithGrowing(new CircleDecoration(300, lifespan, Colors.LightOrange, 0.2, new PositionConnector(effect.Position)), lifespan.end);
            }
        }
    }
}
