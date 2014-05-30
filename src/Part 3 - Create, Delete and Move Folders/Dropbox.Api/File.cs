using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dropbox.Api
{
    [JsonObject(MemberSerialization.OptIn)]
    public class File
    {
        [JsonProperty(PropertyName = "size")]
        public string Size { get; internal set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; internal set; }

        [JsonProperty(PropertyName = "thumb_exists")]
        public bool ThumbnailExists { get; internal set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Bytes { get; internal set; }

        [JsonProperty(PropertyName = "is_dir")]
        public bool IsDirectory { get; internal set; }

        [JsonProperty(PropertyName = "root")]
        public string Root { get; internal set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; internal set; }

        [JsonProperty(PropertyName = "mime_type")]
        public string MimeType { get; internal set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; internal set; }

        [JsonProperty(PropertyName = "contents")]
        public IEnumerable<File> Contents { get; internal set; }

        [JsonProperty(PropertyName = "modified")]
        public DateTime Modified { get; internal set; }
    }
}
