﻿namespace WebApiRestNetCore.Models
{
    public class AperturaPDV
    {
        public int? ID { get; set; }

        public string? JefedeVenta { get; set; }
        public string? Supervisor { get; set; }
        public string? PDV { get; set; }
        public string? Fecha { get; set; }
        public string? HoraApertura { get; set; }
        public string? HoraCierre { get; set; }
        public DateTime? FechaHoraTransaccion { get; set; } // Nueva propiedad para la fecha y hora de transacción

    }
}
