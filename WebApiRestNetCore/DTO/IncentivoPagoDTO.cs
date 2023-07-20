namespace WebApiRestNetCore.DTO
{
    public class IncentivoPagoDTO
    {
        public string Descripcion { get; set; }
        public string Empresa { get; set; }
        public decimal Monto { get; set; }
        public Boolean ConfirmacionEntrega { get; set; }

    }

}
