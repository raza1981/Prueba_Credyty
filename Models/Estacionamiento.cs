using System;
using System.Collections.Generic;

namespace ParqueaderoApi.Models
{
    public partial class Estacionamiento
    {
        public long Id { get; set; }
        public string Placa { get; set; } = null!;
        public DateTime? Ingreso { get; set; }
        public DateTime? Salida { get; set; }
        public long IdTipoVehiculo { get; set; }
        public bool? AplicaDescuento { get; set; }

        public virtual TipoVehiculo IdTipoVehiculoNavigation { get; set; } = null!;
    }
}
