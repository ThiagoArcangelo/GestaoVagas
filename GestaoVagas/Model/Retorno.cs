using System.Net;

namespace GestaoVagas.Model;

public class Retorno
{
    public int StatusCode { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
