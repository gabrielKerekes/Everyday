using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Diary.Model;
using Newtonsoft.Json;

namespace Diary.CloudClient
{
    public static class EntryApi
    {
        private const string ResourceUri = "http://localhost:57038/api/diaryentries";

        // todo: refactor
        // todo: error handling
        public static IEnumerable<DiaryEntry> GetAll()
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(ResourceUri);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                string html;
                using (var response = (HttpWebResponse) request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<IEnumerable<DiaryEntry>>(html);
            }
            catch (Exception e)
            {
                // todo: smth
                return new List<DiaryEntry>();
            }
        }

        // todo: refactor
        // todo: error handling
        public static void AddOrUpdate(DiaryEntry entry)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(ResourceUri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(entry);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var asd = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // todo: smth
            }
        }

        // todo: refactor
        // todo: error handling
        public static void Delete(int id)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(ResourceUri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentType = "application/json";
                request.Method = "DELETE";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(id);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = (HttpWebResponse) request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var asd = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // todo: smth
            }
        }
    }
}
