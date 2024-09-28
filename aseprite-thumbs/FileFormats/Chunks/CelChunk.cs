using System.IO.Compression;

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
	
	public ImageData Image { get; set; }

	public class ImageData
	{
		public ushort Width { get; set; }
		public ushort Height { get; set; }
		public byte[] ImageBytes { get; set; }
	}

	public class LinkedCel { /* (unsupported) */ }
	public class TileMap { /* (unsupported) */ }
	
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
			ret.Image = new ImageData();
			ret.Image.Width = reader.ReadUInt16();
			ret.Image.Height = reader.ReadUInt16();
			// ChunkHeaderで6Bytes, CelChunk本体で16 + 4, 合計26Bytesを読み込んだ残りが画像データ
			ret.Image.ImageBytes = reader.ReadBytes((int)(header.ChunkSize - 26));
		}
		else if (ret.CelType == 1)
		{
			throw new NotSupportedException($"Unsupported CelType: {ret.CelType} (Linked Cel)");
		}
		else if (ret.CelType == 2)
		{
			ret.Image = new ImageData();
			ret.Image.Width = reader.ReadUInt16();
			ret.Image.Height = reader.ReadUInt16();
			// ChunkHeaderで6Bytes, CelChunk本体で16 + 4, 合計26Bytesを読み込んだ残りが画像データ
			var compressedBytes = reader.ReadBytes((int)(header.ChunkSize - 26));
			
			// ZLibStreamで解凍する
			using var ms = new MemoryStream(compressedBytes);
			using var ds = new ZLibStream(ms, CompressionMode.Decompress);
			using var resultStream = new MemoryStream();
			ds.CopyTo(resultStream);
			ret.Image.ImageBytes = resultStream.ToArray();
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