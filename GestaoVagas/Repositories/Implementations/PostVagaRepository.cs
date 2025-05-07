using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using GestaoVagas.Entities;
using GestaoVagas.Services.Interfaces;
using Npgsql;

namespace GestaoServices.Implementations;

public class PostVagaRepository : IPostVagaRepository
{
    StringBuilder sql;
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
        VagasEntity vagas = new();

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

            using (var cn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("ID", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;

                    var rd = await cmd.ExecuteReaderAsync();

                    if (rd.HasRows)
                    {

                        vagas.Titulo = Convert.ToString(rd["TITULO"]);
                        vagas.Descricao = Convert.ToString(rd["DESCRICAO"]);
                        vagas.Confidencial = Convert.ToChar(rd["CONFIDENCIAL"]);
                        vagas.Empresa = Convert.ToString(rd["EMPRESA"]);
                        vagas.TipoContato = Convert.ToChar(rd["CONTRATO"]);
                        vagas.Cidade = Convert.ToString(rd["CIDADE"]);
                        vagas.Salario = Convert.ToDouble(rd["SALARIO"]);
                        vagas.Email = Convert.ToString(rd["EMAIL"]);
                        vagas.DataExp = Convert.ToDateTime(rd["EXPIRACAO"]);
                        vagas.Pcd = Convert.ToChar(rd["PCD"]);                           

                    }

                    return vagas;
                }
            }
        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }

    }

    public async Task<bool> Gravar(VagasEntity obj)
    {
        bool retorno = false;

        try
        {
            sql = new();
            
            sql.Append("INSERT INTO LEBCAD     ");
            sql.Append("(                      ");
            sql.Append("    LEB_TITULO,        ");
            sql.Append("    LEB_DESCRICAO,     ");
            sql.Append("    LEB_CONFIDENCIAL,  ");
            sql.Append("    LEB_NOMEEMPRESA,   ");
            sql.Append("    LEB_TIPOCONTRATO,  ");
            sql.Append("    LEB_CIDADE,        ");
            sql.Append("    LEB_SALARIO,       ");
            sql.Append("    LEB_EMAIL,         ");
            sql.Append("    LEB_DATAEXP,       ");
            sql.Append("    LEB_PCD            ");
            sql.Append(")                      ");
            sql.Append("VALUES                 ");
            sql.Append("(                      ");
            sql.Append("    @TITULO,           ");
            sql.Append("    @DESCRICAO,        ");
            sql.Append("    @CONFIDENCIAL,     ");
            sql.Append("    @NOMEEMPRESA,      ");
            sql.Append("    @TIPOCONTRATO,     ");
            sql.Append("    @CIDADE,           ");
            sql.Append("    @SALARIO,          ");
            sql.Append("    @EMAIL,            ");
            sql.Append("    @DATAEXP,          ");
            sql.Append("    @PCD               ");
            sql.Append(")                      ");

            using (var cn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("TITULO", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Titulo;
                    cmd.Parameters.Add("DESCRICAO", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Descricao;
                    cmd.Parameters.Add("CONFIDENCIAL", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.Confidencial;
                    cmd.Parameters.Add("NOMEEMPRESA", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Empresa;
                    cmd.Parameters.Add("TIPOCONTRATO", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.TipoContato;
                    cmd.Parameters.Add("CIDADE", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Cidade;
                    cmd.Parameters.Add("SALARIO", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.Salario;
                    cmd.Parameters.Add("EMAIL", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Email;
                    cmd.Parameters.Add("DATAEXP", NpgsqlTypes.NpgsqlDbType.Date).Value = obj.DataExp;
                    cmd.Parameters.Add("PCD", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.Pcd;

                    var rowsAff = await  cmd.ExecuteNonQueryAsync();

                    if (rowsAff > 0)
                        retorno = true;

                }
            }
        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }

        return retorno;
    }


    public async Task<bool> Atualizar(VagasEntity obj)
    {
        bool retorno = false;

        try
        {
            sql = new();

            sql.Append("UPDATE LEBCAD SET                        ");
            sql.Append("       LEB_TITULO = @TITULO,             ");
            sql.Append("       LEB_DESCRICAO = @DESCRICAO,       ");
            sql.Append("       LEB_CONFIDENCIAL = @CONFIDENCIAL, ");
            sql.Append("       LEB_NOMEEMPRESA = @NOMEEMPRESA,   ");
            sql.Append("       LEB_TIPOCONTRATO = @TIPOCONTRATO, ");
            sql.Append("       LEB_CIDADE = @CIDADE,             ");
            sql.Append("       LEB_SALARIO = @SALARIO,           ");
            sql.Append("       LEB_EMAIL = @EMAIL,               ");
            sql.Append("       LEB_DATAEXP = @DATAEXP,           ");
            sql.Append("       LEB_PCD  = @PCD                   ");
            sql.Append(" WHERE LEB_ID = @ID                      ");

            using (var cn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("TITULO", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Titulo;
                    cmd.Parameters.Add("DESCRICAO", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Descricao;
                    cmd.Parameters.Add("CONFIDENCIAL", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.Confidencial;
                    cmd.Parameters.Add("NOMEEMPRESA", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Empresa;
                    cmd.Parameters.Add("TIPOCONTRATO", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.TipoContato;
                    cmd.Parameters.Add("CIDADE", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Cidade;
                    cmd.Parameters.Add("SALARIO", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.Salario;
                    cmd.Parameters.Add("EMAIL", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.Email;
                    cmd.Parameters.Add("DATAEXP", NpgsqlTypes.NpgsqlDbType.Date).Value = obj.DataExp;
                    cmd.Parameters.Add("PCD", NpgsqlTypes.NpgsqlDbType.Char).Value = obj.Pcd;
                    cmd.Parameters.Add("ID", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.Id;

                    var rowsAff = await cmd.ExecuteNonQueryAsync();

                    if (rowsAff > 0)
                        retorno = true;

                }
            }

        }
        catch (Exception erro)
        {

            throw new Exception(erro.Message);
        }

        return retorno;
    }

    public async Task<bool> Excluir(int id)
    {
        bool retorno = false;

        try
        {
            sql = new();

            sql.Append("DELETE FROM LEBCAD  ");
            sql.Append(" WHERE LEB_ID = @ID ");

            using (var cn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("ID", NpgsqlTypes.NpgsqlDbType.Char).Value = id;

                    var rowsAff = await cmd.ExecuteNonQueryAsync();

                    if (rowsAff > 0)
                        retorno = true;
                }
            }


        }
        catch (Exception error)
        {

            throw new Exception(error.Message);
        }

        return retorno;
    }

    Task<IEnumerable<VagasEntity>> IPostVagaRepository.RetornaVagas()
    {
        throw new NotImplementedException();
    }

}
