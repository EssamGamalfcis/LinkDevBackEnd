using DomainLayer.Models;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.IRepository;
using ServiceLayer.ICustomServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<IdentityUser> _identityUser;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IRepository<IdentityUser> identityUser, UserManager<IdentityUser> userManager)
        {
            _identityUser = identityUser;
            _userManager = userManager;
        }
        public async Task<GeneralServiceResponse> Login(LoginParam param)
        {
            GeneralServiceResponse response = new GeneralServiceResponse();
            try
            {
                LoginResponse data = new LoginResponse();
                var user = _identityUser.GetByCondition(z => z.UserName == param.userName).FirstOrDefault();
                if (user == null)
                {
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message =  "User Not Exist";
                }

                var checklogin = _userManager.CheckPasswordAsync(user, param.password).Result;
                if (checklogin)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.AddRange(new List<Claim>
                        { new Claim(JwtRegisteredClaimNames.Sub,user.UserName)
                    , new Claim(ClaimTypes.NameIdentifier , Guid.NewGuid().ToString())
                    , new Claim(JwtRegisteredClaimNames.Jti , user.Id)
                    });

                    var token = await GenerateToken(claims,user);
                    response.success = true;
                    response.statusCode = HttpStatusCode.OK;
                    response.message =  "Success";
                    data.name =  user.UserName;
                    data.userId = user.Id;
                    data.token = token;
                    var roles = await _userManager.GetRolesAsync(user);
                    data.roles = roles.ToList();
                    response.data = data;
                }
                else
                {
                    response.success = false;
                    response.message = "user name or password is incorrect";
                    response.statusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = "Internal server error";
                response.statusCode = HttpStatusCode.InternalServerError;
            }
            return response;

        }
        private async Task<string> GenerateToken(List<Claim> claims, IdentityUser user)
        {
            SymmetricSecurityKey Key = GetSecurityKey();
            var roles = await _userManager.GetRolesAsync(user);
            AddRolesToClaims(claims, roles);
            var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: "LinkDev",
                audience: "LinkDev",
                expires: DateTime.Now.AddHours(24),
                claims: claims,
                signingCredentials: cred
                );
            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return generatedToken;
        }
        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
        private static SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LinkDevelopementPasswordKey"));
        }
    }
}
