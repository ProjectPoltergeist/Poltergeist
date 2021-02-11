using Mono.Unix;
using Mono.Unix.Native;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Poltergeist.Core
{
	public static class ExitHandler
	{
		public static event Action Exit;

		private static readonly object ExitLock = new();
		private static readonly HandlerRoutine Routine = OnNativeSignal;

		private static Process _currentProcess;
		private static bool _called;

		static ExitHandler()
		{
			_currentProcess = Process.GetCurrentProcess();
			_currentProcess.EnableRaisingEvents = true;
			_currentProcess.Exited += (_, _) => CallExit();
			AppDomain.CurrentDomain.ProcessExit += (_, _) => CallExit();
			AppDomain.CurrentDomain.DomainUnload += (_, _) => CallExit();

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				SetConsoleCtrlHandler(Routine, true);
			}
			else
			{
				new Thread(() =>
				{
					UnixSignal.WaitAny(new UnixSignal[]{
						new(Signum.SIGINT),
						new(Signum.SIGTERM),
						new(Signum.SIGKILL),
						new(Signum.SIGUSR1),
						new(Signum.SIGUSR2),
						new(Signum.SIGHUP)
					}, -1);
					CallExit();
				})
				{ IsBackground = true }.Start();
			}
		}

		private static void CallExit()
		{
			lock (ExitLock)
			{
				if (_called)
					return;

				_called = true;
				_currentProcess?.Dispose();
				_currentProcess = null;
				Exit?.Invoke();
			}
		}

		public static void ExitProcess(int code = 0)
		{
			lock (ExitLock)
			{
				CallExit();
				Environment.Exit(code);
			}
		}

		private static bool OnNativeSignal(CtrlTypes ctrl)
		{
			CallExit();
			return true;
		}

		[DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
		private static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private delegate bool HandlerRoutine(CtrlTypes ctrlType);

		private enum CtrlTypes
		{
			// ReSharper disable UnusedMember.Local
			CtrlCEvent,
			CtrlBreakEvent,
			CtrlCloseEvent,
			CtrlLogoffEvent,
			CtrlShutdownEvent
			// ReSharper restore UnusedMember.Local
		}

		public sealed class LifetimeHandle : IDisposable
		{
			public void Dispose()
			{
				CallExit();
				GC.SuppressFinalize(this);
			}

			~LifetimeHandle()
			{
				CallExit();
			}
		}
	}
}
