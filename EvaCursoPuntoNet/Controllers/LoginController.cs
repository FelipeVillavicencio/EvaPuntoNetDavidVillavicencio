using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EvaCursoPuntoNet.Models;

namespace EvaCursoPuntoNet.Controllers
{
    public class LoginController : Controller
    {
        private readonly AdventureWorksLt2019Context _context; // Usando AdventureWorksLT2019Context
        private readonly ILogger<LoginController> _logger;

        public LoginController(AdventureWorksLt2019Context context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string cEmail, string cPassword)
        {
            try
            {
                var userInfo = await (from cust in _context.Customers
                                                  where cust.EmailAddress == cEmail  // Validamos el correo, pero pueden agregar lo que sige, con las respectivas consideraciones --> //&& cust.PasswordHash == cPassword (ESTA PARTE PUEDE IR O NO, PERO SI LA USAN RECORDAR QUE EN LA VIDA REAL NO SE USA // Ajustado para usar PasswordHash
                                                  select new
                                                  {
                                                      IDCustomer = cust.CustomerId,
                                                      Nombre = cust.FirstName,
                                                      Apellido = cust.LastName,
                                                      Email = cust.EmailAddress
                                                  }).SingleOrDefaultAsync();

                            if (userInfo != null)
                            {
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, userInfo.IDCustomer.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, userInfo.IDCustomer.ToString()),
                                    new Claim(ClaimTypes.GivenName, userInfo.Nombre),
                                    new Claim(ClaimTypes.Surname, userInfo.Apellido),
                                    new Claim(ClaimTypes.Email, userInfo.Email)
                                };

                                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                                await HttpContext.SignInAsync(
                                    "CookieAuth",
                                    new ClaimsPrincipal(claimsIdentity));

                                _logger.LogInformation("User: {} successfully logged in", userInfo.Email);

                                return RedirectToAction("Index", "Home");
                            }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
                return RedirectToAction("Index", "Login");
            }
            // Usando la tabla 'Customer' y la columna 'PasswordHash' de AdventureWorksLT2019
            
            TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}