namespace ISC_ELIB_SERVER.Configurations
{
    public static class ErrorConfig
    {
        public static Dictionary<string, string[]> Msg(string key, string value)
        {
            return new Dictionary<string, string[]>
            {
                [key] = new[] { value }
            };
        }
    }

}
