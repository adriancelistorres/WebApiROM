namespace WebApiRestNetCore.DTO.DtoIncentivo
{
    public class IncentivoPagoDTO
    {
        public string Descripcion { get; set; }
        public string Empresa { get; set; }
        public decimal Monto { get; set; }
        public bool ConfirmacionEntrega { get; set; }

    }

}
