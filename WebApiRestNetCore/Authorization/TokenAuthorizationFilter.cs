using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;


namespace WebApiRestNetCore.Authorization
{
    public class TokenAuthorizationFilter
    {
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    // Obtener el token del encabezado Authorization
        //    string token = context.HttpContext.Request.Headers["Authorization"];

        //    // Verificar si el token es válido o tiene el formato adecuado (debes implementar la lógica para validar el token JWT)
        //    bool isValidToken = ValidateToken(token);

        //    if (!isValidToken)
        //    {
        //        // Devolver un resultado de acceso no autorizado
        //        context.Result = new UnauthorizedResult();
        //    }
        //}
    }
}
