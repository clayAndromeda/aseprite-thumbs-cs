namespace AsepriteThumbs.FileFormats.Chunks;

public class LayerChunk : IBinaryReadableChunk<LayerChunk>
{
    /*
     * Layer Chunk (0x2004)
	 * In the first frame should be a set of layer chunks to determine the entire layers layout:
     */
    
    public ushort Flags { get; set; }
    public ushort LayerType { get; set; }
    public ushort LayerChildLevel { get; set; }
    public ushort DefaultLayerWidth { get; set; }
    public ushort DefaultLayerHeight { get; set; }
    public ushort BlendMode { get; set; }
    public byte Opacity { get; set; }
    public byte[] ForFuture { get; set; } = new byte[3];
    public STRING LayerName { get; set; }
    public uint? TilesetIndex { get; set; }

    public static LayerChunk ReadBinary(BinaryReader reader, ChunkHeader header)
    {
        var ret = new LayerChunk();
        ret.Flags = reader.ReadUInt16();
        ret.LayerType = reader.ReadUInt16();
        ret.LayerChildLevel = reader.ReadUInt16();
        ret.DefaultLayerWidth = reader.ReadUInt16();
        ret.DefaultLayerHeight = reader.ReadUInt16();
        ret.BlendMode = reader.ReadUInt16();
        ret.Opacity = reader.ReadByte();
        ret.ForFuture = reader.ReadBytes(3);
        ret.LayerName = STRING.ReadBinary(reader);

        if (ret.LayerType == 2)
        {
            ret.TilesetIndex = reader.ReadUInt32();
        }

        return ret;
    }
}