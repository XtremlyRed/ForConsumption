namespace ForConsumption.Common.Common
{
    public class TokenParameter
    {
        public static TokenParameter Instance { get; set; }

        //颁发者 
        public string Issuer { get; set; } = "ForConsumption";
        //接收者   
        public string Audience { get; set; } = "ForConsumption_Client";
        //签名秘钥
        public string Secret { get; set; }   

        //AccessToken过期时间（分钟）
        public int AccessTimeout { get; set; } = 30;



    }
}
