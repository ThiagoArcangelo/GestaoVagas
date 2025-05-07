using GestaoVagas.Entities;
using GestaoVagas.Models;

namespace GestaoVagas.Services.Interfaces;

public interface IPostVagaRepository
{
    Task<IEnumerable<VagasEntity>> RetornaVagas();
    Task<VagasEntity> RetornaVagasPorId(int id);
    Task<bool> Gravar(VagasEntity entity);
    Task<bool> Atualizar(VagasEntity entity);
    Task<bool> Excluir(int id);
}
