using System;
using SQLite;

namespace MeadowBoardDemo.Entitties
{
    [Table("MailboxLogs")]
    public class MailboxLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public MailboxLogType Type { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public enum MailboxLogType
    {
        MailboxOpened,
        MailboxClosed,
    }
}
