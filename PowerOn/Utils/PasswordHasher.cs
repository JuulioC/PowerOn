using BCrypt.Net; // Importa o namespace do BCrypt
using System;

namespace PowerOn.Utils // Ou PowerOn.Services, se preferir uma camada de serviço
{
    public static class PasswordHasher
    {
        // Este método gera um hash seguro da senha
        public static string HashPassword(string password)
        {
            // BCrypt gera um salt aleatório automaticamente para cada hash,
            // tornando-o mais seguro contra ataques de rainbow table.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Este método verifica uma senha em texto puro contra um hash existente
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // BCrypt verifica se a senha fornecida corresponde ao hash,
            // lidando com o salt e o algoritmo internamente.
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Lidar com hashes que não estão no formato BCrypt.
                // Isso é crucial para a migração de senhas antigas.
                // Neste ponto, você pode retornar false ou logar um erro.
                return false;
            }
            catch (Exception)
            {
                // Lidar com outras exceções genéricas
                return false;
            }
        }
    }
}