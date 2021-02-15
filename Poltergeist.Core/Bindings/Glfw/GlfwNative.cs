using System;
using Poltergeist.Core.Bindings.Glfw.Structures;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Bindings.Glfw
{
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

		[DllImport(GlfwLibrary, EntryPoint = "glfwInit", CallingConvention = Convention)]
		public static extern int Init();

		[DllImport(GlfwLibrary, EntryPoint = "glfwTerminate", CallingConvention = Convention)]
		public static extern void Terminate();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void* GetProcessAddress([MarshalAs(UnmanagedType.LPUTF8Str)] string processName);

		[DllImport(GlfwLibrary, EntryPoint = "glfwMakeContextCurrent", CallingConvention = Convention)]
		public static extern void MakeContextCurrent(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersion", CallingConvention = Convention)]
		public static extern void GetVersion(out int major, out int minor, out int rev);

		#region Window
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
		public static extern void SetWindowOpacity (GlfwWindow* window, float opacity);

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

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = Convention)]
		public static extern void* GetWindowUserPointer(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPollEvents", CallingConvention = Convention)]
		public static extern void PollEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEvents", CallingConvention = Convention)]
		public static extern void WaitEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = Convention)]
		public static extern void WaitEventsTimeout(double timeout);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPostEmptyEvent", CallingConvention = Convention)]
		public static extern void PostEmptyEvent();

		[DllImport(GlfwLibrary, EntryPoint = "glfwSwapBuffers", CallingConvention = Convention)]
		public static extern void SwapBuffers(GlfwWindow* window);
		#endregion

	}
}
