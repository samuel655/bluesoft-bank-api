using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public class ClienteNatural : Cliente
    {
        private ClienteNatural(string nombre, string correo, TipoPersonaEnum tipoPersona) 
            : base(nombre, correo, tipoPersona)
        {
        }

        public string Documento { get; private set; } = null!;
        public DateOnly Nacimiento { get; private set; }
        public ICollection<CuentaAhorro> Cuentas { get; private set; } = null!;

        public ClienteNatural Crear(
            string nombre, 
            string correo, 
            string documento, 
            DateOnly nacimiento,
            List<CuentaAhorro> cuentas)
        {
            return new ClienteNatural(nombre, correo, TipoPersonaEnum.Natural)
            {
                Documento = documento,
                Nacimiento = nacimiento,
                Cuentas = cuentas
            };
        }
    }
}
