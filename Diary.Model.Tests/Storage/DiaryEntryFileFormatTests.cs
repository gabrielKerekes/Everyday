using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diary.Model.Storage
{
    [TestClass]
    public class DiaryEntryFileFormatTests
    {
        [TestMethod]
        public void DiaryEntryToFileContent_AndBack_Basic()
        {
            var diaryEntry = CreateDiaryEntry();

            var diaryEntryString = DiaryEntryFileFormat.DiaryEntryToFileContent(diaryEntry);
            var recreatedEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(diaryEntryString.Split(new[] { "\r\n" }, StringSplitOptions.None));

            Assert.AreEqual(diaryEntry.Id, recreatedEntry.Id);
            Assert.AreEqual(diaryEntry.Date, recreatedEntry.Date);
            Assert.AreEqual(diaryEntry.Title, recreatedEntry.Title);
            Assert.AreEqual(diaryEntry.Text, recreatedEntry.Text);
            Assert.AreEqual(diaryEntry.Thoughts, recreatedEntry.Thoughts);
        }

        [TestMethod]
        public void DiaryEntryToFileContent_AndBack_SomeFieldsCanBeNull()
        {
            var diaryEntry = CreateDiaryEntry();
            diaryEntry.Title = null;
            diaryEntry.Text = null;
            diaryEntry.Thoughts = null;
            diaryEntry.Tags = null;

            var diaryEntryString = DiaryEntryFileFormat.DiaryEntryToFileContent(diaryEntry);
            var recreatedEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(diaryEntryString.Split(new[] { "\r\n" }, StringSplitOptions.None));

            Assert.AreEqual(diaryEntry.Id, recreatedEntry.Id);
            Assert.AreEqual(diaryEntry.Date, recreatedEntry.Date);
            Assert.AreEqual(string.Empty, recreatedEntry.Title);
            Assert.AreEqual(string.Empty, recreatedEntry.Text);
            Assert.AreEqual(string.Empty, recreatedEntry.Thoughts);
        }

        [TestMethod]
        public void DiaryEntryToFileContent_AndBack_SomeFieldsCanBeEmpty()
        {
            var diaryEntry = CreateDiaryEntry();
            diaryEntry.Title = string.Empty;
            diaryEntry.Text = string.Empty;
            diaryEntry.Thoughts = string.Empty;
            diaryEntry.Tags = new List<string>();

            var diaryEntryString = DiaryEntryFileFormat.DiaryEntryToFileContent(diaryEntry);
            var recreatedEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(diaryEntryString.Split(new[] { "\r\n" }, StringSplitOptions.None));

            Assert.AreEqual(diaryEntry.Id, recreatedEntry.Id);
            Assert.AreEqual(diaryEntry.Date, recreatedEntry.Date);
            Assert.AreEqual(diaryEntry.Title, recreatedEntry.Title);
            Assert.AreEqual(diaryEntry.Text, recreatedEntry.Text);
            Assert.AreEqual(diaryEntry.Thoughts, recreatedEntry.Thoughts);
        }

        [TestMethod]
        public void FileContentToDiaryEntry_PassingEmptyArrayReturnsNull()
        {
            var recreatedEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(new string[] { });

            Assert.AreEqual(null, recreatedEntry);
        }

        [TestMethod]
        public void FileContentToDiaryEntry_PassingNullReturnsNull()
        {
            var recreatedEntry = DiaryEntryFileFormat.FileContentToDiaryEntry(null);

            Assert.AreEqual(null, recreatedEntry);
        }

        private DiaryEntry CreateDiaryEntry()
        {
            var diaryEntry = DiaryEntry.NewDefault();
            diaryEntry.Id = 1;
            diaryEntry.Date = new DateTime(2018, 2, 13);

            return diaryEntry;
        }
    }
}
