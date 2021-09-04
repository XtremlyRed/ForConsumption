
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace ForConsumption.Common.Common
{
    public class JwtMapper
    {
        private static readonly IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        private static readonly IJsonSerializer serializer = new JsonNetSerializer();
        private static readonly IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();

        public string JwtMapperKey { get; set; } = "!@#$%^&*())(*&^%$#@234*-*+9+9587%^*&#@$%^&((@#68-/9-/-*4%&**(^%()44^%&^%$^$4$&&*32T**$#$%";
        public string Encrypt(object payload)
        {
            IJwtEncoder jwtEncoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            string resut = jwtEncoder.Encode(payload, JwtMapperKey);

            return resut;
        }



        public string Decrypt(string token)
        {
            IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();
            IJwtValidator jwtValidator = new JwtValidator(serializer, dateTimeProvider);
            IJwtDecoder jwtDecoder = new JwtDecoder(serializer, jwtValidator, urlEncoder, algorithm);

            string jsonResult = jwtDecoder.Decode(token, JwtMapperKey, true);
            return jsonResult;
        }

    }
}
