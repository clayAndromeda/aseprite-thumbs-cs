namespace AsepriteThumbs.FileFormats;

public class AsepriteHeader : IBinaryReadable<AsepriteHeader>
{
	public uint FileSize { get; set; }
	public ushort MagicNumber { get; set; }
	public ushort Frames { get; set; }
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	public ushort ColorDepth { get; set; }
	public uint Flags { get; set; }
	public ushort Speed { get; set; }
	public uint Reserved1 { get; set; }
	public uint Reserved2 { get; set; }
	public byte PaletteEntry { get; set; }
	public byte[] IgnoreBytes { get; set; } = new byte[3];
	public ushort NumOfColors { get; set; }
	public byte PixelWidth { get; set; }
	public byte PixelHeight { get; set; }
	public short XPositionOfGrid { get; set; }
	public short YPositionOfGrid { get; set; }
	public ushort GridWidth { get; set; }
	public ushort GridHeight { get; set; }
	
	// この後に84Bytesの予約領域がある

	public static AsepriteHeader ReadBinary(BinaryReader reader)
	{
		var ret = new AsepriteHeader();
		ret.FileSize = reader.ReadUInt32();
		ret.MagicNumber = reader.ReadUInt16();
		ret.Frames = reader.ReadUInt16();
		ret.Width = reader.ReadUInt16();
		ret.Height = reader.ReadUInt16();
		ret.ColorDepth = reader.ReadUInt16();
		ret.Flags = reader.ReadUInt32();
		ret.Speed = reader.ReadUInt16();
		ret.Reserved1 = reader.ReadUInt32();
		ret.Reserved2 = reader.ReadUInt32();
		ret.PaletteEntry = reader.ReadByte();
		ret.IgnoreBytes = reader.ReadBytes(3);
		ret.NumOfColors = reader.ReadUInt16();
		ret.PixelWidth = reader.ReadByte();
		ret.PixelHeight = reader.ReadByte();
		ret.XPositionOfGrid = reader.ReadInt16();
		ret.YPositionOfGrid = reader.ReadInt16();
		ret.GridWidth = reader.ReadUInt16();
		ret.GridHeight = reader.ReadUInt16();
		
		// 84Bytesだけ、予約領域を捨てReadする
		reader.ReadBytes(84);
		
		return ret;
	}

	public override string ToString()
	{
		// プロパティを改行区切りで出力
		return string.Join("\n", GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(this)} ({p.GetValue(this):X})"));
	}
}
