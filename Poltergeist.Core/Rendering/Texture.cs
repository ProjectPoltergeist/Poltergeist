using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poltergeist.Core.Bindings.OpenGl;

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
			uint textureId;
			
			OpenGl3Native.GenerateTextures(1, &textureId);
			
			Texture texture = new(textureId);

			OpenGl3Native.BindTexture(OpenGlTextureType.Texture2D, textureId);
			
			OpenGl3Native.TextureParameter(OpenGlTextureType.Texture2D, OpenGlTextureParameter.TextureMagnifyingFilter,
				OpenGlTextureParameterValue.Linear);
			OpenGl3Native.TextureParameter(OpenGlTextureType.Texture2D, OpenGlTextureParameter.TextureMinifyingFilter,
				OpenGlTextureParameterValue.Linear);
			OpenGl3Native.TextureParameter(OpenGlTextureType.Texture2D, OpenGlTextureParameter.TextureWrapS,
				OpenGlTextureParameterValue.Clamp);
			OpenGl3Native.TextureParameter(OpenGlTextureType.Texture2D, OpenGlTextureParameter.TextureWrapT,
				OpenGlTextureParameterValue.Clamp);

			Bitmap bitmap = new(filePath);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb);
			void* rawData = bitmapData.Scan0.ToPointer();
			
			OpenGl3Native.TextureImage2D(OpenGlTextureType.Texture2D, 0, OpenGlTextureFormat.Rgb, bitmap.Width,
				bitmap.Height,
				OpenGlTextureFormat.Bgr, OpenGlType.UnsignedByte, rawData);

			bitmap.UnlockBits(bitmapData);

			texture.Unbind();

			return texture;
		}

		public void Bind(int slot)
		{
			OpenGl3Native.ActiveTexture(slot);
			OpenGl3Native.BindTexture(OpenGlTextureType.Texture2D, _textureId);
		}

		public void Unbind()
		{
			OpenGl3Native.BindTexture(OpenGlTextureType.Texture2D, 0);
		}

		public void Dispose()
		{
			fixed (uint* textureIdPointer = &_textureId)
				OpenGl3Native.DeleteTextures(1, textureIdPointer);
		}
	}
}
