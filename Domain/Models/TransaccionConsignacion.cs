namespace BluesoftBank.Domain.Models
{
    public class TransaccionConsignacion : Transaccion
    {
        private TransaccionConsignacion(
            Guid clienteId,
            int cuentaId,
            decimal monto,
            DateTime fecha)
            : base(clienteId, cuentaId, monto, fecha, Enums.TipoTransaccionEnum.Deposito)
        {
        }

        public string CiudadConsignacion { get; private set; } = null!;

        public TransaccionConsignacion Crear(
            Guid clienteId, 
            int cuentaId, 
            decimal monto, 
            string ciudadConsignacion)
        {
            return new TransaccionConsignacion(clienteId, cuentaId, monto, DateTime.Now)
            {
                CiudadConsignacion = ciudadConsignacion
            };
        }
    }
}
