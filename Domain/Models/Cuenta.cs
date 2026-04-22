using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public abstract class Cuenta(
        int numeroCuenta,
        Guid clienteId,
        decimal saldo,
        string ciudadOrigen,
        TipoCuentaEnum tipoCuenta)
    {
        public int CuentaId { get; set; }
        public int NumeroCuenta { get; private set; } = numeroCuenta;
        public Guid ClienteId { get; private set; } = clienteId;
        public decimal Saldo { get; private set; } = saldo;
        public string CiudadOrigen { get; private set; } = ciudadOrigen;
        public TipoCuentaEnum TipoCuenta { get; private set; } = tipoCuenta;
    }
}
