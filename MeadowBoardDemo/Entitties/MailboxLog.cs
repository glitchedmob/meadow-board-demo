using System;
using System.Collections.Generic;
using System.Globalization;

namespace MeadowBoardDemo.Entitties
{
    public class MailboxLog
    {
        public Guid Id { get; set; }
        public MailboxLogType Type { get; set; }
        public DateTime TimeStamp { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                { "id", Id.ToString() },
                { "type", Type.ToString() },
                { "timeStamp", TimeStamp.ToString("u") },
            };
        }
    }

    public enum MailboxLogType
    {
        MailboxOpened,
        MailboxClosed,
    }
}
