using GestaoVagas.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GestaoVagas.Services;

public class TokenService
{
    private string ChavePrivada { get; set; } = "8233e47408d7ca8f9865114b2db72a4616e3be0a6e41e7051c35a31fa01dd5fa";

    public string GenerateToken(Usuario user)
    {
        // Cria uma instância do JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();

        // Chave transformada em um array de bytes.
        var key = Encoding.ASCII.GetBytes(ChavePrivada);

        // Assinatura do token
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
        };

        // Gera um Token
        var createToken = handler.CreateToken(tokenDescriptor);

        // Gera uma string do Token
        var token = handler.WriteToken(createToken);

        return token;
    }
}


// handler - é um manipulador responsável por escrever o token e gerar a string do token
//           - Possui os métodos:
//           - CreateToken: cria o token
//           - WriteToken:  escreve a string baseada no token

// A chave simétrica é necessária para que o token possa ser assinado.
// O objeto responsável pela assinatura é o SigningCredentials(SECRECT, ALGORITMO)
// Chave: Deve ser um chave simetrica ex: new SymmetricSecurityKey(KEY).
// O método não aceita uma string no lugar da chave
// A chave deve ser transformada em um array de bytes com Encoding.ASCII.GetBytes(CHAVE)
// Algoritmo: tipo de encriptação ex: SHA256 ou HMAC256, etc...
// TokenDescriptor - o conteúdo concreto do token
// Expire: Data de expiração do token, para controlar o tempo de uso do token. 
// Deve-se registrar o token no builder.Services na classe Program para .Net 6 acima ou start up(.Net 5 ou -)

