using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebApiRestNetCore.DTO.DtoIncentivo;
using WebApiRestNetCore.Models.jwt;
using WebApiRestNetCore.Services.ServicesIncentivos;

//namespace WebApiRestNetCore.Controllers.ControllerIncentivos
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class IncentivosController : ControllerBase
//    {
//        private readonly IncentivosServices _incentivosService;
//        public IConfiguration _configuration;

//        public IncentivosController(IConfiguration configuracion)
//        {
//            _incentivosService = new IncentivosServices(configuracion);
//            _configuration=configuracion;
//        }

//        [HttpGet]
//        public ActionResult<IEnumerable<IncentivoPagoDTO>> Get(string dni)
//        {
//            var incentivosPagos = _incentivosService.GetIncentivosPagos(dni);
//            return Ok(incentivosPagos);
//        }

//        [HttpPost("GeneralWithDNI")]
//        public ActionResult<IEnumerable<IncentivoPagoDTO>> GetGeneralWithDNI([FromBody] IncentivoPagoRequestDTO request)
//        {
//            string dni = request.Dni;
//            var incentivosPagos = _incentivosService.GetGeneralIncentivosPagosWithDNI(dni);
//            return Ok(incentivosPagos);


//        }

//        ///----------------------------lo que se usa -----------------------------------------------------------------------

//        //[HttpPost("login")]
//        //public dynamic LoginDNI([FromBody] IncentivoPagoRequestDTO request)
//        //{
//        //    string dni = request.Dni;
//        //    bool isDniPresent = _incentivosService.IsDniPresent(dni);

//        //    if (isDniPresent)
//        //    {
//        //        //var data = JsonConvert.DeserializeObject<dynamic>(request.ToString());
//        //        string user = dni;

//        //        var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//        //        var claims = new[]
//        //        {
//        //            new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
//        //           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
//        //          new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
//        //          new Claim("dni", user)

//        //        };

//        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//        //        var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//        //        var token = new JwtSecurityToken(
//        //            jwt.Issuer,
//        //            jwt.Audience,
//        //            claims:claims,
//        //            expires: DateTime.Now.AddMinutes(5),
//        //            signingCredentials:singIn
//        //            );

//        //        return new
//        //        {
//        //            succes=true,
//        //            message="exito",
//        //            result= new JwtSecurityTokenHandler().WriteToken(token)
//        //        };
//        //    }
//        //    else
//        //    {
//        //        return NotFound("No tiene registros");
//        //    }
//        //}


//        //[HttpPost("GeneralWithDNIConfirmationFalse")]
//        //public ActionResult<IEnumerable<IncentivoVistaDTO>> GetGeneralWithDNIConfirmationFalse([FromBody] IncentivoPagoRequestDTO request)
//        //{
//        //    string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

//        //    if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
//        //    {
//        //        return BadRequest("Token inválido");
//        //    }

//        //    token = token.Substring("Bearer ".Length);

//        //    try
//        //    {
//        //        // Validar el token
//        //        var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//        //        var tokenHandler = new JwtSecurityTokenHandler();
//        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//        //        var validationParameters = new TokenValidationParameters
//        //        {
//        //            ValidateIssuerSigningKey = true,
//        //            IssuerSigningKey = key,
//        //            ValidateIssuer = true,
//        //            ValidIssuer = jwt.Issuer,
//        //            ValidateAudience = true,
//        //            ValidAudience = jwt.Audience,
//        //            ValidateLifetime = true,
//        //            ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
//        //        };

//        //        // Validar el token y obtener el ClaimsPrincipal
//        //        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

//        //        // Verificar que el token contiene el claim "dni"
//        //        var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

//        //        if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
//        //        {
//        //            return BadRequest("No se encontró el claim 'dni' en el token.");
//        //        }

//        //        string dni = dniClaim.Value;

//        //        // Continuar con el resto del código para obtener los incentivos con el DNI
//        //        var incentivosVistas = _incentivosService.GetGeneralIncentivosVistasWithDNIConfirmationFalse(dni);

//        //        return Ok(incentivosVistas);
//        //    }
//        //    catch (SecurityTokenException ex)
//        //    {
//        //        // Manejar la excepción de token inválido
//        //        return BadRequest("Token inválido: " + ex.Message);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // Manejar otras excepciones que puedan ocurrir durante la validación
//        //        return StatusCode(500, "Error interno del servidor: " + ex.Message);
//        //    }
//        //}

//        [HttpPost("login")]
//        public dynamic LoginDNI([FromBody] IncentivoPagoRequestDTO request)
//        {
//            string dni = request.Dni;
//            bool isDniPresent = _incentivosService.IsDniPresent(dni);

//            if (isDniPresent)
//            {
//                string user = dni;

//                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//                var claims = new[]
//                {
//            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
//            new Claim("dni", user)
//        };

