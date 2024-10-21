# Revisão Completa do Projeto

## 1. Estrutura do Projeto
O projeto é uma API web construída com o framework **ASP.NET Core**. O objetivo é manipular uma lista de produtos, permitindo que esses produtos sejam listados, cadastrados, alterados, buscados e removidos por meio de requisições HTTP.

## 2. Comandos Iniciais para Criar o Projeto
Antes de começar a implementação, os seguintes comandos são usados para configurar o ambiente:

### Criar a solução:
```
dotnet new sln --output "Desenvolvimento de Software Visual"
```
### Criar o projeto web:
```
dotnet new web --name API
```
### Adicionar o Projeto à Solução:
```
dotnet sln add API
```

### Adicionar suporte ao banco de dados SQLite:
```bash
dotnet tool install --global dotnet-ef
dotnet ef migration add InialCreate
dotnet add package Microsoft.Data.Sqlite
```
### Restaurar ferramentas e construir o projeto:
```
dotnet tool restore
dotnet build
```

### 3.Banco.cs (Contexto do Banco de Dados)
```
using Microsoft.EntityFrameworkCore;
namespace API.Models;
public class Banco : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Ecommerce.db");
    }
}
```
### Explicação:
DbContext: A classe Banco herda de DbContext, que é a classe base para trabalhar com bancos de dados usando Entity Framework Core.
DbSet<Produto>: Representa a tabela de produtos no banco de dados, permitindo operações CRUD (Criar, Ler, Atualizar, Excluir).
OnConfiguring: Configura o provedor de banco de dados (neste caso, SQLite) e define o arquivo de banco de dados como Ecommerce.db.

### 4. Produto.cs (Classe Modelo)
Este arquivo define a classe Produto que é usada para representar os produtos no sistema.

```
namespace API.Models
{
    public class Produto
    {   
        // Construtor da classe Produto
        public Produto(){
            Id = Guid.NewGuid().ToString(); // Gera um identificador único para cada produto.
            CriadoEm = DateTime.Now; // Armazena a data e hora de criação do produto.
        }

        // Atributos/Propriedades - Características de cada produto
        public string? Id { get; set; } // Identificador único do produto.
        public string? Nome { get; set; } // Nome do produto.
        public double Valor { get; set; } // Valor/preço do produto.
        public int Quantidade { get; set; } // Quantidade em estoque do produto.
        public DateTime CriadoEm { get; set; } // Data de criação do produto.
    }
}
```
### Explicação:
Construtor Produto(): Cada vez que um novo produto é criado, ele recebe um ID exclusivo usando Guid.NewGuid() e registra o horário de criação com DateTime.Now.
Propriedades: As propriedades da classe (Id, Nome, Valor, Quantidade, CriadoEm) representam as características dos produtos. Essas propriedades podem ser acessadas e modificadas durante o ciclo de vida de um produto.

### 5. Program.cs (Ponto de Entrada da Aplicação)

