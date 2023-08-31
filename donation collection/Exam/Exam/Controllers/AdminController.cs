using Exam.Dtos;
using Exam.Interfaces;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Exam.Controllers
{

    [EnableCors("Policy1")]

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {



        private readonly ExamDbContext _context;
      

        public AdminController(ExamDbContext context)
        {
            _context = context;
           
        }




        //get user detaills

        [Authorize(AuthenticationSchemes ="Bearer")]
        [HttpGet("getuserdetails")]
        public async Task<IActionResult> getuserdetails()
        {
            try
            {
                var jwt = Request.Headers["Authorization"].First().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(jwt);
                var userid = Convert.ToInt64(jwtToken.Claims.First().Value);

                var data = _context.UserdeatilSP.FromSqlRaw($"Exec userdetails @userid={userid}");
                return Ok(data);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //delete user

        //[Authorize(AuthenticationSchemes = "Bearer")]

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {



            
            var data = _context.Users.FirstOrDefault(x => x.Id == id);
            if(data == null)
            {
                return NotFound();

            }
            else
            {
                var check = (from p in _context.Payments where p.Uid == id select p).ToList();

                if (check != null) {

                    foreach (var item in check)
                    {
                        _context.Payments.Remove(item);


                    }


                }
                _context.Users.Remove(data);
                _context.SaveChanges();
                return Ok();
            }

           





        }




        //create donation request

        [Authorize(AuthenticationSchemes = "Bearer")]

        // DELETE: api/Users/5
        [HttpPost("donationrequest")]
        public async Task<IActionResult> createDonation(paymentdto pay)
        {

            if(_context.Payments.Any(x=> x.Uid == pay.UID && x.Status==2))
            {
                return BadRequest("Request alredy raised");


            }

            var payment= new Payment()
            {
                Uid=pay.UID,
                Month=pay.Month,
                Year=pay.Year,
                Amount=pay.Amount,
                Status=2

            };
            _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return Ok(payment);
        }




        //payment table users


       [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpGet("donationuserlist")]
        public async Task<IActionResult> donationuserlist()
        {

            var ulist = from u in _context.Payments  select new { uid = u.Uid };


            return Ok(ulist);
        }


        //payment table collact payment done

       [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpPut("collectdonation/{id}")]
        public async Task<IActionResult> collectdonation(int id)
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }
          

            var z = _context.Payments.FirstOrDefault(a => a.Uid == id);
            z.Status = 1;
            await _context.SaveChangesAsync();

            return Ok();
        }





        //payment table collact info 

        //[Authorize(AuthenticationSchemes = "Bearer")]

        [HttpGet("paymentcollect/{id}")]
        public async Task<IActionResult> paymentcollect(int id)
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }

            var result = (from u in _context.Users
                          join p in _context.Payments on u.Id equals p.Uid
                          where u.Id == id
                          orderby p.Pid descending

                          select new
                          {
                              u.Id,
                              u.Uid,
                              FullName = u.Firstname + " " + u.Middlename + " " + u.Lastname,
                              u.Email,
                              u.Photo,
                              p.Month,
                              p.Year,
                              p.Amount,
                              p.Status
                          });


            return Ok(result);
        }





        //for edit user details
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpPut("editprofile")]
        public async Task<IActionResult> editprofile(int id,string fullname,string email)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var name=fullname.Split(' ');
            var firstname = name[0];
            var middledname = name[1];   
            var lastname = name[2];

            var z = _context.Users.FirstOrDefault(a => a.Id == id);
            z.Firstname=firstname;
            z.Middlename=middledname;
            z.Lastname=lastname;    
            z.Email=email;
            
            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}
