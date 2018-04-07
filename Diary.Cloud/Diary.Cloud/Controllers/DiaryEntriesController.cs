using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Diary.Model;
using Diary.Model.Storage;

namespace Diary.Cloud.Controllers
{
    public class DiaryEntriesController : ApiController
    {
        private static string BaseFilePath => $"{HttpContext.Current.Server.MapPath("..")}\\entries\\";

        private static object entriesCacheLock = new object();

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
        
        public IEnumerable<DiaryEntry> Get()
        {
            lock (entriesCacheLock)
            {
                return EntriesCache;
            }
        }

        private static void LoadCache()
        {
            lock (entriesCacheLock)
            {
                entriesCache = new List<DiaryEntry>();

                var filePaths = Directory.GetFiles(BaseFilePath);
                foreach (var filePath in filePaths)
                {
                    var diaryEntryFile = new DiaryEntryFile(filePath);
                    entriesCache.Add(diaryEntryFile.GetDiaryEntry());
                }
            }
        }

        public void Post([FromBody]DiaryEntry entry)
        {
            lock (entriesCacheLock)
            {
                var existingEntry = EntriesCache.FirstOrDefault(e => e.Id == entry.Id);
                if (existingEntry != null)
                {
                    EntriesCache.Remove(existingEntry);
                    var existingFilePath = CombinePaths(BaseFilePath, DiaryEntryFile.CreateFileName(existingEntry));
                    if (File.Exists(existingFilePath))
                        File.Delete(existingFilePath);
                }

                Directory.CreateDirectory(BaseFilePath);

                var diaryEntryFile = new DiaryEntryFile(CombinePaths(BaseFilePath, DiaryEntryFile.CreateFileName(entry)));
                diaryEntryFile.SaveDiaryEntry(entry);

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

                    var existingFilePath = CombinePaths(BaseFilePath, DiaryEntryFile.CreateFileName(existingEntry));
                    if (File.Exists(existingFilePath))
                        File.Delete(existingFilePath);
                }
            }
        }

        private string CombinePaths(string p1, string p2)
        {
            return $"{p1}\\{p2}";
        }
    }
}
