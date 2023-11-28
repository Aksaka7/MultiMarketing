namespace MultiMarketing.Model.CustomerInfo
{
    public class UserModel
    {
        // Login işlemi gerçekleştikten sonra olusacak olaylar
        public bool Authenticate { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public string? Message { get; set; }
    }
}
