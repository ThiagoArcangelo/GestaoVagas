namespace GestaoVagas.Models;

public class ResponseEntity<T>
{
    public T? Dados { get; set; }
    public string?  Mensagem { get; set; }
    public bool Status { get; set; }
}
