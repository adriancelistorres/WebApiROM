namespace WebApiRestNetCore.DTO.DtoIncentivo
{
    public class UsuarioDTO
    {
        public int? IDUSUARIO { get; set; }
        public string? NOMBRES { get; set; }
        public string? APELLIDOPATERNO { get; set; }
        public string? APELLIDOMATERNO { get; set; }
        public string? JERARQUIA { get; set; }
        public int? IDJERARQUIA { get; set; }
        public string? CORREO { get; set; }
        public string? USUARIO { get; set; }
        public string? COD_NEGOCIO { get; set; }
        public string? COD_CUENTA { get; set; }
        public string? COD_PAIS { get; set; }
        public string? CLAVE { get; set; }
        public string?   ES_ADMIN { get; set; }
        public string? TOKEN { get; set; }
    }
}
