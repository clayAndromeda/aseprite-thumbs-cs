using SixLabors.ImageSharp.PixelFormats;

namespace AsepriteLoader.FileFormats.Chunks;

public class OldPalette11Chunk : IBinaryReadableChunk<OldPalette11Chunk>
{
	/*
	 * Old palette chunk (0x0011)
	 *
	 * Ignore this chunk if you find the new palette chunk (0x2019)
	 */


	public ushort NumOfPackets { get; set; }
	public Packet[] Packets { get; set; }


	public static OldPalette11Chunk ReadBinary(BinaryReader reader, ChunkHeader header)
	{
		var ret = new OldPalette11Chunk();
		ret.NumOfPackets = reader.ReadUInt16();
		var packets = new Packet[ret.NumOfPackets];
		for (var i = 0; i < packets.Length; ++i)
		{
			var packet = new Packet();
			packet.NumOfPaletteEntries = reader.ReadByte();
			packet.NumOfColors = reader.ReadByte();
			if (packet.NumOfColors == 0)
				packet.Colors = new RGB63[256];
			else
				packet.Colors = new RGB63[packet.NumOfColors];
			for (var j = 0; j < packet.Colors.Length; ++j) packet.Colors[j] = RGB63.ReadBinary(reader);

			packets[i] = packet;
		}

		ret.Packets = packets;

		return ret;
	}

	public Rgba32[] GetPaletteColors()
	{
		return Packets
			.SelectMany(x => x.Colors)
			.Select(x => x.ToRgba32())
			.ToArray();
	}

	public class Packet
	{
		public byte NumOfPaletteEntries { get; set; }
		public byte NumOfColors { get; set; }
		public RGB63[] Colors { get; set; }
	}
}