using System;

namespace System
{
	/// <summary>Specifies the standard keys on a console.</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public enum ConsoleKey
	{
		/// <summary>The BACKSPACE key.</summary>
		Backspace = 8,
		/// <summary>The TAB key.</summary>
		Tab,
		/// <summary>The CLEAR key.</summary>
		Clear = 12,
		/// <summary>The ENTER key.</summary>
		Enter,
		/// <summary>The PAUSE key.</summary>
		Pause = 19,
		/// <summary>The ESC (ESCAPE) key.</summary>
		Escape = 27,
		/// <summary>The SPACEBAR key.</summary>
		Spacebar = 32,
		/// <summary>The PAGE UP key.</summary>
		PageUp,
		/// <summary>The PAGE DOWN key.</summary>
		PageDown,
		/// <summary>The END key.</summary>
		End,
		/// <summary>The HOME key.</summary>
		Home,
		/// <summary>The LEFT ARROW key.</summary>
		LeftArrow,
		/// <summary>The UP ARROW key.</summary>
		UpArrow,
		/// <summary>The RIGHT ARROW key.</summary>
		RightArrow,
		/// <summary>The DOWN ARROW key.</summary>
		DownArrow,
		/// <summary>The SELECT key.</summary>
		Select,
		/// <summary>The PRINT key.</summary>
		Print,
		/// <summary>The EXECUTE key.</summary>
		Execute,
		/// <summary>The PRINT SCREEN key.</summary>
		PrintScreen,
		/// <summary>The INS (INSERT) key.</summary>
		Insert,
		/// <summary>The DEL (DELETE) key.</summary>
		Delete,
		/// <summary>The HELP key.</summary>
		Help,
		/// <summary>The 0 key.</summary>
		D0,
		/// <summary>The 1 key.</summary>
		D1,
		/// <summary>The 2 key.</summary>
		D2,
		/// <summary>The 3 key.</summary>
		D3,
		/// <summary>The 4 key.</summary>
		D4,
		/// <summary>The 5 key.</summary>
		D5,
		/// <summary>The 6 key.</summary>
		D6,
		/// <summary>The 7 key.</summary>
		D7,
		/// <summary>The 8 key.</summary>
		D8,
		/// <summary>The 9 key.</summary>
		D9,
		/// <summary>The A key.</summary>
		A = 65,
		/// <summary>The B key.</summary>
		B,
		/// <summary>The C key.</summary>
		C,
		/// <summary>The D key.</summary>
		D,
		/// <summary>The E key.</summary>
		E,
		/// <summary>The F key.</summary>
		F,
		/// <summary>The G key.</summary>
		G,
		/// <summary>The H key.</summary>
		H,
		/// <summary>The I key.</summary>
		I,
		/// <summary>The J key.</summary>
		J,
		/// <summary>The K key.</summary>
		K,
		/// <summary>The L key.</summary>
		L,
		/// <summary>The M key.</summary>
		M,
		/// <summary>The N key.</summary>
		N,
		/// <summary>The O key.</summary>
		O,
		/// <summary>The P key.</summary>
		P,
		/// <summary>The Q key.</summary>
		Q,
		/// <summary>The R key.</summary>
		R,
		/// <summary>The S key.</summary>
		S,
		/// <summary>The T key.</summary>
		T,
		/// <summary>The U key.</summary>
		U,
		/// <summary>The V key.</summary>
		V,
		/// <summary>The W key.</summary>
		W,
		/// <summary>The X key.</summary>
		X,
		/// <summary>The Y key.</summary>
		Y,
		/// <summary>The Z key.</summary>
		Z,
		/// <summary>The left Windows logo key (Microsoft Natural Keyboard).</summary>
		LeftWindows,
		/// <summary>The right Windows logo key (Microsoft Natural Keyboard).</summary>
		RightWindows,
		/// <summary>The Application key (Microsoft Natural Keyboard).</summary>
		Applications,
		/// <summary>The Computer Sleep key.</summary>
		Sleep = 95,
		/// <summary>The 0 key on the numeric keypad.</summary>
		NumPad0,
		/// <summary>The 1 key on the numeric keypad.</summary>
		NumPad1,
		/// <summary>The 2 key on the numeric keypad.</summary>
		NumPad2,
		/// <summary>The 3 key on the numeric keypad.</summary>
		NumPad3,
		/// <summary>The 4 key on the numeric keypad.</summary>
		NumPad4,
		/// <summary>The 5 key on the numeric keypad.</summary>
		NumPad5,
		/// <summary>The 6 key on the numeric keypad.</summary>
		NumPad6,
		/// <summary>The 7 key on the numeric keypad.</summary>
		NumPad7,
		/// <summary>The 8 key on the numeric keypad.</summary>
		NumPad8,
		/// <summary>The 9 key on the numeric keypad.</summary>
		NumPad9,
		/// <summary>The Multiply key.</summary>
		Multiply,
		/// <summary>The Add key.</summary>
		Add,
		/// <summary>The Separator key.</summary>
		Separator,
		/// <summary>The Subtract key.</summary>
		Subtract,
		/// <summary>The Decimal key.</summary>
		Decimal,
		/// <summary>The Divide key.</summary>
		Divide,
		/// <summary>The F1 key.</summary>
		F1,
		/// <summary>The F2 key.</summary>
		F2,
		/// <summary>The F3 key.</summary>
		F3,
		/// <summary>The F4 key.</summary>
		F4,
		/// <summary>The F5 key.</summary>
		F5,
		/// <summary>The F6 key.</summary>
		F6,
		/// <summary>The F7 key.</summary>
		F7,
		/// <summary>The F8 key.</summary>
		F8,
		/// <summary>The F9 key.</summary>
		F9,
		/// <summary>The F10 key.</summary>
		F10,
		/// <summary>The F11 key.</summary>
		F11,
		/// <summary>The F12 key.</summary>
		F12,
		/// <summary>The F13 key.</summary>
		F13,
		/// <summary>The F14 key.</summary>
		F14,
		/// <summary>The F15 key.</summary>
		F15,
		/// <summary>The F16 key.</summary>
		F16,
		/// <summary>The F17 key.</summary>
		F17,
		/// <summary>The F18 key.</summary>
		F18,
		/// <summary>The F19 key.</summary>
		F19,
		/// <summary>The F20 key.</summary>
		F20,
		/// <summary>The F21 key.</summary>
		F21,
		/// <summary>The F22 key.</summary>
		F22,
		/// <summary>The F23 key.</summary>
		F23,
		/// <summary>The F24 key.</summary>
		F24,
		/// <summary>The Browser Back key (Windows 2000 or later).</summary>
		BrowserBack = 166,
		/// <summary>The Browser Forward key (Windows 2000 or later).</summary>
		BrowserForward,
		/// <summary>The Browser Refresh key (Windows 2000 or later).</summary>
		BrowserRefresh,
		/// <summary>The Browser Stop key (Windows 2000 or later).</summary>
		BrowserStop,
		/// <summary>The Browser Search key (Windows 2000 or later).</summary>
		BrowserSearch,
		/// <summary>The Browser Favorites key (Windows 2000 or later).</summary>
		BrowserFavorites,
		/// <summary>The Browser Home key (Windows 2000 or later).</summary>
		BrowserHome,
		/// <summary>The Volume Mute key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		VolumeMute,
		/// <summary>The Volume Down key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		VolumeDown,
		/// <summary>The Volume Up key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		VolumeUp,
		/// <summary>The Media Next Track key (Windows 2000 or later).</summary>
		MediaNext,
		/// <summary>The Media Previous Track key (Windows 2000 or later).</summary>
		MediaPrevious,
		/// <summary>The Media Stop key (Windows 2000 or later).</summary>
		MediaStop,
		/// <summary>The Media Play/Pause key (Windows 2000 or later).</summary>
		MediaPlay,
		/// <summary>The Start Mail key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		LaunchMail,
		/// <summary>The Select Media key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		LaunchMediaSelect,
		/// <summary>The Start Application 1 key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		LaunchApp1,
		/// <summary>The Start Application 2 key (Microsoft Natural Keyboard, Windows 2000 or later).</summary>
		LaunchApp2,
		/// <summary>The OEM 1 key (OEM specific).</summary>
		Oem1 = 186,
		/// <summary>The OEM Plus key on any country/region keyboard (Windows 2000 or later).</summary>
		OemPlus,
		/// <summary>The OEM Comma key on any country/region keyboard (Windows 2000 or later).</summary>
		OemComma,
		/// <summary>The OEM Minus key on any country/region keyboard (Windows 2000 or later).</summary>
		OemMinus,
		/// <summary>The OEM Period key on any country/region keyboard (Windows 2000 or later).</summary>
		OemPeriod,
		/// <summary>The OEM 2 key (OEM specific).</summary>
		Oem2,
		/// <summary>The OEM 3 key (OEM specific).</summary>
		Oem3,
		/// <summary>The OEM 4 key (OEM specific).</summary>
		Oem4 = 219,
		/// <summary>The OEM 5 (OEM specific).</summary>
		Oem5,
		/// <summary>The OEM 6 key (OEM specific).</summary>
		Oem6,
		/// <summary>The OEM 7 key (OEM specific).</summary>
		Oem7,
		/// <summary>The OEM 8 key (OEM specific).</summary>
		Oem8,
		/// <summary>The OEM 102 key (OEM specific).</summary>
		Oem102 = 226,
		/// <summary>The IME PROCESS key.</summary>
		Process = 229,
		/// <summary>The PACKET key (used to pass Unicode characters with keystrokes).</summary>
		Packet = 231,
		/// <summary>The ATTN key.</summary>
		Attention = 246,
		/// <summary>The CRSEL (CURSOR SELECT) key.</summary>
		CrSel,
		/// <summary>The EXSEL (EXTEND SELECTION) key.</summary>
		ExSel,
		/// <summary>The ERASE EOF key.</summary>
		EraseEndOfFile,
		/// <summary>The PLAY key.</summary>
		Play,
		/// <summary>The ZOOM key.</summary>
		Zoom,
		/// <summary>A constant reserved for future use.</summary>
		NoName,
		/// <summary>The PA1 key.</summary>
		Pa1,
		/// <summary>The CLEAR key (OEM specific).</summary>
		OemClear
	}
}
