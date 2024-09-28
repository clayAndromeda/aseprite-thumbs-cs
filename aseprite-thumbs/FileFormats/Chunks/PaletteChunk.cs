namespace AsepriteThumbs.FileFormats.Chunks;

public class PaletteChunk : IBinaryReadableChunk<PaletteChunk>
{
	/*
	 * Palette Chunk (0x2019)
	 */
	
	public uint PaletteSize { get; set; } // 4Bytes
	public uint FirstColorIndexToChange { get; set; } // 4Bytes
	public int LastColorIndexToChange { get; set; } // 4Bytes
	public byte[] ForFuture { get; set; } = new byte[8]; // 8Bytes

	public PaletteEntry[] Entries { get; set; }

	public class PaletteEntry : IBinaryReadable<PaletteEntry>
	{
		public ushort EntryFlag { get; set; }
		public RGBA Color { get; set; }
		public STRING ColorName { get; set; }
		
		public static PaletteEntry ReadBinary(BinaryReader reader)
		{
			var ret = new PaletteEntry();
			ret.EntryFlag = reader.ReadUInt16();
			ret.Color = RGBA.ReadBinary(reader);
			if (ret.EntryFlag == 1)
			{
				// Has Name
				ret.ColorName = STRING.ReadBinary(reader);
			}
			return ret;
		}
	}
	
	public static PaletteChunk ReadBinary(BinaryReader reader, ChunkHeader header)
	{
		var ret = new PaletteChunk();
		ret.PaletteSize = reader.ReadUInt32();
		ret.FirstColorIndexToChange = reader.ReadUInt32();
		ret.LastColorIndexToChange = reader.ReadInt32();
		ret.ForFuture = reader.ReadBytes(8);
		
		ret.Entries = new PaletteEntry[ret.PaletteSize];
		for (int i = 0; i < ret.PaletteSize; i++)
		{
			ret.Entries[i] = PaletteEntry.ReadBinary(reader);
		}
		return ret;
	}
}