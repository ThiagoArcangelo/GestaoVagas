using System.Text;
using Dapper;
using GestaoModels;
using GestaoServices.Interfaces;
using Npgsql;

namespace GestaoServices.Implementations;

public class PostVagaRepository : IPostVagaRepository
{
    StringBuilder sql = null;
    private readonly IConfiguration _configuration;   

    public PostVagaRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<VagasEntity>> RetornaVagas()
    {
        IEnumerable<VagasEntity> vagas ;

        try
        {
            using (var con = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                sql = new();

                sql.Append("SELECT LEB_ID           AS CODIGO,        ");
                sql.Append("       LEB_TITULO       AS TITULO,       ");
                sql.Append("       LEB_DESCRICAO    AS DESCRICAO,    ");
                sql.Append("       LEB_CONFIDENCIAL AS CONFIDENCIAL, ");
                sql.Append("       LEB_NOMEEMPRESA  AS EMPRESA,      ");
                sql.Append("       LEB_TIPOCONTRATO AS CONTRATO,     ");
                sql.Append("       LEB_CIDADE       AS CIDADE,       ");
                sql.Append("       COALESCE(TO_CHAR(LEB_SALARIO, 'FM999999999,00'), '0,00') AS SALARIO, ");
                sql.Append("       LEB_EMAIL        AS EMAIL,        ");
                sql.Append("       LEB_DATAEXP      AS EXPIRACAO,    ");
                sql.Append("       CASE                              ");
                sql.Append("         WHEN LEB_PCD = 'S' THEN 'SIM'   ");
                sql.Append("         WHEN LEB_PCD = 'N' THEN 'NÃO'   ");
                sql.Append("         ELSE NULL                       ");
                sql.Append("       END AS PCD                        ");
                sql.Append("  FROM LEBCAD;                           ");

                vagas = await con.QueryAsync<VagasEntity>(sql.ToString());           
              
            }
        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }

        return vagas;
    }

    public async Task<VagasEntity> RetornaVagasPorId(int id)
    {
        VagasEntity vagas;

        try
        {
            sql = new();

            sql.Append("SELECT LEB_ID           AS CODIGO,        ");
            sql.Append("       LEB_TITULO       AS TITULO,       ");
            sql.Append("       LEB_DESCRICAO    AS DESCRICAO,    ");
            sql.Append("       LEB_CONFIDENCIAL AS CONFIDENCIAL, ");
            sql.Append("       LEB_NOMEEMPRESA  AS EMPRESA,      ");
            sql.Append("       LEB_TIPOCONTRATO AS CONTRATO,     ");
            sql.Append("       LEB_CIDADE       AS CIDADE,       ");
            sql.Append("       COALESCE(TO_CHAR(LEB_SALARIO, 'FM999999999,00'), '0,00') AS SALARIO, ");
            sql.Append("       LEB_EMAIL        AS EMAIL,        ");
            sql.Append("       LEB_DATAEXP      AS EXPIRACAO,    ");
            sql.Append("       CASE                              ");
            sql.Append("         WHEN LEB_PCD = 'S' THEN 'SIM'   ");
            sql.Append("         WHEN LEB_PCD = 'N' THEN 'NÃO'   ");
            sql.Append("         ELSE NULL                       ");
            sql.Append("       END AS PCD                        ");
            sql.Append("  FROM LEBCAD;                           ");
            sql.Append(" WHERE LEB_ID = @ID                      ");

            using (var con = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("ID", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;

                    var rd = await cmd.ExecuteReaderAsync();

                    if (rd.HasRows)
                    {
                        vagas = new() 
                        {
                            Titulo = Convert.ToString(rd["TITULO"]),
                            Descricao = Convert.ToString(rd["DESCRICAO"]),
                            Confidencial = Convert.ToChar(rd["CONFIDENCIAL"]),
                            Empresa = Convert.ToString(rd["EMPRESA"]),
                            TipoContato = Convert.ToChar(rd["CONTRATO"]),
                            Cidade = Convert.ToString(rd["CIDADE"]),
                            Salario = Convert.ToDouble(rd["SALARIO"]),
                            Email = Convert.ToString(rd["EMAIL"]),
                            Expiracao = Convert.ToDateTime(rd["EXPIRACAO"]),
                            Pcd = Convert.ToChar(rd["PCD"]),
                        };                        
                    }

                    rd.Close();

                }
            }
        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }
    }
    public Task<string> Gravar()
    {
        try
        {

        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }
    }


    public Task<string> Atualizar()
    {
        try
        {

        }
        catch (Exception erro)
        {

            throw new Exception(erro.Message);
        }
    }

    public Task<string> Excluir()
    {
        try
        {

        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }
    }
}
