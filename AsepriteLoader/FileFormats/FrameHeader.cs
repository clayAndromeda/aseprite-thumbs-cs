namespace AsepriteLoader.FileFormats;

public class FrameHeader : IBinaryReadable<FrameHeader>
{
	public uint BytesInThisFrame { get; set; }
	public ushort MagicNumber { get; set; }
	public ushort OldField { get; set; }
	public ushort FrameDuration { get; set; }
	public byte[] ForFuture { get; set; } = new byte[2];
	public uint TheNumberOfChunks { get; set; }

	public static FrameHeader ReadBinary(BinaryReader reader)
	{
		var ret = new FrameHeader();
		ret.BytesInThisFrame = reader.ReadUInt32();
		ret.MagicNumber = reader.ReadUInt16();
		ret.OldField = reader.ReadUInt16();
		ret.FrameDuration = reader.ReadUInt16();
		ret.ForFuture = reader.ReadBytes(2);
		ret.TheNumberOfChunks = reader.ReadUInt32();
		return ret;
	}
}