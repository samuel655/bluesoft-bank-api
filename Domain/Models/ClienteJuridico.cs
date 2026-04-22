using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public class ClienteJuridico : Cliente
    {
        private ClienteJuridico(string nombre, string correo, TipoPersonaEnum tipoPersona) 
            : base(nombre, correo, tipoPersona)
        {
        }
        public string RazonSocial { get; private set; } = null!;
        public string Nit { get; private set; } = null!;
        public ICollection<CuentaCorriente> Cuentas { get; private set; } = null!;
        public ClienteJuridico Crear(
            string nombre, 
            string correo, 
            string razonSocial, 
            string nit,
            List<CuentaCorriente> cuentas)
        {
            return new ClienteJuridico(nombre, correo, TipoPersonaEnum.Juridica)
            {
                RazonSocial = razonSocial,
                Nit = nit,
                Cuentas = cuentas
            };
        }
    }
}
