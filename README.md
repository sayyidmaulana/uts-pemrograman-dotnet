# UTS IF133 Pemrograman .NET — Inventory Product API

**Nama:** Sayyid Maulana Khakul Yakin · **NIM:** 1003260026

REST API sederhana untuk mengelola data produk inventori toko.
Stack: ASP.NET Core Web API (.NET 10), Entity Framework Core 10, SQLite, Swagger (Swashbuckle).

## Struktur project

```
UTS_IF133_1003260026/
├── Controllers/ProductsController.cs   # endpoint CRUD /api/products (async)
├── Data/AppDbContext.cs                # DbContext + konfigurasi entity (Fluent API)
├── Dtos/ProductRequest.cs              # body request POST/PUT + validasi
├── Models/Product.cs                   # entity Product
├── Migrations/                         # migration awal (InitialCreate)
├── appsettings.json                    # connection string "DefaultConnection"
├── Program.cs                          # registrasi DbContext, Swagger, auto-migrate
└── UTS_IF133_1003260026.http           # skenario uji semua endpoint
```

## Entity Product

| Properti | Tipe    | Keterangan                       |
|----------|---------|----------------------------------|
| Id       | int     | Primary key, auto increment      |
| Code     | string  | Wajib, maks. 50 karakter, unik   |
| Name     | string  | Wajib, maks. 150 karakter        |
| Price    | decimal | Presisi (18,2), tidak boleh < 0  |
| Stock    | int     | Tidak boleh < 0                  |
| IsActive | bool    | Default `true`                   |

## Cara menjalankan

Jalankan dari folder `UTS_IF133_1003260026/` (yang berisi file `.csproj`):

```bash
# 1. (sekali saja) install tool EF Core CLI
dotnet tool install --global dotnet-ef
#    kalau sudah pernah install: dotnet tool update --global dotnet-ef

# 2. restore & build
dotnet build

# 3. terapkan migration awal ke database SQLite (membuat file inventory.db)
dotnet ef migrations list
dotnet ef database update

# 4. jalankan API
dotnet run --launch-profile http
```

Buka Swagger UI di **http://localhost:5135/swagger**.

> Program.cs juga memanggil `Database.MigrateAsync()` saat start, jadi `dotnet run`
> tetap membuat/memperbarui database walaupun langkah 3 terlewat.

## Endpoint

| Method | URL                   | Sukses           | Gagal                                    |
|--------|-----------------------|------------------|------------------------------------------|
| GET    | `/api/products`       | 200 OK           | —                                        |
| GET    | `/api/products/{id}`  | 200 OK           | 404 Not Found                            |
| POST   | `/api/products`       | 201 Created      | 400 (validasi), 409 (kode sudah dipakai) |
| PUT    | `/api/products/{id}`  | 204 No Content   | 404, 400, 409                            |
| DELETE | `/api/products/{id}`  | 204 No Content   | 404 Not Found                            |

POST mengembalikan `201 Created` lewat `CreatedAtAction(nameof(GetProduct), ...)`,
sehingga response berisi header `Location: http://localhost:5135/api/products/{id}`.

Contoh body POST/PUT:

```json
{
  "code": "PRD-001",
  "name": "Keyboard Mechanical",
  "price": 750000,
  "stock": 12,
  "isActive": true
}
```

## Skenario pengujian (bukti)

Jalankan berurutan di Swagger (atau file `UTS_IF133_1003260026.http` / Postman) dan
screenshot request + response tiap langkah:

1. `GET /api/products` → 200, `[]` (masih kosong)
2. `POST /api/products` dengan data PRD-001 → 201, perhatikan header `location`
3. `POST /api/products` dengan data PRD-002 → 201
4. `GET /api/products/1` → 200
5. `GET /api/products/999` → 404
6. `PUT /api/products/1` (ubah name/price/stock) → 204, lalu `GET /api/products/1` untuk melihat perubahannya
7. `PUT /api/products/999` → 404
8. `DELETE /api/products/2` → 204
9. `DELETE /api/products/2` lagi → 404
10. (tambahan) POST dengan price negatif → 400, POST dengan code `PRD-001` lagi → 409

Simpan juga screenshot terminal saat `dotnet ef database update` berhasil.

## Troubleshooting

Kalau `dotnet ef database update` menampilkan pesan *pending model changes*,
buat ulang migration-nya:

```bash
rm -rf Migrations inventory.db
dotnet ef migrations add InitialCreate
dotnet ef database update
```
