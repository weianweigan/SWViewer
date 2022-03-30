namespace SwFormatConverter.Model
{
    public class SldResult<TData>
    {
        public int Code { get; set; } = 200;

        public TData Data { get; set; }

        public string Msg { get; set; }

    }
}
