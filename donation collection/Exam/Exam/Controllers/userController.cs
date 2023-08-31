using Exam.Dtos;
using Exam.Interfaces;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Exam.Controllers
{
    [EnableCors("Policy1")]

    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {

        private readonly ExamDbContext _context;
        private readonly Random _random = new Random();
        private readonly EmailService _emailService;



        public userController(ExamDbContext context,EmailService email)
        {
            _context = context;
            _emailService = email;

        }



        //get user detaills

       [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getnormaluserdetails")]
        public async Task<IActionResult> getnormaluserdetails()
        {
            try
            {
                var jwt = Request.Headers["Authorization"].First().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(jwt);
                var userid = Convert.ToInt64(jwtToken.Claims.First().Value);

                var data = _context.UserdeatilSP.FromSqlRaw($"Exec normaluserdetails @userid={userid}");
                return Ok(data);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        //user payment details


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("userpaymentdetails")]
        public async Task<IActionResult> userpaymentdetails()
        {
            try
            {
                var jwt = Request.Headers["Authorization"].First().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(jwt);
                var userid = Convert.ToInt64(jwtToken.Claims.First().Value);

                var data = (from u in _context.Users
                            join p in _context.Payments on u.Id equals p.Uid
                            where u.Id == userid
                            select new
                            {
                                u.Id,
                                u.Uid,
                                u.Email,
                                FullName = u.Firstname + " " + u.Middlename + " " + u.Lastname,
                                u.Photo,
                                Address = u.Flatno + ", " + u.Area + ", " + u.State + ", " + u.City + ", " + u.Pincode,
                                p.Status,
                                p.Amount,
                                p.Month,
                                p.Year,
                            });
                return Ok(data);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpGet("userotp")]
        public async Task<IActionResult> userotp()
        {
            try
            {

                var jwt = Request.Headers["Authorization"].First().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(jwt);
                var userid = Convert.ToInt64(jwtToken.Claims.First().Value);
                var useremail = Convert.ToString(jwtToken.Claims.FirstOrDefault(x => x.Value.Contains('@')).Value);

                //  var data = (from x in _context.Users where x.Id == userid select new { email = x.Email });


                var otp = RandomNumber(100, 999);


           //     Send the email
               var r = useremail;
                var s = "Payment Confirmation";
                var b = $"Userid:{userid} \t   Otp:{otp}";
                _emailService.SendEmail(r, s, b);
                var resp = new
                {
                    message = $"Email sent to {r}"
                };

                return Ok(otp);

            }

               // return Ok(otp);
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);


            }

        }


        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }














    //payment table collact payment done

    [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPut("payment")]
    public async Task<IActionResult> payment(int id,int month,int year)
    {
        if (_context.Payments == null)
        {
            return NotFound();
        }


        var z = _context.Payments.FirstOrDefault(a => a.Uid == id && a.Month==month && a.Year==year);
        z.Status = 1;
        await _context.SaveChangesAsync();

        return Ok();
    }




    }
}
