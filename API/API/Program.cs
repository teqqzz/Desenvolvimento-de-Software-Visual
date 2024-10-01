using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Banco>();
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
app.MapGet("/api/produto/listar", async ([FromServices] Banco ctx) =>
{
    var produtos = await ctx.Produtos.ToListAsync();

    if (produtos.Any())
    {
        return Results.Ok(produtos);
    }
    
    return Results.NotFound("Nenhum produto encontrado.");
});

// POST: /api/produto/cadastrar
app.MapPost("/api/produto/cadastrar", async ([FromBody] Produto produto, [FromServices] Banco ctx) => {
    ctx.Produtos.Add(produto);
    await ctx.SaveChangesAsync();
    return Results.Created($"/api/produto/{produto.Id}", produto);
});

// POST: /api/produto/remove
app.MapDelete("/api/produto/remove", async ([FromBody] Produto produto, [FromServices] Banco ctx) =>
{
    // Busca o produto pelo ID no banco de dados
    var produtoRemove = await ctx.Produtos.FindAsync(produto.Id);

    // Verifica se o produto foi encontrado
    if (produtoRemove != null){
        // Remove o produto do banco de dados
        ctx.Produtos.Remove(produtoRemove);
        await ctx.SaveChangesAsync(); // Salva as mudanças no banco

        return Results.Accepted("", produtoRemove); // Retorna o produto removido
    }

    // Se o produto não for encontrado, retorna NotFound
    return Results.NotFound("Produto não encontrado.");
});

app.MapPost("/api/produto/buscar", async ([FromBody] Produto produto, [FromServices] Banco ctx) =>
{
    // Busca o produto pelo ID no banco de dados
    Produto? produtoBuscar = await ctx.Produtos.FindAsync(produto.Id);
    // Verifica se o produto foi encontrado
    if (produtoBuscar != null){
        return Results.Ok(produtoBuscar); // Retorna o produto encontrado
    }
    // Se o produto não for encontrado, retorna NotFound
    return Results.NotFound("Produto não encontrado.");
});

app.MapPut("/api/produto/alterar", async ([FromBody] Produto produto, [FromServices] Banco ctx) =>
{
    // Busca o produto pelo ID no banco de dados
    var produtoAlterar = await ctx.Produtos.FindAsync(produto.Id);
    if (produtoAlterar != null){
        // Atualiza os campos do produto encontrado, se valores novos foram fornecidos
        if (produto.Valor != 0) produtoAlterar.Valor = produto.Valor;
        if (produto.Quantidade != 0) produtoAlterar.Quantidade = produto.Quantidade;
        // Salva as alterações no banco de dados
        await ctx.SaveChangesAsync();
        return Results.Ok(produtoAlterar); // Retorna o produto atualizado
    }
    return Results.NotFound("Produto não encontrado."); // Caso o produto não seja encontrado
});


app.Run();