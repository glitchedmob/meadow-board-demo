using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Meadow;
using MeadowBoardDemo.Entitties;

namespace MeadowBoardDemo.Services
{
    public class StorageService
    {
        private const string MailboxLogsFile = "MailboxLogs.json";

        private List<MailboxLog> _mailboxLogs = new List<MailboxLog>();

        public async Task Initialize()
        {
            var json = await LoadFile(MailboxLogsFile);

            if (json == null)
            {
                return;
            }

            var parsedLogs = JsonSerializer.Deserialize<List<MailboxLog>>(json);

            if (parsedLogs == null)
            {
                return;
            }

            Console.WriteLine($"Found {parsedLogs.Count} Logs");

            _mailboxLogs = parsedLogs;
        }

        public List<MailboxLog> GetMailboxLogs()
        {
            return _mailboxLogs;
        }

        public async Task SaveMailboxLogs(List<MailboxLog> mailboxLogs)
        {
            _mailboxLogs = mailboxLogs;

            var json = JsonSerializer.Serialize(mailboxLogs);

            await SaveFile(MailboxLogsFile, json);
        }

        private async Task<string?> LoadFile(string filename)
        {
            var path = Path.Combine(MeadowOS.FileSystem.DataDirectory, filename);

            if (!File.Exists(path))
            {
                return null;
            }

            return await File.ReadAllTextAsync(path);
        }

        private async Task SaveFile(string filename, string content)
        {
            var path = MeadowOS.FileSystem.DataDirectory;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using var fs = File.CreateText(Path.Combine(path, filename));

            await fs.WriteAsync(content);
        }
    }
}
