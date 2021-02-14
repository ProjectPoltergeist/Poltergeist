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

		#region Basic
		[DllImport(GlfwLibrary, EntryPoint = "glfwInit", CallingConvention = Convention)]
		public static extern int Init();
		#endregion
		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint(int hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHintString", CallingConvention = Convention)]
		public static extern void WindowHint(int hint, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDefaultWindowHints", CallingConvention = Convention)]
		public static extern void DefaultWindowHints(int hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void* GetProcessAddress([MarshalAs(UnmanagedType.LPUTF8Str)] string processName);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateWindow", CallingConvention = Convention, BestFitMapping = false)]
		public static extern GlfwWindow* CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPUTF8Str)] string title, GlfwMonitor* monitor, GlfwWindow* share);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDestroyWindow", CallingConvention = Convention)]
		public static extern void DestroyWindow(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwMakeContextCurrent", CallingConvention = Convention)]
		public static extern void MakeContextCurrent(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwTerminate", CallingConvention = Convention)]
		public static extern void Terminate();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowShouldClose", CallingConvention = Convention)]
		public static extern int WindowShouldClose(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowTitle", CallingConvention = Convention)]
		public static extern void SetWindowTitle(GlfwWindow* window, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPollEvents", CallingConvention = Convention)]
		public static extern void PollEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwSwapBuffers", CallingConvention = Convention)]
		public static extern void SwapBuffers(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersion", CallingConvention = Convention)]
		public static extern void GetVersion(out int major, out int minor, out int rev);

		#region Input
		[DllImport(GlfwLibrary, EntryPoint = "glfwSetKeyCallback", CallingConvention = Convention)]
		public static extern void SetKeyCallback(GlfwWindow* window);

		#endregion
	}
}
