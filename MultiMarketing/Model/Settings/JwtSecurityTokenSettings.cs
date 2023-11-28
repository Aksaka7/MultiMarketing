namespace MultiMarketing.Model.Settings
{
    public class JwtSecurityTokenSettings
    {
        //Burayı Program.cs te bulunana alanı çalıştırmak istedim. 71. satırraki Code için geçerli 
        //Ayrıca bu alanın iiçerisinde bulunan degişkenlere
        //ulaşıp ilgili işlemleri yapması için açılmıştır. 

        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public double DurationIn { get; set; }

    }
}