```
O arquivo Program.cs contém a configuração da aplicação e define os endpoints da API.

using API.Models; // Importa a classe Produto.
using Microsoft.AspNetCore.Mvc; // Facilita o uso de controllers e ações para requisições HTTP.

var builder = WebApplication.CreateBuilder(args); // Cria um construtor para o aplicativo.
var app = builder.Build(); // Constrói a aplicação.

// Endpoint: GET /
app.MapGet("/", () => "Api de Produtos");

// Lista de produtos inicializada como uma lista vazia.
List<Produto> produtos = new List<Produto>();

// Endpoint: GET /api/produto/listar
app.MapGet("/api/produto/listar", () => {
    if (produtos.Count > 0) 
    {
        return Results.Ok(produtos); 
    }
    return Results.NotFound(); 
});

// Endpoint: POST /api/produto/cadastrar
app.MapPost("/api/produto/cadastrar", ([FromBody] Produto produto) => {
    produtos.Add(produto); 
    return Results.Created("", produto); 
});

// Endpoint: DELETE /api/produto/remove
app.MapDelete("/api/produto/remove", ([FromBody] Produto produto) => {
    var produtoRemove = produtos.FirstOrDefault(p => p.Nome == produto.Nome); 
    if (produtoRemove != null) 
    {
        produtos.Remove(produtoRemove); 
        return Results.Accepted("", produtoRemove); 
    }
    return Results.NotFound("Produto não encontrado."); 
});

// Endpoint: GET /api/produto/buscar
app.MapGet("/api/produto/buscar", ([FromBody] Produto produto) => {
    var produtoBuscar = produtos.FirstOrDefault(p => p.Nome == produto.Nome); 
    if (produtoBuscar != null) 
    {
        return Results.Ok(produtoBuscar); 
    }
    return Results.NotFound("Produto não encontrado."); 
});

// Endpoint: PUT /api/produto/alterar
app.MapPut("/api/produto/alterar", ([FromBody] Produto produto) => {
    var produtoAlterar = produtos.FirstOrDefault(p => p.Nome == produto.Nome); 
    if (produtoAlterar != null) { 
        if (produto.Valor != 0) produtoAlterar.Valor = produto.Valor;
        if (produto.Quantidade != 0) produtoAlterar.Quantidade = produto.Quantidade;
        return Results.Ok(produtoAlterar); 
    }
    return Results.NotFound("Produto não encontrado."); 
});

app.Run(); // Inicia a aplicação.
```
### Explicação:
Mapeamento de Endpoints: Cada endpoint usa métodos como MapGet, MapPost, MapDelete e MapPut para definir rotas HTTP específicas (GET, POST, DELETE, PUT).
Lista de Produtos: A lista produtos funciona como um "banco de dados" temporário em memória para armazenar os produtos durante a execução da API.
Explicação de Cada Endpoint:
GET /api/produto/listar: Retorna a lista de todos os produtos. Se não houver produtos, retorna um erro 404.
POST /api/produto/cadastrar: Adiciona um novo produto à lista, usando os dados enviados no corpo da requisição.
DELETE /api/produto/remove: Remove um produto da lista baseado no nome.
GET /api/produto/buscar: Busca um produto específico pelo nome e o retorna.
PUT /api/produto/alterar: Altera os dados de um produto existente na lista, se encontrado.

### 6. teste.http (Arquivo de Testes)
Este arquivo é usado para testar a API, simulando requisições HTTP para os diversos endpoints.
```
###Listar Produtos:
GET /api/produto/listar
###Cadastrar Produto:
POST /api/produto/cadastrar
Content-Type: application/json

{
    "nome": "Mouse Bluetooth",
    "valor": 50.00,
    "quantidade": 50
}
###Cadastrar Outro Produto:
POST /api/produto/cadastrar
Content-Type: application/json

{
    "nome": "Cadeira Gamer",
    "valor": 800.00,
    "quantidade": 15
}
###Remover Produto Mouse Bluetooth:
DELETE /api/produto/remove
Content-Type: application/json

{
    "nome": "Mouse Bluetooth"
}
###Remover Produto Cadeira Gamer:
DELETE /api/produto/remove
Content-Type: application/json

{
    "nome": "Cadeira Gamer"
}
###Buscar Produto:
GET /api/produto/buscar
Content-Type: application/json

{
    "nome": "Cadeira Gamer"
}
###Alterar Produto:
http
PUT /api/produto/alterar
Content-Type: application/json

{
    "nome": "Cadeira Gamer",
    "valor": 850.00,
    "quantidade": 10
}
```
### 7. Como Executar o Projeto
Certifique-se de que o .NET esteja instalado.
Navegue até a pasta raiz do projeto e execute os seguintes comandos:
```
dotnet build
dotnet ef database update
dotnet run
```

Acesse a aplicação em http://localhost:5216 e use ferramentas como o Postman ou o arquivo teste.http para realizar testes na API.

