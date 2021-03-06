﻿using System.Collections.Generic;
using GW2EIEvtcParser.ParsedData;

namespace GW2EIEvtcParser.EIData
{

    internal class HitOnEnemyMechanic : SkillMechanic
    {

        public HitOnEnemyMechanic(long skillId, string inGameName, MechanicPlotlySetting plotlySetting, string shortName, int internalCoolDown, SkillChecker condition) : this(skillId, inGameName, plotlySetting, shortName, shortName, shortName, internalCoolDown, condition)
        {
        }

        public HitOnEnemyMechanic(long skillId, string inGameName, MechanicPlotlySetting plotlySetting, string shortName, string description, string fullName, int internalCoolDown, SkillChecker condition) : base(skillId, inGameName, plotlySetting, shortName, description, fullName, internalCoolDown, condition)
        {
        }

        public HitOnEnemyMechanic(long skillId, string inGameName, MechanicPlotlySetting plotlySetting, string shortName, int internalCoolDown) : this(skillId, inGameName, plotlySetting, shortName, shortName, shortName, internalCoolDown)
        {
        }

        public HitOnEnemyMechanic(long skillId, string inGameName, MechanicPlotlySetting plotlySetting, string shortName, string description, string fullName, int internalCoolDown) : base(skillId, inGameName, plotlySetting, shortName, description, fullName, internalCoolDown)
        {
        }

        internal override void CheckMechanic(ParsedEvtcLog log, Dictionary<Mechanic, List<MechanicEvent>> mechanicLogs, Dictionary<int, AbstractSingleActor> regroupedMobs)
        {
            CombatData combatData = log.CombatData;
            IEnumerable<AgentItem> agents = log.AgentData.GetNPCsByID((int)SkillId);
            foreach (AgentItem a in agents)
            {
                IReadOnlyList<AbstractHealthDamageEvent> combatitems = combatData.GetDamageTakenData(a);
                foreach (AbstractHealthDamageEvent c in combatitems)
                {
                    if (c is DirectHealthDamageEvent && c.HasHit && Keep(c, log))
                    {
                        foreach (AbstractSingleActor actor in log.Friendlies)
                        {
                            if (c.From.GetFinalMaster() == actor.AgentItem)
                            {
                                mechanicLogs[this].Add(new MechanicEvent(c.Time, this, actor));
                            }
                        }
                    }

                }
            }
        }
    }
}
