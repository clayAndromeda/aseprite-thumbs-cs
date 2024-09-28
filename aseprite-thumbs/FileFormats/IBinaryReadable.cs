namespace AsepriteThumbs.FileFormats;

public interface IBinaryReadable<T>
{
	public static abstract T ReadBinary(BinaryReader reader);
}