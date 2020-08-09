using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserIdentityMicroService.DataProvides;
using UserIdentityMicroService.DTO;
using UserIdentityMicroService.Utilities;

namespace UserIdentityMicroService.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // private readonly AppSettings appSettings;
        private readonly IUserService userService;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_userService"></param>
        /// <param name="options"></param>
        public AuthenticationController(IUserService _userService)
        {
            this.userService = _userService;
            // this.appSettings = options.Value;
        }

        /// <summary>
        /// Authenticates User
        /// </summary>
        /// <param name="userInfo">user data</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>User details with token</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserLoginDTO userInfo)
        {
            if (userInfo == null)
                return StatusCode(400, new { message = Constants.InvalidObject });

            if (!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.InvalidObject });

            try
            {
                userInfo.Password = CommonMethods.EncryptText(userInfo.Password);
                var user = await userService.Authenticate(userInfo.Username, userInfo.Password);
                
                if (user == null)
                    return StatusCode(400, new { message = Constants.WrongCredentials });

                string userRole = Enum.GetName(typeof(Roles), user.RoleId);
                string jwttoken = GenerareToken(user, userRole);

                return Ok(new
                {
                    user.FirstName,
                    user.Username,
                    user.LastName,
                    Token = jwttoken
                });
            }
            catch (CustomException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// New user Registration
        /// </summary>
        /// <param name="userData">userData</param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>User Details</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userData)
        {
            if (userData == null)
                return StatusCode(400, new { message = Constants.InvalidObject});

            if(!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.InvalidObject });

            try
            {
                userData.Password = CommonMethods.EncryptText(userData.Password);
                await userService.Create(userData, userData.Password);
                return Created("Registered", new { 
                    Username = userData.Username
                });
            }
            catch (CustomException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Generates Token
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="userRole">user role</param>
        /// <returns></returns>
        private static string GenerareToken(UserDTO user, string userRole)
        {
            //security key
            string securityKey = AppSettings.Secret;
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, userRole));
            claims.Add(new Claim("Our_Custom_Claim", "Our custom value"));

            //create token
            var token = new JwtSecurityToken(
                    issuer: "smesk.in",
                    audience: "readers",
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCredentials
                    , claims: claims
                );


            var jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwttoken;
        }
    }
}
