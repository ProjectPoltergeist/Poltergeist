﻿namespace Poltergeist.Core.Bindings.Glfw.Enums
{
	public enum GlfwHint 
	{
		#region Window
		FOCUSED = 0x00020001,

		ICONIFIED = 0x00020002,

		RESIZABLE = 0x00020003,

		VISIBLE = 0x00020004,

		DECORATED = 0x00020005,

		AUTO_ICONIFY = 0x00020006,

		FLOATING = 0x00020007,

		MAXIMIZED = 0x00020008,

		CENTER_CURSOR = 0x00020009,

		TRANSPARENT_FRAMEBUFFER = 0x0002000A,

		HOVERED = 0x0002000B,

		FOCUS_ON_SHOW = 0x0002000C,
		#endregion

		#region Framebuffer
		RED_BITS = 0x00021001,

		GREEN_BITS = 0x00021002,

		BLUE_BITS = 0x00021003,

		ALPHA_BITS = 0x00021004,

		DEPTH_BITS = 0x00021005,

		STENCIL_BITS = 0x00021006,

		ACCUM_RED_BITS = 0x00021007,

		ACCUM_GREEN_BITS = 0x00021008,

		ACCUM_BLUE_BITS = 0x00021009,

		ACCUM_ALPHA_BITS = 0x0002100A,

		AUX_BUFFERS = 0x0002100B,

		STEREO = 0x0002100C,

		SAMPLES = 0x0002100D,

		SRGB_CAPABLE = 0x0002100E,

		REFRESH_RATE = 0x0002100F,

		DOUBLEBUFFER = 0x00021010,
		#endregion

		#region Context client API
		CLIENT_API = 0x00022001,

		CONTEXT_VERSION_MAJOR = 0x00022002,

		CONTEXT_VERSION_MINOR = 0x00022003,

		CONTEXT_REVISION = 0x00022004,

		CONTEXT_ROBUSTNESS = 0x00022005,

		OPENGL_FORWARD_COMPAT = 0x00022006,

		OPENGL_DEBUG_CONTEXT = 0x00022007,

		OPENGL_PROFILE = 0x00022008,

		CONTEXT_RELEASE_BEHAVIOR = 0x00022009,

		CONTEXT_NO_ERROR = 0x0002200A,

		CONTEXT_CREATION_API = 0x0002200B,

		SCALE_TO_MONITOR = 0x0002200C,
		#endregion
	}
}
