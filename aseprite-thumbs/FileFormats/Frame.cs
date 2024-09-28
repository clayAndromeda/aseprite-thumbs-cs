using AsepriteThumbs.FileFormats.Chunks;
using SixLabors.ImageSharp.PixelFormats;

namespace AsepriteThumbs.FileFormats;

public class Frame : IBinaryReadable<Frame>
{
	public FrameHeader Header { get; set; }
	
	public List<CelChunk> CelChunks { get; set; } = new();
	public List<LayerChunk> LayerChunks { get; set; } = new();
	public List<PaletteChunk> PaletteChunks { get; set; } = new();
	public List<OldPalette04Chunk> OldPalette04Chunks { get; set; } = new();
	public List<OldPalette11Chunk> OldPalette11Chunks { get; set; } = new();

	public List<NotSupportedChunk> NotSupportedChunks { get; set; } = new();
	
	public Rgba32[] GetPaletteColors()
	{
		if (PaletteChunks.Count != 0)
		{
			return PaletteChunks[0].GetPaletteColors();
		}
		else if (OldPalette04Chunks.Count != 0)
		{
			return OldPalette04Chunks[0].GetPaletteColors();
		}
		else if (OldPalette11Chunks.Count != 0)
		{
			return OldPalette11Chunks[0].GetPaletteColors();
		}
		else
		{
			return [new Rgba32(255, 255, 255, 255)];
		}
	}
	
	public static Frame ReadBinary(BinaryReader reader)
	{
		var ret = new Frame();
		ret.Header = FrameHeader.ReadBinary(reader);
		for (int i = 0; i < ret.Header.TheNumberOfChunks; ++i)
		{
			ChunkHeader chunkHeader = ChunkHeader.ReadBinary(reader);
			
			var chunkType = chunkHeader.ChunkType;
			if (chunkType == ChunkTypes.OldPalette04Chunk)
			{
				ret.OldPalette04Chunks.Add(OldPalette04Chunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.OldPalette11Chunk)
			{
				ret.OldPalette11Chunks.Add(OldPalette11Chunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.LayerChunk)
			{
				ret.LayerChunks.Add(LayerChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.CelChunk)
			{
				ret.CelChunks.Add(CelChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.CelExtraChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.ColorProfileChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.ExternalFileChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.DeprecatedChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.PathChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.TagsChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.PaletteChunk)
			{
				ret.PaletteChunks.Add(PaletteChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.UserdataChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.SliceChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else if (chunkType == ChunkTypes.TileSetChunk)
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
			else
			{
				ret.NotSupportedChunks.Add(NotSupportedChunk.ReadBinary(reader, chunkHeader));
			}
		}

		return ret;
	}
}