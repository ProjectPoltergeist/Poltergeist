using System;
using System.Runtime.InteropServices;
using Poltergeist.Core.Bindings.Glfw.Enums;
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
				GetVersion(out int major, out int minor, out int revision);
				Version = new Version(major, minor, revision);
				Loaded = true;
			}
			catch
			{
				Loaded = false;
			}
		}

		#region Function Pointers
		[UnmanagedFunctionPointer(Convention)]
		public delegate void ErrorCallback(GlfwError errorCode, string description);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowPositionCallback(GlfwWindow* window, int xPosition, int yPosition);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowSizeCallback(GlfwWindow* window, int width, int height);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowCloseCallback(GlfwWindow* window);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowRefreshCallback(GlfwWindow* window);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowFocusCallback(GlfwWindow* window, bool focused);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowIconifyCallback(GlfwWindow* window, bool iconified);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowMaximizeCallback(GlfwWindow* window, bool maximized);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void FramebufferSizeCallback(GlfwWindow* window, int width, int height);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void WindowContentScaleCallback(GlfwWindow* window, float xScale, float yScale);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void MouseButtonCallback(GlfwWindow* window, GlfwInput button, GlfwInputState action, GlfwKeyMode mods);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void CursorPositionCallback(GlfwWindow* window, double xMouse, double yMouse);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void CursorEnterCallback(GlfwWindow* window, bool entered);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void ScrollCallback(GlfwWindow* window, double xOffset, double yOffset);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void KeyCallback(GlfwWindow* window, GlfwInput key, int scanCode, GlfwInputState action, GlfwKeyMode mods);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void CharacterCallback(GlfwWindow* window, uint codePoint);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void CharacterModifiersCallback(GlfwWindow* window, int codePoint, GlfwKeyMode mods);
		
		[UnmanagedFunctionPointer(Convention)]
		public delegate void DropCallback(GlfwWindow* window, int count, string[] paths);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void MonitorCallback(GlfwWindow* window, GlfwEvent state);

		[UnmanagedFunctionPointer(Convention)]
		public delegate void JoystickCallback(GlfwInput jid, GlfwEvent state);
		#endregion

		#region Functions
		[DllImport(GlfwLibrary, EntryPoint = "glfwInit", CallingConvention = Convention)]
		public static extern bool Initialize();

		[DllImport(GlfwLibrary, EntryPoint = "glfwTerminate", CallingConvention = Convention)]
		public static extern void Terminate();

		[DllImport(GlfwLibrary, EntryPoint = "glfwInitHint", CallingConvention = Convention)]
		public static extern void InitializeHint(GlfwHint hint, bool value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwInitHint", CallingConvention = Convention)]
		public static extern void InitializeHint(GlfwHint hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersion", CallingConvention = Convention)]
		public static extern void GetVersion(out int major, out int minor, out int revision);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVersionString", CallingConvention = Convention)]
		public static extern byte* GetVersionString();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetError", CallingConvention = Convention, BestFitMapping = false)]
		public static extern GlfwError GetError([MarshalAs(UnmanagedType.LPUTF8Str)] out string description);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetErrorCallback", CallingConvention = Convention)]
		public static extern ErrorCallback SetErrorCallback(ErrorCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitors", CallingConvention = Convention)]
		public static extern GlfwMonitor** GetMonitors(out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = Convention)]
		public static extern GlfwMonitor* GetPrimaryMonitor();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorPos", CallingConvention = Convention)]
		public static extern void GetMonitorPosition(GlfwMonitor* monitor, out int xPosition, out int yPosition);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = Convention)]
		public static extern void GetMonitorWorkarea(GlfwMonitor* monitor, out int xPosition, out int yPosition, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = Convention)]
		public static extern void GetMonitorPhysicalScale(GlfwMonitor* monitor, out int widthMM, out int heightMM);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = Convention)]
		public static extern void GetMonitorContentScale(GlfwMonitor* monitor, out float xScale, out float yScale);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMonitorName", CallingConvention = Convention)]
		public static extern byte* GetMonitorName(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = Convention)]
		public static extern void SetMonitorUserPointer(GlfwMonitor* monitor, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = Convention)]
		public static extern void* GetMonitorUserPointer(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMonitorCallback", CallingConvention = Convention)]
		public static extern MonitorCallback SetMonitorCallback(MonitorCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVideoModes", CallingConvention = Convention)]
		public static extern GlfwVideoMode* GetVideoModes(GlfwMonitor* monitor, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetVideoMode", CallingConvention = Convention)]
		public static extern GlfwVideoMode* GetVideoMode(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetGamma", CallingConvention = Convention)]
		public static extern void SetGamma(GlfwMonitor* monitor, float gamma);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGammaRamp", CallingConvention = Convention)]
		public static extern GlfwGammaRamp* GetGammaRamp(GlfwMonitor* monitor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetGammaRamp", CallingConvention = Convention)]
		public static extern void SetGammaRamp(GlfwMonitor* monitor, GlfwGammaRamp* ramp);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDefaultWindowHints", CallingConvention = Convention)]
		public static extern void DefaultWindowHints();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint(GlfwHint hint, bool value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint(GlfwHint hint, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHint", CallingConvention = Convention)]
		public static extern void WindowHint(GlfwHint hint, GlfwProfile value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwWindowHintString", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void WindowHintString(GlfwHint hint, string value);

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
		public static extern void GetWindowPosition(GlfwWindow* window, out int xPosition, out int yPosition);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowPos", CallingConvention = Convention)]
		public static extern void SetWindowPosition(GlfwWindow* window, int xPosition, int yPosition);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowSize", CallingConvention = Convention)]
		public static extern void GetWindowSize(GlfwWindow* window, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSizeLimits", CallingConvention = Convention)]
		public static extern void SetWindowSizeLimits(GlfwWindow* window, int minWidth, int minHeight, int maxWidth, int maxHeight);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowAspectRatio", CallingConvention = Convention)]
		public static extern void SetWindowAspectRatio(GlfwWindow* window, int numer, int denom);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSize", CallingConvention = Convention)]
		public static extern void SetWindowSize(GlfwWindow* window, int width, int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetFramebufferSize", CallingConvention = Convention)]
		public static extern void GetFramebufferSize(GlfwWindow* window, out int width, out int height);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = Convention)]
		public static extern void GetWindowFrameSize(GlfwWindow* window, out int left, out int top, out int right, out int bottom);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowContentScale", CallingConvention = Convention)]
		public static extern void GetWindowContentScale(GlfwWindow* window, out float xScale, out float yScale);

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
		public static extern void SetWindowMonitor(GlfwWindow* window, GlfwMonitor* monitor, int xPosition, int yPosition, int width, int height, int refreshRate);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowAttrib", CallingConvention = Convention)]
		public static extern bool GetWindowAttribute(GlfwWindow* window, GlfwHint attribute);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowAttrib", CallingConvention = Convention)]
		public static extern void SetWindowAttribute(GlfwWindow* window, GlfwHint attribute, bool value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = Convention)]
		public static extern void SetWindowUserPointer(GlfwWindow* window, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = Convention)]
		public static extern void* GetWindowUserPointer(GlfwWindow* window);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = Convention)]
		public static extern WindowPositionCallback SetWindowPositionCallback(GlfwWindow* window, WindowPositionCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = Convention)]
		public static extern WindowSizeCallback SetWindowSizeCallback(GlfwWindow* window, WindowSizeCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = Convention)]
		public static extern WindowCloseCallback SeWindowClosesCallback(GlfwWindow* window, WindowCloseCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = Convention)]
		public static extern WindowRefreshCallback SetWindowRefreshCallback(GlfwWindow* window, WindowRefreshCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = Convention)]
		public static extern WindowFocusCallback SetWindowFocusCallback(GlfwWindow* window, WindowFocusCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = Convention)]
		public static extern WindowIconifyCallback SetWindowIconifyCallback(GlfwWindow* window, WindowIconifyCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = Convention)]
		public static extern WindowMaximizeCallback SetWindowMaximizeCallback(GlfwWindow* window, WindowMaximizeCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = Convention)]
		public static extern FramebufferSizeCallback SetFramebufferSizeCallback(GlfwWindow* window, FramebufferSizeCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetWindowContentScaleCallback", CallingConvention = Convention)]
		public static extern WindowContentScaleCallback SetWindowContentScaleCallback(GlfwWindow* window, WindowContentScaleCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPollEvents", CallingConvention = Convention)]
		public static extern void PollEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEvents", CallingConvention = Convention)]
		public static extern void WaitEvents();

		[DllImport(GlfwLibrary, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = Convention)]
		public static extern void WaitEventsTimeout(double timeout);

		[DllImport(GlfwLibrary, EntryPoint = "glfwPostEmptyEvent", CallingConvention = Convention)]
		public static extern void PostEmptyEvent();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetInputMode", CallingConvention = Convention)]
		public static extern int GetInputMode(GlfwWindow* window, GlfwInputMode mode);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetInputMode", CallingConvention = Convention)]
		public static extern void SetInputMode(GlfwWindow* window, GlfwInputMode mode, int value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = Convention)]
		public static extern bool RawMouseMotionSupported();

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKeyName", CallingConvention = Convention)]
		public static extern byte* GetKeyName(GlfwInput key, int scancode);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKeyScancode", CallingConvention = Convention)]
		public static extern int GetKeyScancode(GlfwInput key);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetKey", CallingConvention = Convention)]
		public static extern GlfwInputState GetKey(GlfwWindow* window, GlfwInput key);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetMouseButton", CallingConvention = Convention)]
		public static extern GlfwInputState GetMouseButton(GlfwWindow* window, GlfwInput button);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetCursorPos", CallingConvention = Convention)]
		public static extern void GetCursorPosition(GlfwWindow* window, out double xPos, out double yPos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursorPos", CallingConvention = Convention)]
		public static extern void SetCursorPositon(GlfwWindow* window, double xPos, double yPos);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateCursor", CallingConvention = Convention)]
		public static extern GlfwCursor* CreateCursor(GlfwImage* image, int xHot, int yHot);

		[DllImport(GlfwLibrary, EntryPoint = "glfwCreateStandardCursor", CallingConvention = Convention)]
		public static extern GlfwCursor* CreateStandardCursor(GlfwCursorShape shape);

		[DllImport(GlfwLibrary, EntryPoint = "glfwDestroyCursor", CallingConvention = Convention)]
		public static extern void DestroyCursor(GlfwCursor* cursor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursor", CallingConvention = Convention)]
		public static extern void SetCursor(GlfwWindow* window, GlfwCursor* cursor);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetKeyCallback", CallingConvention = Convention)]
		public static extern KeyCallback SetKeyCallback(GlfwWindow* window, KeyCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCharCallback", CallingConvention = Convention)]
		public static extern CharacterCallback SetCharacterCallback(GlfwWindow* window, CharacterCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCharCallback", CallingConvention = Convention)]
		public static extern CharacterModifiersCallback SetCharacterModifiersCallback(GlfwWindow* window, CharacterModifiersCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = Convention)]
		public static extern MouseButtonCallback SetMouseButtonCallback(GlfwWindow* window, MouseButtonCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = Convention)]
		public static extern CursorPositionCallback SetCursorPositionCallback(GlfwWindow* window, CursorPositionCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetCursorEnterCallback", CallingConvention = Convention)]
		public static extern CursorEnterCallback SetCursorEnterCallback(GlfwWindow* window, CursorEnterCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetScrollCallback", CallingConvention = Convention)]
		public static extern ScrollCallback SetScrollCallback(GlfwWindow* window, ScrollCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetDropCallback", CallingConvention = Convention)]
		public static extern DropCallback SetDropCallback(GlfwWindow* window, DropCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwJoystickPresent", CallingConvention = Convention)]
		public static extern bool JoystickPresent(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickAxes", CallingConvention = Convention)]
		public static extern float* GetJoystickAxes(GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickButtons", CallingConvention = Convention)]
		public static extern GlfwInputState* GetJoystickButtons(GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickHats", CallingConvention = Convention)]
		public static extern GlfwInput* GetJoystickHats(GlfwInput jid, out int count);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickName", CallingConvention = Convention)]
		public static extern char* GetJoystickName(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickGUID", CallingConvention = Convention)]
		public static extern char* GetJoystickGUID(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = Convention)]
		public static extern void SetJoystickUserPointer(GlfwInput jid, void* pointer);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = Convention)]
		public static extern void* GetJoystickUserPointer(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = Convention)]
		public static extern bool JoystickIsGamepad(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetJoystickCallback", CallingConvention = Convention)]
		public static extern JoystickCallback SetJoystickCallback(JoystickCallback callback);

		[DllImport(GlfwLibrary, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = Convention, BestFitMapping = false)]
		public static extern bool UpdateGamepadMappings([MarshalAs(UnmanagedType.LPUTF8Str)] string value);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGamepadName", CallingConvention = Convention)]
		public static extern char* GetGamepadName(GlfwInput jid);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetGamepadState", CallingConvention = Convention)]
		public static extern bool GetGamepadState(GlfwInput jid, GlfwGamepadState* state);

		[DllImport(GlfwLibrary, EntryPoint = "glfwSetClipboardString", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void SetClipboardString(GlfwWindow* window, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

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
		public static extern bool ExtensionSupported([MarshalAs(UnmanagedType.LPUTF8Str)] string extension);

		[DllImport(GlfwLibrary, EntryPoint = "glfwGetProcAddress", CallingConvention = Convention, BestFitMapping = false)]
		public static extern void* GetProcessAddress([MarshalAs(UnmanagedType.LPUTF8Str)] string processName);
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
