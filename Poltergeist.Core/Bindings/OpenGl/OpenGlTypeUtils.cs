using System;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public static class OpenGlTypeUtils
	{
		public static int SizeOf(OpenGlType type)
		{
			return type switch
			{
				OpenGlType.UnsignedByte => sizeof(byte),
				OpenGlType.Short => sizeof(short),
				OpenGlType.UnsignedShort => sizeof(ushort),
				OpenGlType.Int => sizeof(int),
				OpenGlType.UnsignedInt => sizeof(uint),
				OpenGlType.Float => sizeof(float),
				OpenGlType.Double => sizeof(double),
				_ => throw new NotImplementedException()
			};
		}
	}
}
