using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.Common.Services;
using ForConsumption.Service.Common;
using ForConsumption.Service.DAO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using JsonAjax = ForConsumption.Common.Common.JsonResult;


namespace ForConsumption.Service.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly MemberDataContext DataContext = new MemberDataContext();

        [HttpPost]
        [Route("[action]")]
        public JsonAjax AccessToken([FromBody] PostContext context)
        {
            if (!context.Verify(out string message))
            {
                return JsonAjax.Error(message);
            }

            string[] formas = JsonMapper.Deserialize<string[]>(context.Context);

            if (formas is null || formas.Length != 2)
            {
                return JsonAjax.Error("无效的账号或密码");
            }

            string username = formas[0];
            string password = formas[1];

            Member member = DataContext.Members.Select.Where(i => i.LoginName == username && i.Password == password).ToList().FirstOrDefault();

            if (member is null)
            {
                return JsonAjax.Error("无效的账号或密码");
            }

            member.LoginCounter += 1;
            member.LoginTime = DateTime.Now;

            DataContext.Members.Update(member);
            DataContext.SaveChanges();

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Hash, member.ID.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenParameter.Instance.Secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtToken = new JwtSecurityToken(TokenParameter.Instance.Issuer, TokenParameter.Instance.Audience, claims, expires: DateTime.UtcNow.AddMinutes(TokenParameter.Instance.AccessTimeout), signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            string ss = ResultMapper.Serialize(new[] { token, JsonMapper.Serialize(member) });
            return JsonAjax.Success<string>(ss);
        }



        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public JsonAjax GetList([FromBody] PostContext context)
        {
            try
            {
                if (context.Verify(out string message) == false)
                {
                    return JsonAjax.Error(message);
                }

                System.Collections.Generic.List<Member> list = DataContext.Members.Select.ToList();

                string ss = ResultMapper.Serialize(list.ToArray());
                return JsonAjax.Success<string>(ss);
            }
            catch (Exception e)
            {
                return JsonAjax.Error(e);
            }

        }
    }
}
