namespace API.Models
{
    public class Produto
    {   

        //C# Contrutor
        public Produto(){
            Id = Guid.NewGuid().ToString();
            CriadoEm = DateTime.Now;
        }

        //Atributos/Propriedades - Caracteristicas
        public string? Id {get; set;}
        public string? Nome {get; set;}
        public double Valor {get; set;}
        public int Quantidade { get; set;}
        public DateTime CriadoEm{get; set;}

    }
}
