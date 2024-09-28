using System.IO.Compression;
using SixLabors.ImageSharp.PixelFormats;

namespace AsepriteThumbs.FileFormats.Chunks;

public class CelChunk : IBinaryReadableChunk<CelChunk>
{
	/*
	 * Cel Chunk (0x2005)
	 * 
	 * This chunk determine where to put a cel in the specified layer/frame.
	 *
	 * CelType (WORD)
	 *	0 - Raw Image Data (unsupported)
	 *  1 - Linked Cel (unsupported)
	 *  2 - Compressed Image
	 *  3 - Compression TileMap (unsupported)
	 */
	
	public ushort LayerIndex { get; set; } // 2Bytes
	public short XPosition { get; set; } // 2Bytes
	public short YPosition { get; set; } // 2Bytes
	public byte OpacityLevel { get; set; } // 1Byte
	public ushort CelType { get; set; } // 2Bytes
	public short ZIndex { get; set; } // 2Bytes
	public byte[] ForFuture { get; set; } = new byte[5]; // 5Bytes
	
	// 共通で合計16Bytes使う
	
	public ImageData Data { get; set; }

	public class ImageData
	{
		public ushort Width { get; set; }
		public ushort Height { get; set; }
		public byte[] ImageBytes { get; set; }
	}

	public class LinkedCel { /* (unsupported) */ }
	public class TileMap { /* (unsupported) */ }

	public Rgba32[] GetPixels(ushort colorDepth, Rgba32[] palette)
	{
		Rgba32[] ret = new Rgba32[Data.Width * Data.Height];
		if (colorDepth == 32)
		{
			for (int i = 0; i < ret.Length; ++i)
			{
				ret[i] = new Rgba32(
					Data.ImageBytes[i * 4 + 0],
					Data.ImageBytes[i * 4 + 1],
					Data.ImageBytes[i * 4 + 2],
					Data.ImageBytes[i * 4 + 3]);
			}
		}
		else if (colorDepth == 16)
		{
			for (int i = 0; i < ret.Length; ++i)
			{
				ret[i] = new Rgba32(
					Data.ImageBytes[i * 2 + 0],
					Data.ImageBytes[i * 2 + 0],
					Data.ImageBytes[i * 2 + 0],
					Data.ImageBytes[i * 2 + 1]);
			}
		}
		else if (colorDepth == 8)
		{
			for (int i = 0; i < ret.Length; ++i)
			{
				ret[i] = palette[Data.ImageBytes[i]];
			}
		}
		else
		{
			throw new NotSupportedException($"Unsupported ColorDepth: {colorDepth}");
		}

		return ret;
	}
	
	public static CelChunk ReadBinary(BinaryReader reader, ChunkHeader header)
	{
		var ret = new CelChunk();
		ret.LayerIndex = reader.ReadUInt16();
		ret.XPosition = reader.ReadInt16();
		ret.YPosition = reader.ReadInt16();
		ret.OpacityLevel = reader.ReadByte();
		ret.CelType = reader.ReadUInt16();
		ret.ZIndex = reader.ReadInt16();
		ret.ForFuture = reader.ReadBytes(5);

		if (ret.CelType == 0)
		{
			ret.Data = new ImageData();
			ret.Data.Width = reader.ReadUInt16();
			ret.Data.Height = reader.ReadUInt16();
			// ChunkHeaderで6Bytes, CelChunk本体で16 + 4, 合計26Bytesを読み込んだ残りが画像データ
			ret.Data.ImageBytes = reader.ReadBytes((int)(header.ChunkSize - 26));
		}
		else if (ret.CelType == 1)
		{
			throw new NotSupportedException($"Unsupported CelType: {ret.CelType} (Linked Cel)");
		}
		else if (ret.CelType == 2)
		{
			ret.Data = new ImageData();
			ret.Data.Width = reader.ReadUInt16();
			ret.Data.Height = reader.ReadUInt16();
			// ChunkHeaderで6Bytes, CelChunk本体で16 + 4, 合計26Bytesを読み込んだ残りが画像データ
			var compressedBytes = reader.ReadBytes((int)(header.ChunkSize - 26));
			
			// ZLibStreamで解凍する
			using var ms = new MemoryStream(compressedBytes);
			using var ds = new ZLibStream(ms, CompressionMode.Decompress);
			using var resultStream = new MemoryStream();
			ds.CopyTo(resultStream);
			ret.Data.ImageBytes = resultStream.ToArray();
		}
		else if (ret.CelType == 3)
		{
			throw new NotSupportedException($"Unsupported CelType: {ret.CelType} (Compression TileMap)");
		}
		else
		{
			throw new NotSupportedException($"Unsupported CelType: {ret.CelType}");
		}

		return ret;
	}
}