using BCrypt.Net;

public static class UsuarioHelper
{
    // Método para gerar o hash da senha
    public static string GerarHashDaSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    // Método para verificar se a senha está correta
    public static bool VerificarSenha(string senhaDigitada, string senhaHashArmazenada)
    {
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaHashArmazenada);
    }
}
