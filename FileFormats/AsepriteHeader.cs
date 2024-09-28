namespace AsepriteThumbs.FileFormats;

public class AsepriteHeader
{
	public UInt32 FileSize { get; set; }
	public UInt16 MagicNumber { get; set; }
	public UInt16 Frames { get; set; }
	public UInt16 Width { get; set; }
	public UInt16 Height { get; set; }
	public UInt16 Depth { get; set; }
	public UInt32 Flags { get; set; }
	public UInt16 Speed { get; set; }
	public UInt32 Reserved1 { get; set; }
	public UInt32 Reserved2 { get; set; }
	public Byte PaletteEntry { get; set; }
	public Byte[] IgnoreBytes { get; set; } = new Byte[3];
	public UInt16 NumOfColors { get; set; }
	public Byte PixelWidth { get; set; }
	public Byte PixelHeight { get; set; }
	public Int16 XPositionOfGrid { get; set; }
	public Int16 YPositionOfGrid { get; set; }
	public UInt16 GridWidth { get; set; }
	public UInt16 GridHeight { get; set; }
	// 84Bytesの予約領域がある

	public static AsepriteHeader ReadBinary(BinaryReader reader)
	{
		var ret = new AsepriteHeader();
		ret.FileSize = reader.ReadUInt32();
		ret.MagicNumber = reader.ReadUInt16();
		ret.Frames = reader.ReadUInt16();
		ret.Width = reader.ReadUInt16();
		ret.Height = reader.ReadUInt16();
		ret.Depth = reader.ReadUInt16();
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
