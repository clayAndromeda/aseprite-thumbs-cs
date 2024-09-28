using System.Text;
using SixLabors.ImageSharp.PixelFormats;

namespace AsepriteThumbs.FileFormats;

public class RGB255 : IBinaryReadable<RGB255>
{
	public byte R { get; set; }
	public byte G { get; set; }
	public byte B { get; set; }
	
	public static RGB255 ReadBinary(BinaryReader reader)
	{
		var ret = new RGB255();
		ret.R = reader.ReadByte();
		ret.G = reader.ReadByte();
		ret.B = reader.ReadByte();
		return ret;
	}
	
	public Rgba32 ToRgba32()
	{
		return new Rgba32(R, G, B, 255);
	}
}

public class RGB63 : IBinaryReadable<RGB63>
{
	// 0-63で表現
	public byte R { get; set; }
	public byte G { get; set; }
	public byte B { get; set; }
	
	public static RGB63 ReadBinary(BinaryReader reader)
	{
		var ret = new RGB63();
		ret.R = reader.ReadByte();
		ret.G = reader.ReadByte();
		ret.B = reader.ReadByte();
		return ret;
	}
	
	public Rgba32 ToRgba32()
	{
		// 0-63を0-255に変換
		return new Rgba32(R * 4, G * 4, B * 4, 255);
	}
}

public class RGBA : IBinaryReadable<RGBA>
{
	public byte R { get; set; }
	public byte G { get; set; }
	public byte B { get; set; }
	public byte A { get; set; }
	
	public static RGBA ReadBinary(BinaryReader reader)
	{
		var ret = new RGBA();
		ret.R = reader.ReadByte();
		ret.G = reader.ReadByte();
		ret.B = reader.ReadByte();
		ret.A = reader.ReadByte();
		return ret;
	}

	public Rgba32 ToRgba32()
	{
		return new Rgba32(R, G, B, A);
	}
}

public class STRING : IBinaryReadable<STRING>
{
	public string Value { get; set; }
	
	public static STRING ReadBinary(BinaryReader reader)
	{
		// 先頭4Byteは文字列の長さ
		// その後にUTF-8文字列が続く。'\0'は含まれない
		var ret = new STRING();
		var length = reader.ReadUInt16();
		var bytes = reader.ReadBytes((int)length);
		ret.Value = Encoding.UTF8.GetString(bytes);

		return ret;
	}
}

public class POINT : IBinaryReadable<POINT>
{
	public int X { get; set; }
	public int Y { get; set; }
	
	public static POINT ReadBinary(BinaryReader reader)
	{
		var ret = new POINT();
		ret.X = reader.ReadInt32();
		ret.Y = reader.ReadInt32();
		return ret;
	}
}

public class SIZE : IBinaryReadable<SIZE>
{
	public int Width { get; set; }
	public int Height { get; set; }
	
	public static SIZE ReadBinary(BinaryReader reader)
	{
		var ret = new SIZE();
		ret.Width = reader.ReadInt32();
		ret.Height = reader.ReadInt32();
		return ret;
	}
}

public class RECT : IBinaryReadable<RECT>
{
	public POINT OriginCoordinate { get; set; }
	public SIZE RectableSize { get; set; }
	public static RECT ReadBinary(BinaryReader reader)
	{
		var ret = new RECT();
		ret.OriginCoordinate = POINT.ReadBinary(reader);
		ret.RectableSize = SIZE.ReadBinary(reader);
		return ret;
	}
}

public class PIXEL : IBinaryReadable<PIXEL>
{
	public RGBA Color { get; set; }
	public byte[] Grayscale { get; set; } = new byte[2];
	public byte Indexed { get; set; }
	
	public static PIXEL ReadBinary(BinaryReader reader)
	{
		var ret = new PIXEL();
		ret.Color = RGBA.ReadBinary(reader);
		ret.Grayscale = reader.ReadBytes(2);
		ret.Indexed = reader.ReadByte();
		return ret;
	}
}

public class UUID : IBinaryReadable<UUID>
{
	public byte[] Data { get; set; } = new byte[16];
	
	public static UUID ReadBinary(BinaryReader reader)
	{
		var ret = new UUID();
		ret.Data = reader.ReadBytes(16);
		return ret;
	}
}