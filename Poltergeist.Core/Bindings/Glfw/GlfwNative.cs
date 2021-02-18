using System;
using System.Collections.Generic;
using Poltergeist.Core.Bindings.Glfw.Structures;
using System.Runtime.InteropServices;
using Poltergeist.Core.Memory;

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

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersion", CallingConvention = Convention)]
		public static extern void GetVersion(out int major, out int minor, out int rev);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersionString", CallingConvention = Convention)]
		public static extern IntPtr GetVersionString();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetError", CallingConvention = Convention)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern GlfwError GetError([MarshalAs(UnmanagedType.LPUTF8Str)] ref string description);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetErrorCallback", CallingConvention = Convention)]
		public static extern GlfwErrorFun SetErrorCallback(GlfwErrorFun callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitors", CallingConvention = Convention)]
		public static extern IntPtr GetMonitors(out int count);

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
		public static extern IntPtr GetMonitorName(GlfwMonitor* monitor);

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

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHintString", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void WindowHint([MarshalAs(UnmanagedType.I4)] GlfwHint hint, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

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
		public static extern int GetWindowAttrib(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwHint attrib);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowAttrib", CallingConvention = Convention)]
		public static extern void SetWindowAttrib(GlfwWindow* window, [MarshalAs(UnmanagedType.I4)] GlfwHint attrib, int value);

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
		//Last


		[DllImport(GlfwLibrary, EntryPoint = "glfwGetProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void* GetProcessAddress([MarshalAs(UnmanagedType.LPUTF8Str)] string processName);

		[DllImport(GlfwLibrary, EntryPoint = "glfwMakeContextCurrent", CallingConvention = Convention)]
		public static extern void MakeContextCurrent(GlfwWindow* window);



		[DllImport(GlfwLibrary, EntryPoint = "glfwSwapBuffers", CallingConvention = Convention)]
		public static extern void SwapBuffers(GlfwWindow* window);

		#endregion
	}
}
