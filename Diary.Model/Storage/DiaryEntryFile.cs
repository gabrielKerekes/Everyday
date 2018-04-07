using System.IO;
using System.Linq;

namespace Diary.Model.Storage
{
    public class DiaryEntryFile
    {
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                fileLines = null;
            }
        }

        private string[] fileLines;
        private DiaryEntry diaryEntry;

        public static string CreateFileName(DiaryEntry diaryEntry)
        {
            return $"{diaryEntry.Date.ToShortDateString()}_{diaryEntry.Id}.txt";
        }

        public DiaryEntryFile(string filePath)
        {
            FilePath = filePath;
        }

        public DiaryEntry GetDiaryEntry()
        {
            if (fileLines == null)
                LoadFromFile();

            return diaryEntry;
        }

        private void LoadFromFile()
        {
            fileLines = File.ReadAllLines(FilePath)?.Where(l => !string.IsNullOrEmpty(l)).ToArray();
            diaryEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(fileLines);
        }

        public void SaveDiaryEntry(DiaryEntry newDiaryEntry)
        {
            var fileContent = DiaryEntryFileFormat.DiaryEntryToFileContent(newDiaryEntry);
            File.WriteAllText(FilePath, fileContent);

            // clear the 'cache' so that it will be reloaded by next get
            fileLines = null;
            diaryEntry = null;
        }
    }
}
