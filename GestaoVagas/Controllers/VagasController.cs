using System.Reflection.Metadata.Ecma335;
using GestaoVagas.Model;
using GestaoVagas.Repositories.Interfaces;
using GestaoVagas.Services.Interfaces;
using GestaoVagas.Utils;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestaoVagas.Controllers;

[Route("[controller]")]
[ApiController]

public class VagasController : ControllerBase
{
    private readonly IVagasRepository _vagasRepository;

    public VagasController(IVagasRepository vagasRepository) => _vagasRepository = vagasRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VagasModel>>> RetornaVagas()
    {
        try
        {
            var retorno = await _vagasRepository.GetAll();

            return retorno.Any()
                ? Ok(retorno)
                : NoContent();

        }
        catch (Exception error)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
        }
    }

    [HttpGet("{id: int}")]
    public async Task<ActionResult<VagasModel>> RetornaVagasPorId(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new Retorno { StatusCode = StatusCodes.Status400BadRequest, Mensagem = Constantes.VAGA_NAO_INFORMADA });

            VagasModel retorno = await _vagasRepository.GetById(id);

            return retorno != null ? Ok(retorno) : NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpPost("gravar")]
    public async Task<ActionResult<Retorno>> AddVaga(VagasModel vaga)
    {
        try
        {
            bool retorno = await _vagasRepository.Add(vaga);

            return retorno 
                ? Ok(new Retorno { StatusCode = StatusCodes.Status201Created, Mensagem = Constantes.OPERACAO_ERRO }) 
                : BadRequest(new Retorno { StatusCode = StatusCodes.Status400BadRequest, Mensagem = Constantes.OPERACAO_SUCESSO });
        }
        catch (Exception erro)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, erro.Message);
        }
    }

    [HttpPut("atualizar")]
    public async Task<ActionResult<Retorno>> AtualizaVaga([FromBody] VagasModel vaga)
    {
        try
        {
            
            bool retorno = await _vagasRepository.Update(vaga);

            return retorno
                ? Ok(new Retorno { StatusCode = StatusCodes.Status200OK, Mensagem = Constantes.OPERACAO_SUCESSO })
                : BadRequest(new Retorno { StatusCode = StatusCodes.Status400BadRequest, Mensagem = Constantes.OPERACAO_ERRO });

        }
        catch (Exception erro)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, erro.Message); 
        }
    }

    [HttpDelete("excluir")]
    public async Task<ActionResult<Retorno>> ExcluirVaga([FromQuery] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new Retorno { StatusCode = StatusCodes.Status400BadRequest, Mensagem = Constantes.VAGA_NAO_INFORMADA });

            bool retorno = await _vagasRepository.Delete(id);

            return retorno
                ? Ok(new Retorno { StatusCode = StatusCodes.Status200OK, Mensagem = Constantes.OPERACAO_SUCESSO })
                : BadRequest(new Retorno { StatusCode = StatusCodes.Status400BadRequest, Mensagem = Constantes.OPERACAO_ERRO });
        }
        catch (Exception erro)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, erro.Message);
        }
    }
    }

}
