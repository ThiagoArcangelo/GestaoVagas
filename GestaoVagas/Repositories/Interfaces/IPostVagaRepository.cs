using GestaoVagas.Models;

namespace GestaoVagas.Services.Interfaces;

public interface IPostVagaRepository
{
    Task<IEnumerable<VagasEntity>> RetornaVagas();
    Task<VagasEntity> RetornaVagasPorId(int id);
    Task<string> Gravar();
    Task<string> Atualizar();
    Task<string> Excluir();
}
