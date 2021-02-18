using System;

namespace Poltergeist.Core.Bindings.OpenGl
{
	[Flags]
	public enum OpenGlClearMask
	{
		DepthBufferBit = 0x00000100,
		StencilBufferBit = 0x00000400,
		ColorBufferBit = 0x00004000
	}
}
