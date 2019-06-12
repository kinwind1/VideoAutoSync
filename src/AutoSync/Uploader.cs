using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoSync
{
    public class UploadTaskInfo
    {
        public string Title { get; set; }
        public string VideoPath { get; set; }
        public string ThumbtailPath { get; set; }
    }

    public class Uploader : ITask<UploadTaskInfo>
    {
        public int ID { get; set; }

        public string TaskType
        {
            get => "uploader";
        }

        public int Progress
        {
            get; set;
        }
        

        public string Description { get; set; }

        public Uploader()
        {
            //set login cookie
        }

        public void Run(UploadTaskInfo info)
        {
            Description = "Uploading: " + info.Title;
            Progress = 0;
            System.Threading.Thread.Sleep(100000);
        }
    }
}
