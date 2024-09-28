namespace AsepriteThumbs.FileFormats.Chunks;

public class OldPalette11Chunk : IBinaryReadable<OldPalette11Chunk>
{
	/*
	 * Old palette chunk (0x0011)
	 * 
	 * Ignore this chunk if you find the new palette chunk (0x2019)
	 */
	
	
	public UInt16 NumOfPackets { get; set; }
	public Packet[] Packets { get; set; }
	
	public class Packet
	{
		public Byte NumOfPaletteEntries { get; set; }
		public Byte NumOfColors { get; set; }
		public RGB63[] Colors { get; set; }
	}
	
	
	public static OldPalette11Chunk ReadBinary(BinaryReader reader)
	{
		var ret = new OldPalette11Chunk();
		ret.NumOfPackets = reader.ReadUInt16();
		Packet[] packets = new Packet[ret.NumOfPackets];
		for (int i = 0; i < packets.Length; ++i)
		{
			var packet = new Packet();
			packet.NumOfPaletteEntries = reader.ReadByte();
			packet.NumOfColors = reader.ReadByte();
			packet.Colors = new RGB63[packet.NumOfColors];
			for (int j = 0; j < packet.Colors.Length; ++j)
			{
				packet.Colors[j] = RGB63.ReadBinary(reader);
			}
			packets[i] = packet;
		}
		ret.Packets = packets;
		
		return ret;
	}
}