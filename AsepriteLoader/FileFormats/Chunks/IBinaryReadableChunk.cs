namespace AsepriteLoader.FileFormats.Chunks;

public interface IBinaryReadableChunk<T>
{
	public static abstract T ReadBinary(BinaryReader reader, ChunkHeader header);
}