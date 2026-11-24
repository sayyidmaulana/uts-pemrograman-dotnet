namespace UTS_IF133_1003260026.Models;

/// <summary>
/// Entity Product: representasi tabel "Products" pada database inventori toko.
/// </summary>
public class Product
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }
}
