using AsepriteThumbs.FileFormats;
using ConsoleAppFramework;

ConsoleApp.Run(args, Commands.ReadAsepriteFile);

static class Commands
{
	public static void ReadAsepriteFile(
		[Argument] string inputPath
	)
	{
		var fileBytes = File.ReadAllBytes(inputPath);
		
		// inputPathにあるファイルをBinaryReaderで読み込む
		using var reader = new BinaryReader(new MemoryStream(fileBytes));
		AsepriteHeader fileHeader = AsepriteHeader.ReadBinary(reader);
		
		// とりあえず先頭フレームだけ読み取る
		Frame frame = Frame.ReadBinary(reader);
		
		Console.WriteLine($"Frame Header: {frame.Header.MagicNumber:X8}");
	}
}