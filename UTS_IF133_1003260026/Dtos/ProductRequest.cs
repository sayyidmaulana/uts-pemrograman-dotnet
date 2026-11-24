using System.ComponentModel.DataAnnotations;

namespace UTS_IF133_1003260026.Dtos;

/// <summary>
/// Body request untuk POST dan PUT. Id tidak dikirim oleh client:
/// Id dibuat otomatis oleh database (POST) atau diambil dari URL (PUT).
/// </summary>
public class ProductRequest
{
    [Required(ErrorMessage = "Code wajib diisi.")]
    [StringLength(50, ErrorMessage = "Code maksimal 50 karakter.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(150, ErrorMessage = "Name maksimal 150 karakter.")]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "9999999999999999", ErrorMessage = "Price tidak boleh negatif.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock tidak boleh negatif.")]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;
}
