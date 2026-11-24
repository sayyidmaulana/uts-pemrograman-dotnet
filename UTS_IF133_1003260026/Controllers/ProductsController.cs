using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTS_IF133_1003260026.Data;
using UTS_IF133_1003260026.Dtos;
using UTS_IF133_1003260026.Models;

namespace UTS_IF133_1003260026.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/products
    /// <summary>Mengambil seluruh produk.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();

        return Ok(products);
    }

    // GET: /api/products/{id}
    /// <summary>Mengambil satu produk berdasarkan id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Produk dengan id {id} tidak ditemukan." });
        }

        return Ok(product);
    }

    // POST: /api/products
    /// <summary>Menambahkan produk baru.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Product>> CreateProduct(ProductRequest request)
    {
        var code = request.Code.Trim();

        if (await _context.Products.AnyAsync(p => p.Code == code))
        {
            return Conflict(new { message = $"Kode produk '{code}' sudah digunakan." });
        }

        var product = new Product
        {
            Code = code,
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = request.Stock,
            IsActive = request.IsActive
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // 201 Created + header Location: /api/products/{id}
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: /api/products/{id}
    /// <summary>Memperbarui data produk.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProduct(int id, ProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Produk dengan id {id} tidak ditemukan." });
        }

        var code = request.Code.Trim();

        if (await _context.Products.AnyAsync(p => p.Code == code && p.Id != id))
        {
            return Conflict(new { message = $"Kode produk '{code}' sudah digunakan produk lain." });
        }

        product.Code = code;
        product.Name = request.Name.Trim();
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /api/products/{id}
    /// <summary>Menghapus produk.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new { message = $"Produk dengan id {id} tidak ditemukan." });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
