using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace AutoSync
{
    public class Spider : ITask<CancellationToken>
    {
        public int ID
        {
            get => 0; set { }
        }

        public string TaskType
        {
            get => "spider";
        }

        public int Progress
        {
            get => -1;
        }

        public string Description { get => "Spider"; }

        public YoutubeClient client;

        public Spider()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("127.0.0.1", 1080),
                UseProxy=true
            };
            HttpClient http = new HttpClient(handler);
            client = new YoutubeClient(http);
        }

        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //fetch
                Channel m = client.GetChannelAsync(YoutubeClient.ParseChannelId("https://www.youtube.com/channel/UC1opHUrw8rvnsadT-iGp7Cg")).Result;
                Playlist p = client.GetPlaylistAsync(m.GetChannelVideosPlaylistId()).Result;
                //filter videos need to be downloaded
                var download = p.Videos[1];

                EventBus.Download(new DownloadTaskInfo() { Title = download.Title,
                    ThumbtailUrl = download.Thumbnails.MediumResUrl, VideoUrl = download.GetUrl() ,Video=download});
                try
                {
                    Task.Delay(5000000, token).Wait();
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
