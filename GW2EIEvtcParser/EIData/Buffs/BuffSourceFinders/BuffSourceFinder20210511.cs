﻿using System;
using System.Collections.Generic;
using System.Linq;
using GW2EIEvtcParser.ParsedData;

namespace GW2EIEvtcParser.EIData
{
    internal class BuffSourceFinder20210511 : BuffSourceFinder20191001
    {
        public BuffSourceFinder20210511(HashSet<long> boonIds) : base(boonIds)
        {
        }

        // Spec specific checks
        protected override int CouldBeEssenceOfSpeed(AgentItem dst, long extension, long buffID, ParsedEvtcLog log)
        {
            BuffInfoEvent buffDescription = log.CombatData.GetBuffInfoEvent(buffID);
            if (buffDescription != null && buffDescription.DurationCap == 0)
            {
                return base.CouldBeEssenceOfSpeed(dst, extension, buffID, log);
            }
            if (extension <= EssenceOfSpeed && dst.Prof == "Soulbeast")
            {
                if (log.FriendliesListBySpec.ContainsKey("Herald") || log.FriendliesListBySpec.ContainsKey("Tempest") || log.FriendliesListBySpec.ContainsKey("Chronomancer"))
                {
                    // uncertain, needs to check more
                    return 0;
                }
                // if not herald, tempest or chrono in squad then can only be the trait
                return 1;
            }
            return -1;
        }

        protected override HashSet<long> GetIDs(ParsedEvtcLog log, long buffID, long extension)
        {
            BuffInfoEvent buffDescription = log.CombatData.GetBuffInfoEvent(buffID);
            if (buffDescription != null && buffDescription.DurationCap == 0)
            {
                return base.GetIDs(log, buffID, extension);
            }
            var res = new HashSet<long>();
            foreach (KeyValuePair<long, HashSet<long>> pair in DurationToIDs)
            {
                if (pair.Key >= extension)
                {
                    res.UnionWith(pair.Value);
                }
            }
            return res;
        }
    }
}
