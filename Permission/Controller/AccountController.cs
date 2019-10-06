using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Snail.Common;
using Snail.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snail.Permission.Controller
{
    [Route("[controller]/[action]")]
    public class AccountController<TUser,TKey>:ControllerBase
        where TUser:User<TKey>,new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly ILogger _logger;
        public AccountController(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController<TUser,TKey>>();
        }

        /// <summary>
        /// 登录并获取token
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Token(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.LoginName);
            if (user==null || ! await _userManager.CheckPasswordAsync(user, model.Pwd))
            {
                return BadRequest($"用户名或密码错误");
            }
            var claims = await _signInManager.ClaimsFactory.CreateAsync(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890abcdefg"));
            //var key = new SymmetricSecurityKey(Convert.FromBase64String("mPorwQB8kMDNQeeYO35KOrMMFn6rFVmbIohBphJPnp4="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("", "", claims.Claims, null, DateTime.Now.AddMinutes(30), creds);
            var tokenStr=new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenStr);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <remarks>
        /// 用于网页上的登录，登录成功后写入到cockies
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> login(LoginModel model,string returnUrl)
        {
            if (string.IsNullOrEmpty(model.LoginName) || string.IsNullOrEmpty(model.Pwd))
            {
                return new BadRequestObjectResult("用户名和密码不能为空");
            }
            var result=await _signInManager.PasswordSignInAsync(model.LoginName, model.Pwd, false, false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"用户{model.LoginName}登录成功");
                return Redirect(returnUrl??"/");
            }
            else
            {
                _logger.LogWarning($"用户{model.LoginName}登录失败，输入密码{model.Pwd}，返回结果:{JsonConvert.SerializeObject(result)}");
                return new BadRequestObjectResult("用户名或密码错误");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new TUser
            {
                Id = IdGenerator.Generate<TKey>(),
                LoginName = model.LoginName
            };
            var result = await _userManager.CreateAsync(user, model.Pwd);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }
            else
            {
                return BadRequest($"注册失败:{string.Join(",",result.Errors?.ToList().Select(a=>a.Description))}");
            }

        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("success");
        }

    }

    public class LoginModel
    {
        public string LoginName { get; set; }
        public string Pwd { get; set; }
    }

    public class RegisterModel
    {
        public string LoginName { get; set; }
        public string Pwd { get; set; }
    }
}
