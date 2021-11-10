using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Dropbox.Api
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FileSystemInfo
    {
        [JsonProperty(PropertyName = "size")]
        public string Size { get; internal set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; internal set; }

        [JsonProperty(PropertyName = "thumb_exists")]
        public bool ThumbnailExists { get; internal set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Bytes { get; internal set; }

        [JsonProperty(PropertyName = "modified")]
        public DateTime Modified { get; internal set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; internal set; }

        [JsonProperty(PropertyName = "is_dir")]
        public bool IsDirectory { get; internal set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; internal set; }

        [JsonProperty(PropertyName = "root")]
        public string Root { get; internal set; }

        [JsonProperty(PropertyName = "is_deleted")]
        public bool IsDeleted { get; internal set; }

        public byte[] Data { get; internal set; }

        public void Save(string path)
        {
            using (var fileStream = new FileStream(
                path, FileMode.Create, FileAccess.ReadWrite))
            {
                fileStream.Write(Data, 0, Data.Length);
            }            
        }
    }
}
