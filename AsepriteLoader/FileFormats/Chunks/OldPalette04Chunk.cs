using SixLabors.ImageSharp.PixelFormats;

namespace AsepriteLoader.FileFormats.Chunks;

public class OldPalette04Chunk : IBinaryReadableChunk<OldPalette04Chunk>
{
	/*
	 * Old palette chunk (0x0004)
	 *
	 * Ignore this chunk if you find the new palette chunk (0x2019).
	 * Aseprite v1.1 saves both chunks (0x0004 and 0x2019) just for backward compatibility.
	 * Aseprite v1.3.5 writes this chunk if the palette doesn't have alpha channel and contains 256 colors or less (because this chunk is smaller),
	 * in other case the new palette chunk (0x2019) will be used (and the old one is not saved anymore).
	 */

	public ushort NumOfPackets { get; set; }
	public Packet[] Packets { get; set; }

	public static OldPalette04Chunk ReadBinary(BinaryReader reader, ChunkHeader header)
	{
		var ret = new OldPalette04Chunk();
		ret.NumOfPackets = reader.ReadUInt16();

		var packets = new Packet[ret.NumOfPackets];
		for (var i = 0; i < packets.Length; ++i)
		{
			var packet = new Packet();
			packet.numOfPaletteEntries = reader.ReadByte();
			packet.numOfColors = reader.ReadByte();
			if (packet.numOfColors == 0)
				packet.Colors = new RGB255[256];
			else
				packet.Colors = new RGB255[packet.numOfColors];

			for (var j = 0; j < packet.Colors.Length; ++j) packet.Colors[j] = RGB255.ReadBinary(reader);

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
		public byte numOfPaletteEntries { get; set; }
		public byte numOfColors { get; set; }
		public RGB255[] Colors { get; set; }
	}
}