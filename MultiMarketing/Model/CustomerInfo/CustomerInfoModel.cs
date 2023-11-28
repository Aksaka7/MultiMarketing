namespace MultiMarketing.Model.CustomerInfo
{
    public class CustomerInfoModel
    {
        //Modelden gelecek olan bilgileri DB e yollayacagız. 
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateTime { get; set; }
        public double Balance { get; set; }
        public double BonusBalance { get; set; }

    }
}
