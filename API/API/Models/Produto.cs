namespace API.Models
{
    public class Produto
    {   
        //Atributos/Propriedades - Caracteristicas
        public string? Id {get; set;}
        public string? Nome {get; set;}
        public double Valor {get; set;}
        public int Quantidade { get; set;}
        public DateTime CriadoEm {get; set;} = DateTime.Now;

    }
}
