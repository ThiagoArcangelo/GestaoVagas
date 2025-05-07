namespace GestaoVagas.Entities;

public class VagasEntity
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public char Confidencial { get; set; }
    public string? Empresa { get; set; }
    public char TipoContato { get; set; }
    public string? Cidade { get; set; }
    public double Salario { get; set; }
    public string? Email { get; set; }
    public DateTime DataExp { get; set; }
    public char Pcd { get; set; }
}
