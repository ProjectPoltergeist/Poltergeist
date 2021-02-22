using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.ES20;

using PixelFormat = System.Drawing.Imaging.PixelFormat;
using OpenTkPixelFormat = OpenTK.Graphics.ES30.PixelFormat;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct Texture : IDisposable
	{
		private uint _textureId;

		private Texture(uint textureId)
		{
			_textureId = textureId;
		}

		public static Texture Create(string filePath)
		{
			var textureId = 0u;		
			GL.GenTextures(1, &textureId);
			
			var texture = new Texture(textureId);

			GL.BindTexture(TextureTarget.Texture2D, textureId);
			
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

			Bitmap bitmap = new(filePath);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb);
			var rawData = bitmapData.Scan0;
			
			GL.TexImage2D(All.Texture2D, 0, All.Rgb, bitmap.Width, bitmap.Height,0, All.AbgrExt, All.UnsignedByte, rawData); //nie wiem czemu ale nie przyjmuje enumów normalnych

			bitmap.UnlockBits(bitmapData);
			texture.Unbind();

			return texture;
		}

		public void Bind(int slot) //check this bullshit
		{
			var textureUnit = (TextureUnit)(33984 + slot);
			GL.ActiveTexture(textureUnit);
			GL.BindTexture(TextureTarget.Texture2D, _textureId);
		}

		public void Unbind()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void Dispose()
		{
			fixed (uint* textureIdPointer = &_textureId)
				GL.DeleteTextures(1, textureIdPointer);
		}
	}
}
