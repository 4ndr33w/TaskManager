namespace TaskManager.Models.Content
{
    public class DataContent
    {
        public Enums.AuthorizationType AuthorizationType { get; set; }
        public Enums.HttpMethod HttpMethod { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Content { get; set; }
        public string Token { get; set; }
    }
}
