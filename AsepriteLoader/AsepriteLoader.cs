using AsepriteLoader.FileFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AsepriteLoader;

public class AsepriteLoader
{
    public async Task<byte[]> ConvertToPngAsync(string src)
    {
        try
        {
            var fileBytes = await IcnsToPngAsync(src);
            return fileBytes;
        }
        catch (Exception ex)
        {
            throw new Exception("Error generating thumbnail", ex);
        }
    }

    private async Task<byte[]> IcnsToPngAsync(string src)
    {
        var fileBytes = await File.ReadAllBytesAsync(src);
        
		// inputPathにあるファイルをBinaryReaderで読み込む
		using var reader = new BinaryReader(new MemoryStream(fileBytes));
		AsepriteHeader fileHeader = AsepriteHeader.ReadBinary(reader);
		
		// フレームだけ読み取る
		Frame frame = Frame.ReadBinary(reader);
		using var image = new Image<Rgba32>(fileHeader.Width, fileHeader.Height);
        
		var celChunk = frame.CelChunks.First();
		var pixels = celChunk.GetPixels(fileHeader.ColorDepth, frame.GetPaletteColors());
		for (int y = 0; y < fileHeader.Height; ++y)
		{
			for (int x = 0; x < fileHeader.Width; ++x)
			{
				image[x, y] = pixels[y * fileHeader.Width + x];
			}
		}
		
		// サイズ調整して出力
		image.Mutate(x => x.Resize(256, 256, KnownResamplers.NearestNeighbor));
		
		using var outputMemoryStream = new MemoryStream();
		await image.SaveAsPngAsync(outputMemoryStream);
		return outputMemoryStream.ToArray();
    }
}