namespace AsepriteThumbs.FileFormats.Chunks;

public class LayerChunk : IBinaryReadable<LayerChunk>
{
    /*
     * Layer Chunk (0x2004)
	 * In the first frame should be a set of layer chunks to determine the entire layers layout:
     */
    
    public UInt16 Flags { get; set; }
    public UInt16 LayerType { get; set; }
    public UInt16 LayerChildLevel { get; set; }
    public UInt16 DefaultLayerWidth { get; set; }
    public UInt16 DefaultLayerHeight { get; set; }
    public UInt16 BlendMode { get; set; }
    public Byte Opacity { get; set; }
    public Byte[] ForFuture { get; set; } = new byte[3];
    public STRING LayerName { get; set; }
    public UInt32? TilesetIndex { get; set; }

    public static LayerChunk ReadBinary(BinaryReader reader)
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