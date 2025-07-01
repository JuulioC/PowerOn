using System.Collections.Generic; // Necessário para Dictionary

namespace PowerOn.Utils // Assumindo que está na pasta Utils
{
    public static class TiposEnum
    {
        public static string EnumPerfil(int id)
        {
            var perfilMap = new Dictionary<int, string>
            {
                { 0, "MAXMOBILITY" },
                { 1, "CONSULTOR" },
                { 2, "GERENTE" },
                { 3, "ADMINISTRADOR" },
                { 4, "ISOTECH" }
            };

            return perfilMap.TryGetValue(id, out var perfil) ? perfil : "DESCONHECIDO";
        }
    }
}
