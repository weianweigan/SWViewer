namespace SwFormat.Entity
{
    public static class ExistItems
    {
        public static List<string> Items { get;set; } = new List<string>();

        public static void Add(string id)
        {
            Items.Add(id);
        }

        public static bool Exist(string id)
        {
            return Items.Contains(id);
        }
    }
}
