namespace AsepriteThumbs.FileFormats.Chunks;

public class CelChunk : IBinaryReadable<CelChunk>
{
	/*
	 * Cel Chunk (0x2005)
	 * 
	 * This chunk determine where to put a cel in the specified layer/frame.
	 *
	 * CelType (WORD)
	 *	0 - Raw Image Data (unsupported)
	 *  1 - Linked Cel (unsupported)
	 *  2 - Compressed Image
	 *  3 - Compression TileMap (unsupported)
	 */
	
	public static CelChunk ReadBinary(BinaryReader reader)
	{
		// 圧縮されているので、対応する
		// 各ピクセルのバイト数と順序は、カラーモード・タイル形式に依存する
		
		throw new NotImplementedException();
	}
}