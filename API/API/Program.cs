using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint - Funcionalidade
// Requisição - url e metodo/verbo HTTP
// GET: /
app.MapGet("/", () => "Api de Produtos");


// Criação de uma lista de produtos feita pelo chat
List<Produto> produtos = new List<Produto>{
    // Adicionando produtos à lista
    // new Produto { Nome = "Notebook", Valor = 3500.00, Quantidade = 10 },
    // new Produto { Nome = "Mouse", Valor = 100.00, Quantidade = 50 },
    // new Produto { Nome = "Teclado", Valor = 150.00, Quantidade = 30 },
    // new Produto { Nome = "Monitor", Valor = 900.00, Quantidade = 20 },
    // new Produto { Nome = "Impressora", Valor = 600.00, Quantidade = 15}
};

// GET: /api/produto/listar
app.MapGet("/api/produto/listar", () => {
    if (produtos.Count > 0)
    {
        return Results.Ok(produtos);
    }
    return Results.NotFound();
});

// POST: /api/produto/cadastrar
app.MapPost("/api/produto/cadastrar", ([FromBody] Produto produto) => {
    produtos.Add(produto);
    return Results.Created("", produto);
});

// POST: /api/produto/remove
app.MapDelete("/api/produto/remove", ([FromBody] Produto produto) => {
    // Busca o produto pelo nome
    var produtoRemove = produtos.FirstOrDefault(p => p.Nome == produto.Nome);

    // Verifica se o produto foi encontrado
    if (produtoRemove != null)
    {
        produtos.Remove(produtoRemove);
        return Results.Accepted("", produtoRemove);
    }

    // Se o produto não for encontrado, retorna NotFound
    return Results.NotFound("Produto não encontrado.");
});

app.MapGet("/api/produto/buscar", ([FromBody] Produto produto) => {
    // Busca o produto pelo nome
    var produtoBuscar = produtos.FirstOrDefault(p => p.Nome == produto.Nome);

    // Verifica se o produto foi encontrado
    if (produtoBuscar != null)
    {
        return Results.Ok(produtoBuscar);
    }

    // Se o produto não for encontrado, retorna NotFound
    return Results.NotFound("Produto não encontrado.");
});

app.MapPut("/api/produto/alterar", ([FromBody] Produto produto) => {
    var produtoAlterar = produtos.FirstOrDefault(p => p.Nome == produto.Nome);

    if (produtoAlterar != null) {
        // Atualiza os campos do produto encontrado
        if (produto.Valor != 0) produtoAlterar.Valor = produto.Valor;
        if (produto.Quantidade != 0) produtoAlterar.Quantidade = produto.Quantidade;
        return Results.Ok(produtoAlterar);
    }

    return Results.NotFound("Produto não encontrado.");
});


app.Run();