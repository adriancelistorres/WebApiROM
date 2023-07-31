namespace WebApiRestNetCore.DTO.DtoIncentivo
{
    public class IncentivoVistaDTO
    {
        public int id { get; set; }
        public string PeriodoIncentivo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string DniPromotor { get; set; }
        public string NombreCompleto { get; set; }
        public string PUNTOVENTA { get; set; }
        public int IdIncentivo { get; set; }
        public string NombreIncentivo { get; set; }
        public string Empresa { get; set; }
        public decimal Monto { get; set; }
        public string EstadoIncentivo { get; set; }



    }
}
