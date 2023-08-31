using Exam.Dtos;
using Exam.Interfaces;
using Exam.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCotroller : ControllerBase
    {

        private readonly ExamDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHash _hash;
        private readonly EmailService _emailService;

        public AuthCotroller(ExamDbContext context,IConfiguration config,IHash hash,EmailService email)
        {
            _context = context; 
            _config = config;
            _hash = hash;
            _emailService = email;
        }

        [EnableCors("Policy1")]

        [HttpPost("register")]
        public async Task<IActionResult> register(Register reg)
        {
            if (_context.Users.Any(x => x.Email == reg.Email)){
                return BadRequest("user alredy exist");

            }
            var hmac = new HMACSHA256();
            string salt = Convert.ToBase64String(hmac.Key);
            int roleid = 2;

            string userId = DateTime.Now.Year.ToString() + reg.FirstName.Substring(0,2)+reg.LastName.Substring(0,2)+DateTime.Now.Month.ToString(); ;

            var pass = reg.FirstName.Substring(0, 2) + DateTime.Now.Year.ToString() + reg.LastName.Substring(0, 2) ;

            var password=_hash.Hash(pass,salt);
      
            var user = new User { Uid = userId, Firstname = reg.FirstName, Middlename = reg.MiddleName, Lastname = reg.LastName, Photo = reg.Photo, Role = roleid, PasswordHash = password, PasswordSalt = salt, Email = reg.Email,Createddate=reg.Createddate };
       
            await _context.AddAsync(user);

           



            await _context.SaveChangesAsync();
            try
            {
                // Send the email
                var r = reg.Email;
                var s = "Accout credentials";
                var b = $"Userid:{userId} \t  registeremail:{reg.Email}   \t  password:{pass}";
                _emailService.SendEmail(r, s, b);
                var resp = new
                {
                    message = $"Email sent to {r}",
                    pass=password
                };
            }
            catch (Exception)
            {

                return BadRequest();
            }

          


            return Ok(user);
        
        }

        [EnableCors("Policy1")]

        [HttpPost("login")]
        public async Task<IActionResult> login(Login login)
        {
           
            var user=(
                from u in _context.Users join rol in _context.Roles on u.Role equals rol.Id 
                select new {Id=u.Id ,UID=u.Uid,Email=u.Email,Photo=u.Photo,
                Passwordhash=u.PasswordHash,Passwordsalt=u.PasswordSalt,Role=rol.Role1}).FirstOrDefault(x=> x.Email==login.Email);


            if(user==null)
            {

                return BadRequest("user not found");


            }


            if (!_hash.verify(login.Password, user.Passwordhash, user.Passwordsalt))
            {
                return BadRequest("wrong password");
            }


            usrdto usr = new usrdto()
            {
                Id=user.Id,
                UID=user.UID,
                Email=user.Email,
                Photo=user.Photo,
                Passwordhash=user.Passwordhash,
                Passwordsalt=user.Passwordsalt,
                Role=user.Role

            };


            var tkn = tokengenrator(usr);

            return Ok(new {token=tkn,role=user.Role,message="user logged in"});

        }

        private object tokengenrator(usrdto usr)
        {


            var sectret= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:key").Value));
            var crad= new SigningCredentials(sectret,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.SerialNumber,usr.Id.ToString()),

                new Claim(
                    ClaimTypes.Role,usr.Role),
                new Claim(
                    ClaimTypes.Email,usr.Email),
                new Claim(
                    ClaimTypes.Name,usr.UID),

            };

            var tk = new JwtSecurityToken(


_config.GetSection("Jwt:Issuer").Value,
                _config.GetSection("Jwt:Audience").Value,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: crad,
                claims:claims
                );


            var token = new JwtSecurityTokenHandler().WriteToken( tk );
            return Ok(token);
        }
    }
}
