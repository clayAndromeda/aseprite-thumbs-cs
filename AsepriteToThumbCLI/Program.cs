using AsepriteLoader.FileFormats;
using ConsoleAppFramework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

ConsoleApp.Run(args, Commands.ReadAsepriteFile);

static class Commands
{
	public static void ReadAsepriteFile(
		[Argument] string inputPath
	)
	{
		// inputPath = "sample.aseprite";
		// inputPath = "sample-glayscale.aseprite";
		inputPath = "sample-indexed.aseprite";
		
		var fileBytes = File.ReadAllBytes(inputPath);
		
		// inputPathにあるファイルをBinaryReaderで読み込む
		using var reader = new BinaryReader(new MemoryStream(fileBytes));
		AsepriteHeader fileHeader = AsepriteHeader.ReadBinary(reader);
		
		// とりあえず先頭フレームだけ読み取る
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
		image.Save("output.png");
	}
}