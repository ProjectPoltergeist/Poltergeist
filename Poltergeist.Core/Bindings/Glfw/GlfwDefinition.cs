using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poltergeist.Core.Bindings.Glfw
{
	public static class GlfwDefinition
	{
		#region Window
		public const uint FOCUSED = 0x00020001;
		public const uint ICONIFIED = 0x00020002;
		public const uint RESIZABLE = 0x00020003;
		public const uint VISIBLE = 0x00020004;
		public const uint DECORATED = 0x00020005;
		public const uint AUTO_ICONIFY = 0x00020006;
		public const uint FLOATING = 0x00020007;
		public const uint MAXIMIZED = 0x00020008;
		public const uint CENTER_CURSOR = 0x00020009;
		public const uint TRANSPARENT_FRAMEBUFFER = 0x0002000A;
		public const uint HOVERED = 0x0002000B;
		public const uint FOCUS_ON_SHOW = 0x0002000C;
		#endregion

		#region Framebuffer
		public const uint RED_BITS = 0x00021001;
		public const uint GREEN_BITS = 0x00021002;
		public const uint BLUE_BITS = 0x00021003;
		public const uint ALPHA_BITS = 0x00021004;
		public const uint DEPTH_BITS = 0x00021005;
		public const uint STENCIL_BITS = 0x00021006;
		public const uint ACCUM_RED_BITS = 0x00021007;
		public const uint ACCUM_GREEN_BITS = 0x00021008;
		public const uint ACCUM_BLUE_BITS = 0x00021009;
		public const uint ACCUM_ALPHA_BITS = 0x0002100A;
		public const uint AUX_BUFFERS = 0x0002100B;
		public const uint STEREO = 0x0002100C;
		public const uint SAMPLES = 0x0002100D;
		public const uint SRGB_CAPABLE = 0x0002100E;
		public const uint REFRESH_RATE = 0x0002100F;
		public const uint DOUBLEBUFFER = 0x00021010;
		#endregion

		#region Context client API
		public const uint CLIENT_API = 0x00022001;
		public const uint CONTEXT_VERSION_MAJOR = 0x00022002;
		public const uint CONTEXT_VERSION_MINOR = 0x00022003;
		public const uint CONTEXT_REVISION = 0x00022004;
		public const uint CONTEXT_ROBUSTNESS = 0x00022005;
		public const uint OPENGL_FORWARD_COMPAT = 0x00022006;
		public const uint OPENGL_DEBUG_CONTEXT = 0x00022007;
		public const uint OPENGL_PROFILE = 0x00022008;
		public const uint CONTEXT_RELEASE_BEHAVIOR = 0x00022009;
		public const uint CONTEXT_NO_ERROR = 0x0002200A;
		public const uint CONTEXT_CREATION_API = 0x0002200B;
		public const uint SCALE_TO_MONITOR = 0x0002200C;
		#endregion
	}
}
