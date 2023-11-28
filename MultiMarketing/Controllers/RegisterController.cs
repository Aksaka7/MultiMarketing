using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiMarketing.Context;
using MultiMarketing.Model.CustomerAccount;
using MultiMarketing.Model.CustomerInfo;

namespace MultiMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController(MarketingDBContext dbsettingConnection) : ControllerBase
    {
        private readonly MarketingDBContext dbsettingConnection = dbsettingConnection;

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser(CustomerInfoModel model)
        {
            if (model == null)
                return BadRequest("Lütfen Boşlukları Doldurunuz.");

            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email ve Parola boş bırakılamaz.");
            }


            _ = dbsettingConnection.Registers.Add(new Context.Domain.UserRegister
            {
                Name = model.Name,
                Surname = model.Surname,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Id = Guid.NewGuid(),
                DateTime = model.DateTime,
            });

            var result = dbsettingConnection.SaveChanges();

            if (result <= 0)
            {
                return BadRequest("User Olusmadı");
            }
            else
            {
                return Ok("User Oluştu");
            }


        }

    }
}
