using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.Data;
using Todo.Entity.Concrete;
using TodoAPI.Data;
using TodoAPI.Entity;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContexts _dbcontext;

        public AuthController(DatabaseContexts dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //  POST `/auth/login`

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user.Email == null)
            {
                return BadRequest("Eposta adresisini girmediz!");
            }
            else if (user.Password == null)
            {
                return BadRequest("Şifreyi girmediniz!");
            }

            user = await LoginUser(user.Email, user.Password);
            if (user == null)
            {
                return BadRequest("Şifre ya da eposta yanlış!");
            }

            return Ok(user);
        }

        //POST `/auth/register`

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Register([FromBody] User user)
        {
            if (user.Email == null)
            {
                return BadRequest("Eposta adresisini girmediz!");
            }
            else if (user.Password == null)
            {
                return BadRequest("Şifreyi girmediniz!");
            }

            try
            {
                _dbcontext.Users.Add(user);
                await _dbcontext.SaveChangesAsync();
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }

            user = await LoginUser(user.Email, user.Password);
            return Ok(user);
        }

        /// <summary>
        /// JWT Token kısmı
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>

        public async Task<User> LoginUser(string Email, string Password)
        {
            User user = await _dbcontext.Users.FindAsync(Email);
          
            if (user == null || Password.Equals(user.Password) == false)
            {
                return null;
            }


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                    new Claim(ClaimTypes.NameIdentifier, user.FirstName),
                    new Claim(ClaimTypes.Name, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.IsActive = true;

            return user;
        }
    }
}

