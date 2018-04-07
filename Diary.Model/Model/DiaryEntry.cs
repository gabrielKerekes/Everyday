using System;
using System.Collections.Generic;
using System.Linq;

namespace Diary.Model
{
    public class DiaryEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Thoughts { get; set; }
        // todo: Tag class s tym ze convertovacie metody dat asi tam...
        public List<string> Tags { get; set; }

        public DiaryEntry()
        {
            Title = string.Empty;
            Text = string.Empty;
            Thoughts = string.Empty;
            Tags = new List<string>();
        }

        public string TagsToString()
        {
            return TagsToString(Tags);
        }

        public static string TagsToString(List<string> tags)
        {
            if (tags == null)
                return string.Empty;

            return string.Join(", ", tags);
        }

        public static List<string> StringToTags(string tagsString)
        {
            return tagsString.Split(new[] { ", " }, StringSplitOptions.None).ToList();
        }

        public void SetTags(string tagsString)
        {
            Tags = StringToTags(tagsString);
        }

        public static DiaryEntry NewDefault()
        {
            return new DiaryEntry
            {
                Id = new Random().Next(),
                Date = DateTime.UtcNow,
                Title = "Awesome day",
                Text = "What you did",
                Thoughts = "What you thought about",
                Tags = new List<string> { "sample", "top", }
            };
        }
    }
}
