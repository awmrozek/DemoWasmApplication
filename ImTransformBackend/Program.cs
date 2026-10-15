var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/image", async (IFormFile file) =>
{
    if (file.Length == 0)
        return Results.BadRequest("Upload an image.");

    // Read the uploaded image bytes.
    await using var input = file.OpenReadStream();
    using var buffer = new MemoryStream();
    await input.CopyToAsync(buffer);
    byte[] imageBytes = buffer.ToArray();

    // TODO: Decode image to RGBA, pass pixels to grayscale.wasm,
    // then encode the processed pixels back to PNG.
    byte[] result = imageBytes;

    return Results.File(result, "application/octet-stream", "processed-image");
})
.DisableAntiforgery();

app.Run();
