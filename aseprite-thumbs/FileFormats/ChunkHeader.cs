namespace AsepriteThumbs.FileFormats;

public static class ChunkTypes
{
	public const uint OldPalette04Chunk = 0x0004;
	public const uint OldPalette11Chunk = 0x0011;
	public const uint LayerChunk = 0x2004;
	public const uint CelChunk = 0x2005;
	public const uint CelExtraChunk = 0x2006;
	public const uint ColorProfileChunk = 0x2007;
	public const uint ExternalFileChunk = 0x2008;
	public const uint DeprecatedChunk = 0x2016;
	public const uint PathChunk = 0x2017;
	public const uint TagsChunk = 0x2018;
	public const uint PaletteChunk = 0x2019;
	public const uint UserdataChunk = 0x2020;
	public const uint SliceChunk = 0x2022;
	public const uint TileSetChunk = 0x2023;
}

public class ChunkHeader : IBinaryReadable<ChunkHeader>
{
	public uint ChunkSize { get; set; }
	public ushort ChunkType { get; set; }
	
	public static ChunkHeader ReadBinary(BinaryReader reader)
	{
		var ret = new ChunkHeader();
		ret.ChunkSize = reader.ReadUInt32();
		ret.ChunkType = reader.ReadUInt16();
		return ret;
	}
}

