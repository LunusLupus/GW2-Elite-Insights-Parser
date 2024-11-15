﻿using System;

namespace GW2EIEvtcParser.ParsedData
{
    public abstract class IDToGUIDEvent : AbstractMetaDataEvent
    {

        public string HexContentGUID { get; }
        public string Base64ContentGUID { get; }

        public long ContentID { get; }

        public bool IsValid => ContentID >= 0;

        internal IDToGUIDEvent(CombatItem evtcItem) : base(evtcItem)
        {
            (HexContentGUID, Base64ContentGUID) = UnpackGUID(evtcItem.SrcAgent, evtcItem.DstAgent);
            ContentID = evtcItem.SkillID;
        }
        internal IDToGUIDEvent() : base()
        {
            (HexContentGUID, Base64ContentGUID) = ("", "");
            ContentID = -1;
        }
        internal static (string hex, string base64) UnpackGUID(ulong first8, ulong last8)
        {
            byte[] guid = new byte[16];
            byte[] first8Bytes = BitConverter.GetBytes(first8);
            byte[] last8Bytes = BitConverter.GetBytes(last8);
            first8Bytes.CopyTo(guid, 0);
            last8Bytes.CopyTo(guid, first8Bytes.Length);
            return (ParserHelper.ToHexString(guid, 0, 16), Convert.ToBase64String(guid));
        }

    }
}
