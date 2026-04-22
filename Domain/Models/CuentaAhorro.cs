using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public class CuentaAhorro : Cuenta
    {
        private CuentaAhorro(int numeroCuenta, Guid clienteId, decimal saldo, string ciudadOrigen) 
            : base(numeroCuenta, clienteId, saldo, ciudadOrigen, TipoCuentaEnum.Ahorro)
        {
        }
        public string DocumentoTitular { get; private set; } = null!;
        public DateOnly FechaApertura { get; private set; }
        public CuentaAhorro Crear(
            int numeroCuenta,
            Guid clienteId,
            decimal saldo, 
            string ciudadOrigen,
            string documentoTitular,
            DateOnly fechaApertura)
        {
            return new CuentaAhorro(numeroCuenta, clienteId, saldo, ciudadOrigen)
            {
                DocumentoTitular = documentoTitular,
                FechaApertura = fechaApertura
            };
        }
    }
}
