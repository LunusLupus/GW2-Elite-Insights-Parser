﻿using GW2EIEvtcParser.Exceptions;
using GW2EIEvtcParser.ParsedData;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData;

public class PlayerActor : SingleActor
{
    public bool IsFriendlyPlayer => AgentItem.Type == AgentItem.AgentType.Player || AgentItem.IsNotInSquadFriendlyPlayer;

    // Constructors
    internal PlayerActor(AgentItem agent) : base(agent)
    {
        if (agent.IsNPC)
        {
            throw new EvtcAgentException("Agent is NPC");
        }
        if (IsFakeActor)
        {
            throw new EvtcAgentException("Players can't be fake actors");
        }
    }
    internal override void OverrideName(string name)
    {
        throw new InvalidOperationException("Players' name can't be overriden");
    }
    internal override void SetManualHealth(int health, IReadOnlyList<(long hpValue, double percent)>? hpDistribution = null)
    {
        throw new InvalidOperationException("Players' health can't be overriden");
    }

    public override int GetCurrentHealth(ParsedEvtcLog log, double currentHealthPercent)
    {
        return -1;
    }

    public override int GetCurrentBarrier(ParsedEvtcLog log, double currentBarrierPercent, long time)
    {
        return -1;
    }

    public override string GetIcon()
    {
        return !IsFriendlyPlayer ? GetHighResolutionProfIcon(Spec) : GetProfIcon(Spec);
    }

    protected override void InitAdditionalCombatReplayData(ParsedEvtcLog log, CombatReplay replay)
    {
        base.InitAdditionalCombatReplayData(log, replay);
        // Fight related stuff
        log.FightData.Logic.ComputePlayerCombatReplayActors(this, log, replay);
        ProfHelper.ComputeProfessionCombatReplayActors(this, log, replay);
        if (replay.Rotations.Count != 0)
        {
            replay.Decorations.Add(new ActorOrientationDecoration(((int)replay.TimeOffsets.start, (int)replay.TimeOffsets.end), AgentItem));
        }
    }

    //

    public override SingleActorCombatReplayDescription GetCombatReplayDescription(CombatReplayMap map, ParsedEvtcLog log)
    {
        return new PlayerCombatReplayDescription(this, log, map, InitCombatReplay(log));
    }
}