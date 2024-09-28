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
		var header = AsepriteHeader.ReadBinary(reader);
		
		
	}
}