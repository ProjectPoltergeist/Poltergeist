using System;
using System.Runtime.InteropServices;

using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Bindings.Glfw
{
	//https://www.glfw.org/docs/latest/glfw3_8h.html
	public static unsafe class GlfwNative
	{
		private const string GlfwLibrary = "glfw3";
		private const CallingConvention Convention = CallingConvention.Cdecl;

		public static readonly Version Version;
		public static readonly bool Loaded;

		static GlfwNative()
		{
			try
			{
				GetVersion(out int major, out int minor, out int rev);
				Version = new Version(major, minor, rev);
				Loaded = true;
			}
			catch
			{
				Loaded = false;
			}
		}

		#region Function Pointers
		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwGlProc();

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwVkProc();

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwErrorFun(int errorcode, string description);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowPosFun(GlfwWindow* window, int xpos, int ypos);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowSizeFun(GlfwWindow* window, int width, int height);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowCloseFun(GlfwWindow* window);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowRefreshFun(GlfwWindow* window);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowFocusFun(GlfwWindow* window, int focused);

		public delegate void GlfwWindowIconifyFun(GlfwWindow* window, int iconified);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowMaximizeFun(GlfwWindow* window, int maximized);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwFramebufferSizeFun(GlfwWindow* window, int width, int height);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwWindowContentScaleFun(GlfwWindow* window, float xscale, float yscale);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwMouseButtonFun(GlfwWindow* window, int button, int action, int mods);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwCursorPosFun(GlfwWindow* window, double xmouse, double ymouse);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwCursorEnterFun(GlfwWindow* window, int entered);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwScrollFun(GlfwWindow* window, double xoffset, double yoffset);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwKeyFun(GlfwWindow* window, int key, int scancode, int action, int mods);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwCharFun(GlfwWindow* window, uint codepoint);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwCharModsFun(GlfwWindow* window, int codepoint, int mods);
		
		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwDropFun(GlfwWindow* window, int count, string[] paths);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwMonitorFun(GlfwWindow* window, int monitorevent);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void GlfwJoystickFun(int jid, int ev);
		#endregion

		#region Functions
		[DllImport(GlfwLibrary, EntryPoint = "glfwInit", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg Init();

		[DllImport(GlfwLibrary, EntryPoint = "glfwTerminate", CallingConvention = Convention)]
		public static extern void Terminate();

		[DllImport(GlfwLibrary, EntryPoint = "glfwInitHint", CallingConvention = Convention)]
		public static extern void InitHint([MarshalAs(UnmanagedType.I4)] GlfwHint hint, [MarshalAs(UnmanagedType.I4)] GlfwArg value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwInitHint", CallingConvention = Convention)]
		public static extern void InitHint([MarshalAs(UnmanagedType.I4)] GlfwHint hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersion", CallingConvention = Convention)]
		public static extern void GetVersion(out int major, out int minor, out int rev);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersionString", CallingConvention = Convention)]
		public static extern char* GetVersionString();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetError", CallingConvention = Convention, BestFitMapping = false)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwError GetError([MarshalAs(UnmanagedType.LPUTF8Str)] ref string description);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetErrorCallback", CallingConvention = Convention)]
		public static extern GlfwErrorFun SetErrorCallback(GlfwErrorFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitors", CallingConvention = Convention)]
		public static extern GlfwMonitor** GetMonitors(out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = Convention)]
		public static extern GlfwMonitor* GetPrimaryMonitor();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorPos", CallingConvention = Convention)]
		public static extern void GetMonitorPos(GlfwMonitor* monitor, out int xpos, out int ypos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = Convention)]
		public static extern void GetMonitorWorkarea(GlfwMonitor* monitor, out int xpos, out int ypos, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = Convention)]
		public static extern void GetMonitorPhysicalScale(GlfwMonitor* monitor, out int widthMM, out int heightMM);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = Convention)]
		public static extern void GetMonitorContentScale(GlfwMonitor* monitor, out float xscale, out float yscale);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorName", CallingConvention = Convention)]
		public static extern char* GetMonitorName(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = Convention)]
		public static extern void SetMonitorUserPointer(GlfwMonitor* monitor, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = Convention)]
		public static extern void* GetMonitorUserPointer(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMonitorCallback", CallingConvention = Convention)]
		public static extern GlfwMonitorFun SetMonitorCallback(GlfwMonitorFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVideoModes", CallingConvention = Convention)]
		public static extern IntPtr GetVideoModes(GlfwMonitor* monitor, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVideoMode", CallingConvention = Convention)]
		public static extern GlfwVidMode* GetVideoMode(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetGamma", CallingConvention = Convention)]
		public static extern void SetGamma(GlfwMonitor* monitor, float gamma);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGammaRamp", CallingConvention = Convention)]
		public static extern GlfwGammaRamp* GetGammaRamp(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetGammaRamp", CallingConvention = Convention)]
		public static extern void SetGammaRamp(GlfwMonitor* monitor, GlfwGammaRamp* ramp);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDefaultWindowHints", CallingConvention = Convention)]
		public static extern void DefaultWindowHints();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint([MarshalAs(UnmanagedType.I4)] GlfwHint hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint([MarshalAs(UnmanagedType.I4)] GlfwHint hint, [MarshalAs(UnmanagedType.I4)] GlfwArg value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHintString", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void WindowHintString([MarshalAs(UnmanagedType.I4)] GlfwHint hint, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateWindow", CallingConvention = Convention, BestFitMapping = false)]
		public static extern GlfwWindow* CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPUTF8Str)] string title, GlfwMonitor* monitor, GlfwWindow* share);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDestroyWindow", CallingConvention = Convention)]
		public static extern void DestroyWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowShouldClose", CallingConvention = Convention)]
		public static extern int WindowShouldClose(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = Convention)]
		public static extern int SetWindowShouldClose(GlfwWindow* window, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowTitle", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void SetWindowTitle(GlfwWindow* window, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowIcon", CallingConvention = Convention)]
		public static extern void SetWindowIcon(GlfwWindow* window, int count, GlfwImage* images);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowPos", CallingConvention = Convention)]
		public static extern void GetWindowPos(GlfwWindow* window, out int xpos, out int ypos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowPos", CallingConvention = Convention)]
		public static extern void SetWindowPos(GlfwWindow* window, int xpos, int ypos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowSize", CallingConvention = Convention)]
		public static extern void GetWindowSize(GlfwWindow* window, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSizeLimits", CallingConvention = Convention)]
		public static extern void SetWindowSizeLimits(GlfwWindow* window, int minwidth, int minheight, int maxwidth, int maxheight);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowAspectRatio", CallingConvention = Convention)]
		public static extern void SetWindowAspectRatio(GlfwWindow* window, int numer, int denom);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSize", CallingConvention = Convention)]
		public static extern void SetWindowSize(GlfwWindow* window, int width, int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetFramebufferSize", CallingConvention = Convention)]
		public static extern void GetFramebufferSize(GlfwWindow* window, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = Convention)]
		public static extern void GetWindowFrameSize(GlfwWindow* window, out int left, out int top, out int right, out int bottom);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowContentScale", CallingConvention = Convention)]
		public static extern void GetWindowContentScale(GlfwWindow* window, out float xscale, out float yscale);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowOpacity", CallingConvention = Convention)]
		public static extern float GetWindowOpacity(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowOpacity", CallingConvention = Convention)]
		public static extern void SetWindowOpacity(GlfwWindow* window, float opacity);

		[DllImport(GlfwLibrary, EntryPoint = "glfwIconifyWindow", CallingConvention = Convention)]
		public static extern void IconifyWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwRestoreWindow", CallingConvention = Convention)]
		public static extern void RestoreWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwMaximizeWindow", CallingConvention = Convention)]
		public static extern void MaximizeWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwShowWindow", CallingConvention = Convention)]
		public static extern void ShowWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwHideWindow", CallingConvention = Convention)]
		public static extern void HideWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwFocusWindow", CallingConvention = Convention)]
		public static extern void FocusWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwRequestWindowAttention", CallingConvention = Convention)]
		public static extern void RequestWindowAttention(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowMonitor", CallingConvention = Convention)]
		public static extern GlfwMonitor* GetWindowMonitor(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowMonitor", CallingConvention = Convention)]
		public static extern void SetWindowMonitor(GlfwWindow* window, GlfwMonitor* monitor, int xpos, int ypos, int width, int height, int refreshRate);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowAttrib", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetWindowAttrib(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwHint attrib);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowAttrib", CallingConvention = Convention)]
		public static extern void SetWindowAttrib(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwHint attrib, [MarshalAs(UnmanagedType.I4)] GlfwArg value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = Convention)]
		public static extern void SetWindowUserPointer(GlfwWindow* window, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowUserPointe", CallingConvention = Convention)]
		public static extern void* GetWindowUserPointer(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = Convention)]
		public static extern GlfwWindowPosFun SetWindowPosCallback(GlfwWindow* window, GlfwWindowPosFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = Convention)]
		public static extern GlfwWindowSizeFun SetWindowSizeCallback(GlfwWindow* window, GlfwWindowSizeFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = Convention)]
		public static extern GlfwWindowCloseFun SeWindowClosesCallback(GlfwWindow* window, GlfwWindowCloseFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = Convention)]
		public static extern GlfwWindowRefreshFun SetWindowRefreshCallback(GlfwWindow* window, GlfwWindowRefreshFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = Convention)]
		public static extern GlfwWindowFocusFun SetWindowFocusCallback(GlfwWindow* window, GlfwWindowFocusFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = Convention)]
		public static extern GlfwWindowIconifyFun SetWindowIconifyCallback(GlfwWindow* window, GlfwWindowIconifyFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = Convention)]
		public static extern GlfwWindowMaximizeFun SetWindowMaximizeCallback(GlfwWindow* window, GlfwWindowMaximizeFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = Convention)]
		public static extern GlfwFramebufferSizeFun SeFramebufferSizeCallback(GlfwWindow* window, GlfwFramebufferSizeFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowContentScaleCallback", CallingConvention = Convention)]
		public static extern GlfwWindowContentScaleFun SetWindowContentScaleCallback(GlfwWindow* window, GlfwWindowContentScaleFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPollEvents", CallingConvention = Convention)]
		public static extern void PollEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEvents", CallingConvention = Convention)]
		public static extern void WaitEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = Convention)]
		public static extern void WaitEventsTimeout(double timeout);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPostEmptyEvent", CallingConvention = Convention)]
		public static extern void PostEmptyEvent();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetInputMode", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetInputMode(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwInput mode);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetInputMode", CallingConvention = Convention)]
		public static extern void glfwSetInputMode(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwInput mode, [MarshalAs(UnmanagedType.I4)] GlfwArg value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg RawMouseMotionSupported();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKeyName", CallingConvention = Convention)]
		public static extern IntPtr GetKeyName([MarshalAs(UnmanagedType.I4)] GlfwInput key, int scancode);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKeyScancode", CallingConvention = Convention)]
		public static extern int GetKeyScancode([MarshalAs(UnmanagedType.I4)] GlfwInput key);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKey", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetKey(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwInput key);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMouseButto", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetMouseButton(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwInput button);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetCursorPos", CallingConvention = Convention)]
		public static extern void GetCursorPos(GlfwWindow* window, out double xpos, out double ypos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursorPos", CallingConvention = Convention)]
		public static extern void SetCursorPos(GlfwWindow* window, double xpos, double ypos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateCursor", CallingConvention = Convention)]
		public static extern GlfwCursor* CreateCursor(GlfwImage* image, int xhot, int yhot);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateStandardCursor", CallingConvention = Convention)]
		public static extern GlfwCursor* CreateStandardCursor([MarshalAs(UnmanagedType.I4)] GlfwInput shape);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDestroyCursor", CallingConvention = Convention)]
		public static extern void DestroyCursor(GlfwCursor* cursor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursor", CallingConvention = Convention)]
		public static extern void SetCursor(GlfwWindow* window, GlfwCursor* cursor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetKeyCallback", CallingConvention = Convention)]
		public static extern GlfwKeyFun SetKeyCallback(GlfwWindow* window, GlfwKeyFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCharCallback", CallingConvention = Convention)]
		public static extern GlfwCharFun SetCharCallback(GlfwWindow* window, GlfwCharFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCharModsCallback", CallingConvention = Convention)]
		public static extern GlfwCharModsFun SetCharModsCallback(GlfwWindow* window, GlfwCharModsFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = Convention)]
		public static extern GlfwMouseButtonFun SetMouseButtonCallback(GlfwWindow* window, GlfwMouseButtonFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = Convention)]
		public static extern GlfwCursorPosFun SetCursorPosCallback(GlfwWindow* window, GlfwCursorPosFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursorEnterCallback", CallingConvention = Convention)]
		public static extern GlfwCursorEnterFun SetCursorEnterCallback(GlfwWindow* window, GlfwCursorEnterFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetScrollCallback", CallingConvention = Convention)]
		public static extern GlfwScrollFun SetScrollCallback(GlfwWindow* window, GlfwScrollFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetDropCallback", CallingConvention = Convention)]
		public static extern GlfwDropFun SetDropCallback(GlfwWindow* window, GlfwDropFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwJoystickPresent", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg JoystickPresent(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickAxes", CallingConvention = Convention)]
		public static extern float* GetJoystickAxes([MarshalAs(UnmanagedType.I4)] GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickButtons", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern GlfwArg* GetJoystickButtons([MarshalAs(UnmanagedType.I4)] GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickHats", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern GlfwInput* GetJoystickHats([MarshalAs(UnmanagedType.I4)] GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickName", CallingConvention = Convention)]
		public static extern char* GetJoystickName([MarshalAs(UnmanagedType.I4)] GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickGUID", CallingConvention = Convention)]
		public static extern char* GetJoystickGUID([MarshalAs(UnmanagedType.I4)] GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = Convention)]
		public static extern void SetJoystickUserPointer([MarshalAs(UnmanagedType.I4)] GlfwInput jid, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetJoystickUserPointer([MarshalAs(UnmanagedType.I4)] GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg JoystickIsGamepad([MarshalAs(UnmanagedType.I4)] GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetJoystickCallback", CallingConvention = Convention)]
		public static extern GlfwJoystickFun SetJoystickCallback(GlfwJoystickFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = Convention, BestFitMapping = false)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg UpdateGamepadMappings([MarshalAs(UnmanagedType.LPUTF8Str)] string @string);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGamepadName", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern char* GetGamepadName([MarshalAs(UnmanagedType.I4)] GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGamepadState", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetGamepadState([MarshalAs(UnmanagedType.I4)] GlfwInput jid, GlfwGamepadState* state);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetClipboardString", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void SetClipboardString(GlfwWindow* window, [MarshalAs(UnmanagedType.LPUTF8Str)] string @string);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetClipboardString", CallingConvention = Convention)]
		public static extern char* GetClipboardString(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetTime", CallingConvention = Convention)]
		public static extern double GetTime();

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetTime", CallingConvention = Convention)]
		public static extern void SetTime(double time);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetTimerValue", CallingConvention = Convention)]
		public static extern ulong GetTimerValue();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetTimerFrequency", CallingConvention = Convention)]
		public static extern ulong GetTimerFrequency();

		[DllImport(GlfwLibrary, EntryPoint = "glfwMakeContextCurrent", CallingConvention = Convention)]
		public static extern void MakeContextCurrent(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetCurrentContext", CallingConvention = Convention)]
		public static extern GlfwWindow* GetCurrentContext();

		[DllImport(GlfwLibrary, EntryPoint = "glfwSwapBuffers", CallingConvention = Convention)]
		public static extern void SwapBuffers(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSwapInterval", CallingConvention = Convention)]
		public static extern void SwapInterval(int interval);

		[DllImport(GlfwLibrary, EntryPoint = "glfwExtensionSupported", CallingConvention = Convention, BestFitMapping = false)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg ExtensionSupported([MarshalAs(UnmanagedType.LPUTF8Str)] string extension);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void* GetProcessAddress([MarshalAs(UnmanagedType.LPUTF8Str)] string procname);
		/*
		[DllImport(GlfwLibrary, EntryPoint = "glfwVulkanSupported", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg VulkanSupported();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetRequiredInstanceExtensions", CallingConvention = Convention)]
		public static extern char** GetRequiredInstanceExtensions(out uint count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetInstanceProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern GlfwVkProc GetInstanceProcAddress(VkInstance instance, [MarshalAs(UnmanagedType.LPUTF8Str)] string procname);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetPhysicalDevicePresentationSupport", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwArg GetPhysicalDevicePresentationSupport(VkInstance instance, VkPhysicalDevice device, uint queuefamily);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateWindowSurface", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern VkResult CreateWindowSurface (VkInstance instance, GlfwWindow* window, VkAllocationCallbacks* allocator, VkSurfaceKHR* surface);
		*/
		#endregion
	}
}
