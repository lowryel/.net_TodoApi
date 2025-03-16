// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using TodoApi.Dtos;

// namespace TodoApi.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class AccountController : ControllerBase
// {
//     private readonly UserManager<IdentityUser> _userManager;
//     private readonly SignInManager<IdentityUser> _signInManager;

//     public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//     }

//     [HttpPost("register")]
//     public async Task<IActionResult> Register([FromBody] RegisterDto model)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var user = new Employee
//         {
//             Name = model.Email,
//             Email = model.Email,
//             Name = model.Name,
//             Phone = model.Phone
//         };

//         var result = await _userManager.CreateAsync(user, model.Password);

//         if (!result.Succeeded)
//             return BadRequest(result.Errors);

//         return Ok("User registered successfully");
//     }

//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginDto model)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);

//         if (!result.Succeeded)
//             return Unauthorized("Invalid login attempt");

//         return Ok("Login successful");
//     }

//     [HttpPost("logout")]
//     public async Task<IActionResult> Logout()
//     {
//         await _signInManager.SignOutAsync();
//         return Ok("Logged out successfully");
//     }
// }