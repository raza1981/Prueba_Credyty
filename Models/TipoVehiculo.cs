using System;
using System.Collections.Generic;

namespace ParqueaderoApi.Models
{
    public partial class TipoVehiculo
    {
        public TipoVehiculo()
        {
            Estacionamientos = new HashSet<Estacionamiento>();
        }

        public long IdTipoVehiculo { get; set; }
        public string? Tipo { get; set; }
        public int? Tarifa { get; set; }

        public virtual ICollection<Estacionamiento> Estacionamientos { get; set; }
    }
}
