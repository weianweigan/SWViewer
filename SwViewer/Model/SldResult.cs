namespace SwViewer
{
    public class SldResult<TData>
    {
        public int code { get; set; }

        public TData data { get; set; }

        public string msg { get; set; }
    }

    public class SldFile
    {
        public string fileName { get; set; }

        public string fileId { get; set; }

        public string fileUrl { get; set; }

        public string previewUrl { get; set; }
    }
}
