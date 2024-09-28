namespace AsepriteThumbs.FileFormats.Chunks;

public class OldPalette04Chunk : IBinaryReadable<OldPalette04Chunk>
{
	/*
	 * Old palette chunk (0x0004)
	 * 
	 * Ignore this chunk if you find the new palette chunk (0x2019).
	 * Aseprite v1.1 saves both chunks (0x0004 and 0x2019) just for backward compatibility.
	 * Aseprite v1.3.5 writes this chunk if the palette doesn't have alpha channel and contains 256 colors or less (because this chunk is smaller),
	 * in other case the new palette chunk (0x2019) will be used (and the old one is not saved anymore).
	 */
	
	public UInt32 NumOfPackets { get; set; }
	public Packet[] Packets { get; set; }

	public class Packet
	{
		public Byte numOfPaletteEntries { get; set; }
		public Byte numOfColors { get; set; }
		public RGB255[] Colors { get; set; }
	}

	public static OldPalette04Chunk ReadBinary(BinaryReader reader)
	{
		var ret = new OldPalette04Chunk();
		ret.NumOfPackets = reader.ReadUInt32();
		
		Packet[] packets = new Packet[ret.NumOfPackets];
		for (int i = 0; i < packets.Length; ++i)
		{
			var packet = new Packet();
			packet.numOfPaletteEntries = reader.ReadByte();
			packet.numOfColors = reader.ReadByte();
			packet.Colors = new RGB255[packet.numOfColors];
			for (int j = 0; j < packet.Colors.Length; ++j)
			{
				packet.Colors[j] = RGB255.ReadBinary(reader);
			}
			packets[i] = packet;
		}
		ret.Packets = packets;

		return ret;
	}
}