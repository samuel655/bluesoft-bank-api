using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public class CuentaCorriente : Cuenta
    {
        private CuentaCorriente(int numeroCuenta, Guid clienteId, decimal saldo, string ciudadOrigen) 
            : base(numeroCuenta, clienteId, saldo, ciudadOrigen, TipoCuentaEnum.Corriente)
        {
        }
        public string NitEmpresa { get; private set; } = null!;
        public DateOnly FechaApertura { get; private set; }
        public CuentaCorriente Crear(
            int numeroCuenta,
            Guid clienteId,
            decimal saldo, 
            string ciudadOrigen,
            string nitEmpresa,
            DateOnly fechaApertura)
        {
            return new CuentaCorriente(numeroCuenta, clienteId, saldo, ciudadOrigen)
            {
                NitEmpresa = nitEmpresa,
                FechaApertura = fechaApertura
            };
        }
    }
}
