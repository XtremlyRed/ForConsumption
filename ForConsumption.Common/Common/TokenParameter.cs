namespace ForConsumption.Common.Common
{
    public class TokenParameter
    {
        //颁发者 
        public const string Issuer = "ForConsumption";
        //接收者   
        public const string Audience = "ForConsumption_Client";
        //签名秘钥
        public const string Secret = "1@$DFGKJG%Z#$^%$&^576/*-+66@#7$%^9*909_0(0_(LPPLO*9^&6jdsw#$%^&*()^743%46&4…%**&86dHGJIhu&*9z%z%FHYT*6TGUGYR$#FEz*Zz";

        //AccessToken过期时间（分钟）
        public const int AccessTokenTimeout = 30;

        public static string CurrentToken { get; set; }

    }
}
