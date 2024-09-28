namespace AsepriteThumbs.FileFormats;

public static class ChunkTypes
{
	public const UInt32 OldPalette04Chunk = 0x0004;
	public const UInt32 OldPalette11Chunk = 0x0011;
	public const UInt32 LayerChunk = 0x2004;
	public const UInt32 CelChunk = 0x2005;
	public const UInt32 CelExtraChunk = 0x2006;
	public const UInt32 ColorProfileChunk = 0x2007;
	public const UInt32 ExternalFileChunk = 0x2008;
	public const UInt32 DeprecatedChunk = 0x2016;
	public const UInt32 PathChunk = 0x2017;
	public const UInt32 TagsChunk = 0x2018;
	public const UInt32 PaletteChunk = 0x2019;
	public const UInt32 UserdataChunk = 0x2020;
	public const UInt32 SliceChunk = 0x2022;
	public const UInt32 TileSetChunk = 0x2023;
}

public class ChunkHeader : IBinaryReadable<ChunkHeader>
{
	public UInt32 ChunkSize { get; set; }
	public UInt16 ChunkType { get; set; }
	
	public static ChunkHeader ReadBinary(BinaryReader reader)
	{
		var ret = new ChunkHeader();
		ret.ChunkSize = reader.ReadUInt32();
		ret.ChunkType = reader.ReadUInt16();
		return ret;
	}
}

