using BluesoftBank.Domain.Enums;

namespace BluesoftBank.Domain.Models
{
    public abstract class Cliente(string nombre, string correo, TipoPersonaEnum tipoPersona)
    {
        public Guid ClienteId { get; private set; } = Guid.NewGuid();
        public string Nombre { get; private set; } = nombre;
        public string Correo { get; private set; } = correo;
        public TipoPersonaEnum TipoPersona { get; private set; } = tipoPersona;
    }
}
