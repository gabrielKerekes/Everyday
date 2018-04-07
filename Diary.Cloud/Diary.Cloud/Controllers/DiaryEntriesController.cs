using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Diary.Model;

// todo: refactor
namespace Diary.Cloud.Controllers
{
    public class DiaryEntriesController : ApiController
    {
        private static string BaseFilePath => $"{HttpContext.Current.Server.MapPath("..")}\\entries\\";

        private static object entriesCacheLock = new object();
        // todo: refactor
        private static List<DiaryEntry> entriesCache;
        private static List<DiaryEntry> EntriesCache
        {
            get
            {
                if (entriesCache == null)
                    LoadCache();

                return entriesCache;
            }
        }

        //private static bool cacheLoaded;

        public IEnumerable<DiaryEntry> Get()
        {
            lock (entriesCacheLock)
            {
                return EntriesCache;
            }
        }

        // todo: oddelovat nie riadkamy, ale \n--\n a -- je akoze separator sekcii..
        //      to asi tiez nestaci, sekcie musia by aj nejak oznacene, lebo nemusim mat vzdy text, thoughts alebo tags, mozem neico z tohov ynechat
        // todo: refactor
        private static void LoadCache()
        {
            entriesCache = new List<DiaryEntry>();

            var fileNames = Directory.GetFiles(BaseFilePath);
            foreach (var fileName in fileNames)
            {
                var lines = File.ReadAllLines(fileName).Where(l => !string.IsNullOrEmpty(l)).ToList();
                entriesCache.Add(new DiaryEntry
                {
                    Id = int.Parse(lines[0]),
                    Date = DateTime.Parse(lines[1]),
                    Title = lines[2],
                    Text = lines[3],
                    Thoughts = lines[4],
                    Tags = lines.Count > 5 ? DiaryEntry.StringToTags(lines[5]) : null,
                });
            }

            //cacheLoaded = true;
        }

        // todo: sem sa moze rovno poslat cely entry a mozno sa da spravit aj metoda createfilepath, ktora vrati celu cestu....
        private string CreateFileName(DateTime date, int id)
        {
            return $"{date.ToShortDateString()}_{id}.txt";
        }

        private string CombinePaths(string p1, string p2)
        {
            return $"{p1}\\{p2}";
        }

        public void Post([FromBody]DiaryEntry entry)
        {
            lock (entriesCacheLock)
            {
                var existingEntry = EntriesCache.FirstOrDefault(e => e.Id == entry.Id);
                if (existingEntry != null)
                {
                    entriesCache.Remove(existingEntry);
                    var existingFilePath = CombinePaths(BaseFilePath, CreateFileName(existingEntry.Date, existingEntry.Id));
                    if (File.Exists(existingFilePath))
                        File.Delete(existingFilePath);
                }

                Directory.CreateDirectory(BaseFilePath);

                var textToWrite = $"{entry.Id}\n{entry.Date.ToShortDateString()}\n{entry.Title}\n\n{entry.Text}\n\n{entry.Thoughts}\n\n{entry.TagsToString()}";
                File.WriteAllText(CombinePaths(BaseFilePath, CreateFileName(entry.Date, entry.Id)), textToWrite);

                EntriesCache.Add(entry);
            }
        }

        public void Delete([FromBody]int entryId)
        {
            lock (entriesCacheLock)
            {
                var existingEntry = EntriesCache.FirstOrDefault(e => e.Id == entryId);
                if (existingEntry != null)
                {
                    EntriesCache.Remove(existingEntry);

                    if (File.Exists(CombinePaths(BaseFilePath, CreateFileName(existingEntry.Date, existingEntry.Id))))
                        File.Delete(CombinePaths(BaseFilePath, CreateFileName(existingEntry.Date, existingEntry.Id)));
                }
            }
        }
    }
}
