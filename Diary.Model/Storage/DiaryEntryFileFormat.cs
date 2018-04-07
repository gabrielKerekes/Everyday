using System;
using System.Linq;
using System.Text;

namespace Diary.Model.Storage
{
    public static class DiaryEntryFileFormat
    {
        private static string Separator = "#";

        private static string BeginSeparator = $"{Separator}BEGIN";
        private static string IdSeparator = $"{Separator}ID";
        private static string DateSeparator = $"{Separator}DATE";
        private static string TitleSeparator = $"{Separator}TITLE";
        private static string ContentSeparator = $"{Separator}CONTENT";
        private static string ThoughtsSeparator = $"{Separator}THOUGHTS";
        private static string TagsSeparator = $"{Separator}TAGS";
        private static string EndSeparator = $"{Separator}END";

        // todo: some more error handling - deal with when occur
        public static string DiaryEntryToFileContent(DiaryEntry diaryEntry)
        {
            var sb = new StringBuilder();

            sb.AppendLine(BeginSeparator);

            sb.AppendLine(IdSeparator);
            sb.AppendLine(diaryEntry.Id.ToString());
            sb.AppendLine(DateSeparator);
            sb.AppendLine(diaryEntry.Date.ToShortDateString());
            sb.AppendLine(TitleSeparator);
            sb.AppendLine(diaryEntry.Title);
            sb.AppendLine(ContentSeparator);
            sb.AppendLine(diaryEntry.Text);
            sb.AppendLine(ThoughtsSeparator);
            sb.AppendLine(diaryEntry.Thoughts);
            sb.AppendLine(TagsSeparator);
            sb.AppendLine(diaryEntry.Tags != null ? string.Join(", ", diaryEntry.Tags) : null);

            sb.AppendLine(EndSeparator);

            return sb.ToString();
        }

        // todo: some more error handling - deal with when occur
        public static DiaryEntry FileContentToDiaryEntry(string[] fileLines)
        {
            if (fileLines == null || fileLines.Length == 0)
                return null;

            var diaryEntry = new DiaryEntry();

            var contentLines = fileLines
                .SkipWhile(l => l != BeginSeparator)
                .TakeWhile(l => l != EndSeparator).ToList();
            
            var idLine = contentLines
                .SkipWhile(l => l != IdSeparator)
                .Skip(1)
                .TakeWhile(l => l != DateSeparator)
                .FirstOrDefault(l => !string.IsNullOrEmpty(l));

            diaryEntry.Id = int.Parse(idLine);

            var dateLine = contentLines
                .SkipWhile(l => l != DateSeparator)
                .Skip(1)
                .TakeWhile(l => l != TitleSeparator)
                .FirstOrDefault(l => !string.IsNullOrEmpty(l));

            diaryEntry.Date = DateTime.Parse(dateLine);

            var titleLines = contentLines
                .SkipWhile(l => l != TitleSeparator)
                .Skip(1)
                .TakeWhile(l => l != ContentSeparator)
                .ToList();

            diaryEntry.Title = string.Join("\n", titleLines);

            var textLines = contentLines
                .SkipWhile(l => l != ContentSeparator)
                .Skip(1)
                .TakeWhile(l => l != ThoughtsSeparator)
                .ToList();

            diaryEntry.Text = string.Join("\n", textLines);

            var thoughtsLines = contentLines
                .SkipWhile(l => l != ThoughtsSeparator)
                .Skip(1)
                .TakeWhile(l => l != TagsSeparator)
                .ToList();

            diaryEntry.Thoughts = string.Join("\n", thoughtsLines);

            var tagsLine = contentLines
                .SkipWhile(l => l != TagsSeparator)
                .Skip(1)
                .TakeWhile(l => l != EndSeparator)
                .FirstOrDefault(l => !string.IsNullOrEmpty(l));

            diaryEntry.Tags = tagsLine?.Split(new [] { ", " }, StringSplitOptions.None).ToList();

            return diaryEntry;
        }
    }
}
