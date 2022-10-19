namespace OAuthTest.Models.GoogleOAuth
{
    public record UserInfo
    {
        public string Iss { get; set; } = string.Empty;
        public string Sub { get; set; } = string.Empty;
        public string Hd { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmailVerified { get; set; } = string.Empty;
        public string AtHash { get; set; } = string.Empty;
        public string Iat { get; set; } = string.Empty;
        public string Exp { get; set; } = string.Empty;
        public string Alg { get; set; } = string.Empty;
        public string Kid { get; set; } = string.Empty;
        public string Typ { get; set; } = string.Empty;
    }
}