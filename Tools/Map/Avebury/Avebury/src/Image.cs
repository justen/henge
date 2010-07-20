using System;
namespace Avebury
{
	public class Image
	{
		private byte[] red;
		private byte[] green;
		private byte[] blue;
		private byte[] alpha;
		
		public int Width 	{get; protected set;}
		public int Height 	{get; protected set;}
		
		public byte GetRed(int x, int y)
		{
			return this.red[x+y*Width];	
		}
		
		public byte GetGreen(int x, int y)
		{
			return this.green[x+y*Width];	
		}
		
		public byte GetBlue(int x, int y)
		{
			return this.blue[x+y*Width];	
		}
		
		public byte GetAlpha(int x, int y)
		{
			return this.alpha[x+y*Width];	
		}
		
		public void SetRed(int x, int y, byte pixel)
		{
			this.red[x+y*Width] = pixel;	
		}
		
		public void SetGreen(int x, int y, byte pixel)
		{
			this.green[x+y*Width] = pixel;	
		}
		
		public void SetBlue(int x, int y, byte pixel)
		{
			this.blue[x+y*Width] = pixel;	
		}
		
		public void SetAlpha(int x, int y, byte pixel)
		{
			this.alpha[x+y*Width] = pixel;	
		}
		
		public Image (byte[] red, byte[] green, byte[] blue, byte[] alpha, int width, int height)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = alpha;
			this.Width = width;
			this.Height = height;
		}
		
		public Image (int width, int height)
		{
			this.Width = width;
			this.Height = height;			
			this.red = new byte[width*height];
			this.green = new byte[width*height];
			this.blue = new byte[width*height];
			this.alpha = new byte[width*height];
		}
	}
}

