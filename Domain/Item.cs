using System.Text.Json.Serialization;

namespace Domain
{
    public class Item
    {
        public int Id { get; set; }
        public string? Alimento { get; set; }
        public string Unidade { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string? Classificacao { get; set; }
        public int Posicao { get; set; }
        public int NumeroAndar { get; set; }
        public int NumeroContainer { get; set; }
    }
}