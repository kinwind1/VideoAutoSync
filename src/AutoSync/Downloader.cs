using System;
using System.Net;
using System.Net.Http;
using YoutubeExplode;
using YoutubeExplode.Models;
using System.IO;
using YoutubeExplode.Models.MediaStreams;

namespace AutoSync
{
    public class DownloadTaskInfo
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbtailUrl { get; set; }
        public Video Video { get; set; }
    }

    public class Downloader : ITask<DownloadTaskInfo> , IProgress<double>
    {
        public int ID { get; set; }

        public string TaskType
        {
            get => "downloader";
        }

        public int Progress
        {
            get; set;
        }

        public HttpClient http;
        public YoutubeClient client;

        public Downloader()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("127.0.0.1", 1080),
                UseProxy = true
            };
            http = new HttpClient(handler);
            client = new YoutubeClient(http);
        }

        public string Description { get; set; }

        public void Run(DownloadTaskInfo info)
        {
            Description = "Downloading: " + info.Title;
            Progress = 0;
            var media = client.GetVideoMediaStreamInfosAsync(info.Video.Id).Result;
            using(FileStream fs = new FileStream(string.Format("{0}.mp4", info.Video.Id), FileMode.Create, FileAccess.Write))
            {
                client.DownloadMediaStreamAsync(media.Muxed.WithHighestVideoQuality(),fs,this).Wait();
            }
            using (FileStream  fs2 = new FileStream(string.Format("{0}.png",info.Video.Id), FileMode.Create, FileAccess.Write))
            {
                http.GetStreamAsync(info.ThumbtailUrl).Result.CopyTo(fs2);
            }
            EventBus.Upload(new UploadTaskInfo() { ThumbtailPath = "test.png", Title = info.Title, VideoPath = "test.mp4" });
        }

        public void Report(double p)
        {
            Progress = (int)(p * 100);
        }
    }
}
