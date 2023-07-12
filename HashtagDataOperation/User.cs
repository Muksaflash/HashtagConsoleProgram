namespace HashtagDataOperation
{
    internal class User
    {
        public string nickName;
        public string parserImKey;
        public string instaParserKey;

        public User (string nickName, string parserImKey, string instaParserKey)
        {
            this.nickName = nickName;
            this.parserImKey = parserImKey;
            this.instaParserKey = instaParserKey;
        }
    }
}