//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//                var token = new JwtSecurityToken(
//                    claims: claims,
//                    expires: DateTime.Now.AddMinutes(5),
//                    signingCredentials: singIn
//                );

//                return new
//                {
//                    success = true,
//                    message = "exito",
//                    result = new JwtSecurityTokenHandler().WriteToken(token)
//                };
//            }
//            else
//            {
//                return NotFound("No tiene registros");
//            }
//        }


//        [HttpPost("GeneralWithDNIConfirmationFalse")]
//        public ActionResult<IEnumerable<IncentivoVistaDTO>> GetGeneralWithDNIConfirmationFalse([FromBody] IncentivoPagoRequestDTO request)
//        {
//            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

//            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
//            {
//                return BadRequest("Token inválido");
//            }

//            token = token.Substring("Bearer ".Length);

//            try
//            {
//                // Validar el token
//                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//                var validationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = key,
//                    ValidateIssuer = false, // Desactivar la validación del issuer
//                    ValidateAudience = false, // Desactivar la validación de la audiencia
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
//                };

//                // Validar el token y obtener el ClaimsPrincipal
//                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

//                // Verificar que el token contiene el claim "dni"
//                var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

//                if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
//                {
//                    return BadRequest("No se encontró el claim 'dni' en el token.");
//                }

//                string dni = dniClaim.Value;

//                // Continuar con el resto del código para obtener los incentivos con el DNI
//                var incentivosVistas = _incentivosService.GetGeneralIncentivosVistasWithDNIConfirmationFalse(dni);

//                return Ok(incentivosVistas);
//            }
//            catch (SecurityTokenException ex)
//            {
//                // Manejar la excepción de token inválido
//                return BadRequest("Token inválido: " + ex.Message);
//            }
//            catch (Exception ex)
//            {
//                // Manejar otras excepciones que puedan ocurrir durante la validación
//                return StatusCode(500, "Error interno del servidor: " + ex.Message);
//            }
//        }


//        //[HttpPost("UpdateWithDNI")]
//        //public IActionResult UpdateConfirmacionEntrega([FromBody] IncentivoPagoRequestDTO request)
//        //{
//        //    string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

//        //    if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
//        //    {
//        //        return BadRequest("Token inválido");
//        //    }

//        //    token = token.Substring("Bearer ".Length);

//        //    try
//        //    {
//        //        // Validar el token
//        //        var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//        //        var tokenHandler = new JwtSecurityTokenHandler();
//        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//        //        var validationParameters = new TokenValidationParameters
//        //        {
//        //            ValidateIssuerSigningKey = true,
//        //            IssuerSigningKey = key,
//        //            ValidateIssuer = true,
//        //            ValidIssuer = jwt.Issuer,
//        //            ValidateAudience = true,
//        //            ValidAudience = jwt.Audience,
//        //            ValidateLifetime = true,
//        //            ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
//        //        };

//        //        // Validar el token y obtener el ClaimsPrincipal
//        //        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

//        //        // Verificar que el token contiene el claim "dni"
//        //        var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

//        //        if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
//        //        {
//        //            return BadRequest("No se encontró el claim 'dni' en el token.");
//        //        }

//        //        string dni = dniClaim.Value;

//        //        // Continuar con el resto del código para realizar la acción "UpdateConfirmacionEntrega" con el DNI
//        //        int id = (int)request.Id;
//        //        _incentivosService.UpdateConfirmacionEntrega(dni, id);
//        //        return Ok();
//        //    }
//        //    catch (SecurityTokenException ex)
//        //    {
//        //        // Manejar la excepción de token inválido
//        //        return BadRequest("Token inválido: " + ex.Message);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // Manejar otras excepciones que puedan ocurrir durante la validación
//        //        return StatusCode(500, "Error interno del servidor: " + ex.Message);
//        //    }

//        //}

//        [HttpPost("UpdateWithDNI")]
//        public IActionResult UpdateConfirmacionEntrega([FromBody] IncentivoPagoRequestDTO request)
//        {
//            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

//            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
//            {
//                return BadRequest("Token inválido");
//            }

//            token = token.Substring("Bearer ".Length);

//            try
//            {
//                // Validar el token
//                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
//                var validationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = key,
//                    ValidateIssuer = false, // Desactivar la validación del issuer
//                    ValidateAudience = false, // Desactivar la validación de la audiencia
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
//                };

//                // Validar el token y obtener el ClaimsPrincipal
//                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

//                // Verificar que el token contiene el claim "dni"
//                var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

//                if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
//                {
//                    return BadRequest("No se encontró el claim 'dni' en el token.");
//                }

//                string dni = dniClaim.Value;

