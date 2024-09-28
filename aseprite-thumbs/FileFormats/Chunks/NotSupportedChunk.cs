namespace AsepriteThumbs.FileFormats.Chunks;

public class NotSupportedChunk : IBinaryReadableChunk<NotSupportedChunk>
{
	byte[] Data { get; set; }
	
	public static NotSupportedChunk ReadBinary(BinaryReader reader, ChunkHeader header)
	{
		var ret = new NotSupportedChunk();
		// ChunkHeaderの6Bytesを読み飛ばす
		ret.Data = reader.ReadBytes((int)header.ChunkSize - 6);

		return ret;
	}
}