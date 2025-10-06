using GestaoVagas.Model;
using GestaoVagas.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace GestaoVagas.Repositories.Interfaces;

public interface IVagasRepository : IRepository<VagasModel>
{

}
