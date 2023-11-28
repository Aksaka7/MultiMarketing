using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiMarketing.Context;
using MultiMarketing.Model.CustomerInfo;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IConfiguration configuratiod, MarketingDBContext dbconnector) : ControllerBase
    {
        private readonly MarketingDBContext dbconnector = dbconnector;
        private readonly IConfiguration configuration = configuratiod;

        [HttpPost]
        [Route("EnterIslemleri")]
        public IActionResult GirisIslemleri([FromBody] LoginUserModel model)
        {
            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and Password cannot be empty!");
            }

            var response = new UserModel(); // Login işlemi için bunu çagırıyoruz.

            // Kayıtlı üyeyi DBde ara sonra istediklerimi bana ilet
            var findUser = dbconnector.Registers.FirstOrDefault(p => p.Email == model.Email && p.Password == model.Password);

            if (findUser == null)
            {
                return NotFound("Username veya Password Hatalı kontrol et");
            }

            var userRoles = dbconnector.Rollers.Where(p => p.UserId == findUser.Id).Include(c => c.Customer).AsEnumerable();

            var expirationInMinutes = TimeSpan.FromMinutes(5);
            var expireMinute = DateTime.Now.AddMinutes(expirationInMinutes.Minutes);

            var claims = new List<Claim> //Burda Bir Ekrana login işlemi sonrası gorune bilgi ayarlarını yapıyoruz
            {//Şİfreli olan bilgilerde olması gereken detaylar gorunmesini saglıyoruz. 
                new Claim(JwtRegisteredClaimNames.Sub, configuration["JwtSecurityToken:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,EpochTime.GetIntDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp,EpochTime.GetIntDate(expireMinute).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, configuration["JwtSecurityToken:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, configuration["JwtSecurityToken:Issuer"]),
                new Claim("Name",findUser.Name),
                new Claim("Surname",findUser.Surname),
                new Claim("Email", findUser.Email),
                new Claim("UserId", findUser.Id.ToString())
            };

            if (userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role));
                }
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityToken:Key"]));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token oluşturuyoruz ki bilgilerimiz şifreli iletilsin
            var token = new JwtSecurityToken(
                issuer: configuration["JwtSecurityToken:Issuer"],
                audience: configuration["JwtSecurityToken:Audience"],
                claims: claims,
                expires: expireMinute,
                signingCredentials: singIn);

            var tokenHandler = new JwtSecurityTokenHandler();

            response.TokenExpireDate = DateTime.Now;
            response.Authenticate = true;
            response.Token = tokenHandler.WriteToken(token);
            response.Message = $"Giriş başarılı Hoşgeldiniz {model.Email}";

            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
