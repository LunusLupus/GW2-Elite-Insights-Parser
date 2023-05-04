﻿using System;
using System.Linq;
using GW2EIEvtcParser.ParsedData;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class CastFinderHelpers
    {
        internal static bool HasRelatedHit(CombatData combatData, long skillID, AgentItem agent, long time, long epsilon = ServerDelayConstant)
        {
            return combatData.GetDamageData(skillID)
                .Any(hit => hit.CreditedFrom == agent && Math.Abs(hit.Time - time) < epsilon);
        }

        internal static bool HasPreviousCast(CombatData combatData, long skillID, AgentItem agent, long time, long epsilon = ServerDelayConstant)
        {
            return combatData.GetAnimatedCastData(skillID)
                .Any(cast => cast.Caster == agent && cast.Time <= time && time - cast.Time < epsilon);
        }

        internal static bool HasGainedBuff(CombatData combatData, long buffID, AgentItem agent, long time, long epsilon = ServerDelayConstant)
        {
            return combatData.GetBuffData(buffID)
                .OfType<BuffApplyEvent>()
                .Any(apply => apply.To == agent && Math.Abs(apply.Time - time) < epsilon);
        }
        internal static bool HasGainedBuff(CombatData combatData, long buffID, AgentItem agent, long time, AgentItem source, long epsilon = ServerDelayConstant)
        {
            return combatData.GetBuffData(buffID)
                .OfType<BuffApplyEvent>()
                .Any(apply => apply.To == agent && apply.CreditedBy == source && Math.Abs(apply.Time - time) < epsilon);
        }

        internal static bool HasGainedBuff(CombatData combatData, long buffID, AgentItem agent, long time, long appliedDuration, long epsilon = ServerDelayConstant)
        {
            return combatData.GetBuffData(buffID)
                .OfType<BuffApplyEvent>()
                .Any(apply => apply.To == agent && Math.Abs(apply.Time - time) < epsilon && Math.Abs(apply.AppliedDuration - appliedDuration) < epsilon);
        }

        internal static bool HasGainedBuff(CombatData combatData, long buffID, AgentItem agent, long time, long appliedDuration, AgentItem source, long epsilon = ServerDelayConstant)
        {
            return combatData.GetBuffData(buffID)
                .OfType<BuffApplyEvent>()
                .Any(apply => apply.To == agent && apply.CreditedBy == source && Math.Abs(apply.Time - time) < epsilon && Math.Abs(apply.AppliedDuration - appliedDuration) < epsilon);
        }

        internal static bool HasSpawnedMinion(AgentData agentData, MinionID minion, AgentItem master, long time, long epsilon = ServerDelayConstant)
        {
            return agentData.GetNPCsByID(minion)
                .Any(agent => agent.GetFinalMaster() == master && Math.Abs(agent.FirstAware - time) < epsilon);
        }
    }
}