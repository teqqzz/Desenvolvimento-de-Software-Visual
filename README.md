# Desenvolvimento de Software Visual
Revisão Completa do Projeto
1. Estrutura do Projeto
O projeto é uma API web construída com o framework ASP.NET Core. O objetivo é manipular uma lista de produtos, permitindo que esses produtos sejam listados, cadastrados, alterados, buscados e removidos por meio de requisições HTTP.

2. Comandos Iniciais para Criar o Projeto
Antes de começar a implementação, os seguintes comandos são usados para configurar o ambiente:

Criar a solução:

bash
Copiar código
dotnet new sln --output Desenvolvimento de Software Visual
Criar o projeto web:

bash
Copiar código
dotnet new web --name API
Adicionar o projeto à solução:

bash
Copiar código
dotnet sln add API
Adicionar suporte ao banco de dados SQLite: Como foi usado o SQLite, é necessário adicionar o pacote:

bash
Copiar código
dotnet add package Microsoft.Data.Sqlite
Restaurar ferramentas e construir o projeto:

bash
Copiar código
dotnet tool restore
dotnet build
Aplicar as migrações e criar o banco de dados:

bash
Copiar código
dotnet ef database update
Agora, vamos revisar o código peça por peça.

3. Produto.cs (Classe Modelo)
Este arquivo define a classe Produto que é usada para representar os produtos no sistema.

csharp
Copiar código
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
Explicação de Cada Linha:
Construtor Produto(): Cada vez que um novo produto é criado, ele recebe um ID exclusivo usando Guid.NewGuid() e registra o horário de criação com DateTime.Now.
Propriedades: As propriedades da classe (Id, Nome, Valor, Quantidade, CriadoEm) representam as características dos produtos. Essas propriedades podem ser acessadas e modificadas durante o ciclo de vida de um produto.
4. Program.cs (Ponto de Entrada da Aplicação)
O arquivo Program.cs contém a configuração da aplicação e define os endpoints da API.

csharp
Copiar código
using API.Models; // Importa a classe Produto.
using Microsoft.AspNetCore.Mvc; // Facilita o uso de controllers e ações para requisições HTTP.

var builder = WebApplication.CreateBuilder(args); // Cria um construtor para o aplicativo.
var app = builder.Build(); // Constrói a aplicação.


// Endpoint: GET /
// Retorna uma string simples como resposta.
app.MapGet("/", () => "Api de Produtos");


// Lista de produtos inicializada como uma lista vazia.
List<Produto> produtos = new List<Produto>();

// Endpoint: GET /api/produto/listar
// Retorna todos os produtos cadastrados.
app.MapGet("/api/produto/listar", () => {
    if (produtos.Count > 0) // Se a lista de produtos tiver elementos...
    {
        return Results.Ok(produtos); // Retorna a lista de produtos com status 200 (OK).
    }
    return Results.NotFound(); // Se a lista estiver vazia, retorna status 404 (Não Encontrado).
});

// Endpoint: POST /api/produto/cadastrar
// Recebe um produto do corpo da requisição e o adiciona à lista.
app.MapPost("/api/produto/cadastrar", ([FromBody] Produto produto) => {
    produtos.Add(produto); // Adiciona o novo produto à lista.
    return Results.Created("", produto); // Retorna status 201 (Criado) e o produto adicionado.
});

// Endpoint: DELETE /api/produto/remove
// Remove um produto da lista, baseado no nome.
app.MapDelete("/api/produto/remove", ([FromBody] Produto produto) => {
    var produtoRemove = produtos.FirstOrDefault(p => p.Nome == produto.Nome); // Busca o produto pelo nome.
    
    if (produtoRemove != null) // Se o produto for encontrado...
    {
        produtos.Remove(produtoRemove); // Remove o produto da lista.
        return Results.Accepted("", produtoRemove); // Retorna status 202 (Aceito) e o produto removido.
    }

    return Results.NotFound("Produto não encontrado."); // Se não for encontrado, retorna status 404 (Não Encontrado).
});

// Endpoint: GET /api/produto/buscar
// Busca um produto pelo nome e o retorna.
app.MapGet("/api/produto/buscar", ([FromBody] Produto produto) => {
    var produtoBuscar = produtos.FirstOrDefault(p => p.Nome == produto.Nome); // Busca pelo nome.

    if (produtoBuscar != null) // Se encontrado...
    {
        return Results.Ok(produtoBuscar); // Retorna o produto.
    }

    return Results.NotFound("Produto não encontrado."); // Se não for encontrado, retorna status 404 (Não Encontrado).
});

// Endpoint: PUT /api/produto/alterar
// Altera um produto existente na lista, baseado no nome.
app.MapPut("/api/produto/alterar", ([FromBody] Produto produto) => {
    var produtoAlterar = produtos.FirstOrDefault(p => p.Nome == produto.Nome); // Busca o produto.

    if (produtoAlterar != null) { 
        // Atualiza os valores do produto, se fornecidos.
        if (produto.Valor != 0) produtoAlterar.Valor = produto.Valor;
        if (produto.Quantidade != 0) produtoAlterar.Quantidade = produto.Quantidade;
        return Results.Ok(produtoAlterar); // Retorna o produto alterado.
    }

    return Results.NotFound("Produto não encontrado."); // Se não encontrado, retorna status 404 (Não Encontrado).
});

app.Run(); // Inicia a aplicação.
Explicação Geral:
Mapeamento de Endpoints:
Cada endpoint usa métodos como MapGet, MapPost, MapDelete e MapPut para definir rotas HTTP específicas (GET, POST, DELETE, PUT).
A API aceita dados via corpo da requisição (indicado por [FromBody]).
Lista de Produtos: A lista produtos funciona como um "banco de dados" temporário em memória para armazenar os produtos durante a execução da API.
Explicação de Cada Endpoint:
GET /api/produto/listar: Retorna a lista de todos os produtos. Se não houver produtos, retorna um erro 404.
POST /api/produto/cadastrar: Adiciona um novo produto à lista, usando os dados enviados no corpo da requisição.
DELETE /api/produto/remove: Remove um produto da lista baseado no nome.
GET /api/produto/buscar: Busca um produto específico pelo nome e o retorna.
PUT /api/produto/alterar: Altera os dados de um produto existente na lista, se encontrado.
5. teste.http (Arquivo de Testes)
Este arquivo é usado para testar a API, simulando requisições HTTP para os diversos endpoints.

Exemplos:
GET /api/produto/listar: Testa a listagem de todos os produtos.
POST /api/produto/cadastrar: Simula o cadastro de novos produtos.
DELETE /api/produto/remove: Simula a remoção de produtos pelo nome.
GET /api/produto/buscar: Testa a busca de um produto pelo nome.
PUT /api/produto/alterar: Simula a alteração dos dados de um produto existente.
6. Como Executar o Projeto
Certifique-se de que o .NET esteja instalado.
Navegue até a pasta raiz do projeto e execute os seguintes comandos:
bash
Copiar código
dotnet build
dotnet run
Acesse a aplicação em http://localhost:5216 e use ferramentas como o Postman ou o arquivo teste.http para realizar testes na API.

