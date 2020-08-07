﻿using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private readonly IUserService userService;
        
        public AuthenticationController(IUserService _userService, IOptions<AppSettings> options)
        {
            this.userService = _userService;
            this.appSettings = options.Value;
        }

        /// <summary>
        /// Authenticates User
        /// </summary>
        /// <param name="userInfo">user data</param>
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
                var user = await userService.Authenticate(userInfo.Username, userInfo.Password);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtSecurityToken = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    user.FirstName,
                    user.Username,
                    user.LastName,
                    Token = jwtSecurityToken
                });
            }
            catch(CustomException ex)
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
                await userService.Create(userData, userData.Password);
                return Ok();
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
    }
}