//                // Continuar con el resto del código para realizar la acción "UpdateConfirmacionEntrega" con el DNI
//                int id = (int)request.Id;
//                _incentivosService.UpdateConfirmacionEntrega(dni, id);
//                return Ok();
//            }
//            catch (SecurityTokenException ex)
//            {
//                // Manejar la excepción de token inválido
//                return BadRequest("Token inválido: " + ex.Message);
//            }
//            catch (Exception ex)
//            {
//                // Manejar otras excepciones que puedan ocurrir durante la validación
//                return StatusCode(500, "Error interno del servidor: " + ex.Message);
//            }
//        }

//    }
//}

namespace WebApiRestNetCore.Controllers.ControllerIncentivos
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncentivosController : ControllerBase
    {
        private readonly IncentivosServices _incentivosService;
        public IConfiguration _configuration;

        public IncentivosController(IConfiguration configuracion)
        {
            _incentivosService = new IncentivosServices(configuracion);
            _configuration = configuracion;
        }

        [HttpGet]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> Get(string dni)
        {
            var incentivosPagos = _incentivosService.GetIncentivosPagos(dni);
            return Ok(incentivosPagos);
        }

        [HttpPost("validateUser")]
        public IActionResult ValidateUser([FromBody] UsuarioDTO request)
        {
            string usuario = request.USUARIO;
            string clave = request.CLAVE;

            UsuarioDTO usuarioValidado = _incentivosService.ValidateUser(usuario, clave);

            if (usuarioValidado != null && !string.IsNullOrEmpty(usuarioValidado.USUARIO))
            {
                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
                var claims = new[]
                {
            new Claim("dni", usuarioValidado.USUARIO) // Puedes ajustar el claim según tus necesidades
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signIn
                );

                return Ok(new
                {
                    success = true,
                    message = "Usuario válido",
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest("Usuario no encontrado o datos inválidos.");
            }
        }

        [HttpPost("GeneralWithDNI")]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> GetGeneralWithDNI([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;
            var incentivosPagos = _incentivosService.GetGeneralIncentivosPagosWithDNI(dni);
            return Ok(incentivosPagos);

        }

        // Resto del código...

        [HttpPost("login")]
        public dynamic LoginDNI([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;
            bool isDniPresent = _incentivosService.IsDniPresent(dni);

            if (isDniPresent)
            {
                string user = dni;

                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
                var claims = new[]
                {
                    new Claim("dni", user)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signIn
                );

                return new
                {
                    success = true,
                    message = "exito",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            else
            {
                return NotFound("No tiene registros");
            }
        }


        

        [HttpPost("GeneralWithDNIConfirmationFalse")]
        public ActionResult<IEnumerable<IncentivoVistaDTO>> GetGeneralWithDNIConfirmationFalse([FromBody] IncentivoPagoRequestDTO request)
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return BadRequest("Token inválido");
            }

            token = token.Substring("Bearer ".Length);

            try
            {
                // Validar el token
                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false, // Desactivar la validación del issuer
                    ValidateAudience = false, // Desactivar la validación de la audiencia
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
                };

                // Validar el token y obtener el ClaimsPrincipal
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Verificar que el token contiene el claim "dni"
                var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

                if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
                {
                    return BadRequest("No se encontró el claim 'dni' en el token.");
                }

                string dni = dniClaim.Value;

                // Continuar con el resto del código para obtener los incentivos con el DNI
                var incentivosVistas = _incentivosService.GetGeneralIncentivosVistasWithDNIConfirmationFalse(dni);

                return Ok(incentivosVistas);
            }
            catch (SecurityTokenException ex)
            {
                // Manejar la excepción de token inválido
                return BadRequest("Token inválido: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones que puedan ocurrir durante la validación
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost("UpdateWithDNI")]
        public IActionResult UpdateConfirmacionEntrega([FromBody] IncentivoPagoRequestDTO request)
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return BadRequest("Token inválido");
            }

            token = token.Substring("Bearer ".Length);

            try
            {
                // Validar el token
                var jwt = _configuration.GetSection("JWT").Get<JWTmodel>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false, // Desactivar la validación del issuer
                    ValidateAudience = false, // Desactivar la validación de la audiencia
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // No permitir un margen de tiempo para la expiración del token
                };

                // Validar el token y obtener el ClaimsPrincipal
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Verificar que el token contiene el claim "dni"
                var dniClaim = principal.Claims.FirstOrDefault(c => c.Type == "dni");

                if (dniClaim == null || string.IsNullOrEmpty(dniClaim.Value))
                {
                    return BadRequest("No se encontró el claim 'dni' en el token.");
                }

                string dni = dniClaim.Value;

                // Continuar con el resto del código para realizar la acción "UpdateConfirmacionEntrega" con el DNI
                int id = (int)request.Id;
                _incentivosService.UpdateConfirmacionEntrega(dni, id);
                return Ok();
            }
            catch (SecurityTokenException ex)
            {
                // Manejar la excepción de token inválido
                return BadRequest("Token inválido: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones que puedan ocurrir durante la validación
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}

