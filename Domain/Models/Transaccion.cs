using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public abstract class Transaccion(
        Guid clienteId,
        int cuentaId,
        decimal monto, 
        DateTime fecha, 
        TipoTransaccionEnum tipoTransaccion)
    {
        public Guid TransaccionId { get; private set; } = Guid.NewGuid();
        public Guid ClienteId { get; private set; } = clienteId;
        public int CuentaId { get; private set; } = cuentaId;
        public decimal Monto { get; private set; } = monto;
        public DateTime Fecha { get; private set; } = fecha;
        public TipoTransaccionEnum TipoTransaccion { get; private set; } = tipoTransaccion;
    }
}
