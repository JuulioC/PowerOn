using PowerOn.Utils; // Certifique-se de que este using está presente e que você adicionou a referência ao projeto PowerOn
using System;

namespace HashGeneratorConsole // O namespace do seu novo projeto de console
{
    class Program
    {
        static void Main(string[] args)
        {
            string senhaTextoPuro = "123456";
            string senhaHasheada = PasswordHasher.HashPassword(senhaTextoPuro);
            Console.WriteLine($"A senha '{senhaTextoPuro}' hasheada com BCrypt é: {senhaHasheada}");
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey(); // Para manter a janela aberta até você copiar o hash
        }
    }
}