using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myApi.DTO;
using myApi.Repositries;

namespace myApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenReposirty tokenReposirty;
        public AuthController(UserManager<IdentityUser> userManager,ITokenReposirty tokenReposirty)
        { 
            this.userManager = userManager;
            this.tokenReposirty = tokenReposirty;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) 



        { 
            var identityUser=new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };
            var identityResult=await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded) {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                    { 
                   identityResult= await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if(identityResult.Succeeded) 
                    {
                        return Ok("User registered successfully with roles");
                    }
                }
            }
            return BadRequest("User registration failed");
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
           var user= await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if (user != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (isPasswordValid) 
                { 
                    var roles=await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                      var jwtToken=  tokenReposirty.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponse
                        {
                            JWTToken = jwtToken
                        };
                        return Ok(response);
                    }

                    return Ok("Login successful");
                }

            }
            return Unauthorized("Invalid login attempt");
        }
    }
    }


