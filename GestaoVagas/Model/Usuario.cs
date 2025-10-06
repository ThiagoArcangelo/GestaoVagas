using System.Text.Json.Serialization;

namespace GestaoVagas.Model;

public class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string UserName { get; set; }
    public string[] Roles { get; set; }

    [JsonIgnore]
    public string Senha { get; set; } = string.Empty;
    public bool Ativo { get; set; } 
    
}
