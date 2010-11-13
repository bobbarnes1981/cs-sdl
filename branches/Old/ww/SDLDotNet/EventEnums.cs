using System;

namespace SDLDotNet {
	/// <summary>
	/// Indicates which key on the keyboard the user pressed or released
	/// </summary>
	/// <type>enum</type>
	public enum Key {
		/// <summary></summary>
		K_UNKNOWN	= 0,
		/// <summary></summary>
		K_FIRST		= 0,
		/// <summary></summary>
		K_BACKSPACE	= 8,
		/// <summary></summary>
		K_TAB		= 9,
		/// <summary></summary>
		K_CLEAR		= 12,
		/// <summary></summary>
		K_RETURN	= 13,
		/// <summary></summary>
		K_PAUSE		= 19,
		/// <summary></summary>
		K_ESCAPE	= 27,
		/// <summary></summary>
		K_SPACE		= 32,
		/// <summary></summary>
		K_EXCLAIM	= 33,
		/// <summary></summary>
		K_QUOTEDBL	= 34,
		/// <summary></summary>
		K_HASH		= 35,
		/// <summary></summary>
		K_DOLLAR	= 36,
		/// <summary></summary>
		K_AMPERSAND	= 38,
		/// <summary></summary>
		K_QUOTE		= 39,
		/// <summary></summary>
		K_LEFTPAREN	= 40,
		/// <summary></summary>
		K_RIGHTPAREN= 41,
		/// <summary></summary>
		K_ASTERISK	= 42,
		/// <summary></summary>
		K_PLUS		= 43,
		/// <summary></summary>
		K_COMMA		= 44,
		/// <summary></summary>
		K_MINUS		= 45,
		/// <summary></summary>
		K_PERIOD	= 46,
		/// <summary></summary>
		K_SLASH		= 47,
		/// <summary></summary>
		K_0			= 48,
		/// <summary></summary>
		K_1			= 49,
		/// <summary></summary>
		K_2			= 50,
		/// <summary></summary>
		K_3			= 51,
		/// <summary></summary>
		K_4			= 52,
		/// <summary></summary>
		K_5			= 53,
		/// <summary></summary>
		K_6			= 54,
		/// <summary></summary>
		K_7			= 55,
		/// <summary></summary>
		K_8			= 56,
		/// <summary></summary>
		K_9			= 57,
		/// <summary></summary>
		K_COLON		= 58,
		/// <summary></summary>
		K_SEMICOLON	= 59,
		/// <summary></summary>
		K_LESS		= 60,
		/// <summary></summary>
		K_EQUALS	= 61,
		/// <summary></summary>
		K_GREATER	= 62,
		/// <summary></summary>
		K_QUESTION	= 63,
		/// <summary></summary>
		K_AT		= 64,
		/// <summary></summary>
		K_LEFTBRACKET	= 91,
		/// <summary></summary>
		K_BACKSLASH		= 92,
		/// <summary></summary>
		K_RIGHTBRACKET	= 93,
		/// <summary></summary>
		K_CARET		= 94,
		/// <summary></summary>
		K_UNDERSCORE= 95,
		/// <summary></summary>
		K_BACKQUOTE	= 96,
		/// <summary></summary>
		K_a			= 97,
		/// <summary></summary>
		K_b			= 98,
		/// <summary></summary>
		K_c			= 99,
		/// <summary></summary>
		K_d			= 100,
		/// <summary></summary>
		K_e			= 101,
		/// <summary></summary>
		K_f			= 102,
		/// <summary></summary>
		K_g			= 103,
		/// <summary></summary>
		K_h			= 104,
		/// <summary></summary>
		K_i			= 105,
		/// <summary></summary>
		K_j			= 106,
		/// <summary></summary>
		K_k			= 107,
		/// <summary></summary>
		K_l			= 108,
		/// <summary></summary>
		K_m			= 109,
		/// <summary></summary>
		K_n			= 110,
		/// <summary></summary>
		K_o			= 111,
		/// <summary></summary>
		K_p			= 112,
		/// <summary></summary>
		K_q			= 113,
		/// <summary></summary>
		K_r			= 114,
		/// <summary></summary>
		K_s			= 115,
		/// <summary></summary>
		K_t			= 116,
		/// <summary></summary>
		K_u			= 117,
		/// <summary></summary>
		K_v			= 118,
		/// <summary></summary>
		K_w			= 119,
		/// <summary></summary>
		K_x			= 120,
		/// <summary></summary>
		K_y			= 121,
		/// <summary></summary>
		K_z			= 122,
		/// <summary></summary>
		K_DELETE	= 127,
		/// <summary></summary>
		K_WORLD_0	= 160,
		/// <summary></summary>
		K_WORLD_1	= 161,
		/// <summary></summary>
		K_WORLD_2	= 162,
		/// <summary></summary>
		K_WORLD_3	= 163,
		/// <summary></summary>
		K_WORLD_4	= 164,
		/// <summary></summary>
		K_WORLD_5	= 165,
		/// <summary></summary>
		K_WORLD_6	= 166,
		/// <summary></summary>
		K_WORLD_7	= 167,
		/// <summary></summary>
		K_WORLD_8	= 168,
		/// <summary></summary>
		K_WORLD_9	= 169,
		/// <summary></summary>
		K_WORLD_10	= 170,
		/// <summary></summary>
		K_WORLD_11	= 171,
		/// <summary></summary>
		K_WORLD_12	= 172,
		/// <summary></summary>
		K_WORLD_13	= 173,
		/// <summary></summary>
		K_WORLD_14	= 174,
		/// <summary></summary>
		K_WORLD_15	= 175,
		/// <summary></summary>
		K_WORLD_16	= 176,
		/// <summary></summary>
		K_WORLD_17	= 177,
		/// <summary></summary>
		K_WORLD_18	= 178,
		/// <summary></summary>
		K_WORLD_19	= 179,
		/// <summary></summary>
		K_WORLD_20	= 180,
		/// <summary></summary>
		K_WORLD_21	= 181,
		/// <summary></summary>
		K_WORLD_22	= 182,
		/// <summary></summary>
		K_WORLD_23	= 183,
		/// <summary></summary>
		K_WORLD_24	= 184,
		/// <summary></summary>
		K_WORLD_25	= 185,
		/// <summary></summary>
		K_WORLD_26	= 186,
		/// <summary></summary>
		K_WORLD_27	= 187,
		/// <summary></summary>
		K_WORLD_28	= 188,
		/// <summary></summary>
		K_WORLD_29	= 189,
		/// <summary></summary>
		K_WORLD_30	= 190,
		/// <summary></summary>
		K_WORLD_31	= 191,
		/// <summary></summary>
		K_WORLD_32	= 192,
		/// <summary></summary>
		K_WORLD_33	= 193,
		/// <summary></summary>
		K_WORLD_34	= 194,
		/// <summary></summary>
		K_WORLD_35	= 195,
		/// <summary></summary>
		K_WORLD_36	= 196,
		/// <summary></summary>
		K_WORLD_37	= 197,
		/// <summary></summary>
		K_WORLD_38	= 198,
		/// <summary></summary>
		K_WORLD_39	= 199,
		/// <summary></summary>
		K_WORLD_40	= 200,
		/// <summary></summary>
		K_WORLD_41	= 201,
		/// <summary></summary>
		K_WORLD_42	= 202,
		/// <summary></summary>
		K_WORLD_43	= 203,
		/// <summary></summary>
		K_WORLD_44	= 204,
		/// <summary></summary>
		K_WORLD_45	= 205,
		/// <summary></summary>
		K_WORLD_46	= 206,
		/// <summary></summary>
		K_WORLD_47	= 207,
		/// <summary></summary>
		K_WORLD_48	= 208,
		/// <summary></summary>
		K_WORLD_49	= 209,
		/// <summary></summary>
		K_WORLD_50	= 210,
		/// <summary></summary>
		K_WORLD_51	= 211,
		/// <summary></summary>
		K_WORLD_52	= 212,
		/// <summary></summary>
		K_WORLD_53	= 213,
		/// <summary></summary>
		K_WORLD_54	= 214,
		/// <summary></summary>
		K_WORLD_55	= 215,
		/// <summary></summary>
		K_WORLD_56	= 216,
		/// <summary></summary>
		K_WORLD_57	= 217,
		/// <summary></summary>
		K_WORLD_58	= 218,
		/// <summary></summary>
		K_WORLD_59	= 219,
		/// <summary></summary>
		K_WORLD_60	= 220,
		/// <summary></summary>
		K_WORLD_61	= 221,
		/// <summary></summary>
		K_WORLD_62	= 222,
		/// <summary></summary>
		K_WORLD_63	= 223,
		/// <summary></summary>
		K_WORLD_64	= 224,
		/// <summary></summary>
		K_WORLD_65	= 225,
		/// <summary></summary>
		K_WORLD_66	= 226,
		/// <summary></summary>
		K_WORLD_67	= 227,
		/// <summary></summary>
		K_WORLD_68	= 228,
		/// <summary></summary>
		K_WORLD_69	= 229,
		/// <summary></summary>
		K_WORLD_70	= 230,
		/// <summary></summary>
		K_WORLD_71	= 231,
		/// <summary></summary>
		K_WORLD_72	= 232,
		/// <summary></summary>
		K_WORLD_73	= 233,
		/// <summary></summary>
		K_WORLD_74	= 234,
		/// <summary></summary>
		K_WORLD_75	= 235,
		/// <summary></summary>
		K_WORLD_76	= 236,
		/// <summary></summary>
		K_WORLD_77	= 237,
		/// <summary></summary>
		K_WORLD_78	= 238,
		/// <summary></summary>
		K_WORLD_79	= 239,
		/// <summary></summary>
		K_WORLD_80	= 240,
		/// <summary></summary>
		K_WORLD_81	= 241,
		/// <summary></summary>
		K_WORLD_82	= 242,
		/// <summary></summary>
		K_WORLD_83	= 243,
		/// <summary></summary>
		K_WORLD_84	= 244,
		/// <summary></summary>
		K_WORLD_85	= 245,
		/// <summary></summary>
		K_WORLD_86	= 246,
		/// <summary></summary>
		K_WORLD_87	= 247,
		/// <summary></summary>
		K_WORLD_88	= 248,
		/// <summary></summary>
		K_WORLD_89	= 249,
		/// <summary></summary>
		K_WORLD_90	= 250,
		/// <summary></summary>
		K_WORLD_91	= 251,
		/// <summary></summary>
		K_WORLD_92	= 252,
		/// <summary></summary>
		K_WORLD_93	= 253,
		/// <summary></summary>
		K_WORLD_94	= 254,
		/// <summary></summary>
		K_WORLD_95	= 255,
		/// <summary></summary>
		K_KP0		= 256,
		/// <summary></summary>
		K_KP1		= 257,
		/// <summary></summary>
		K_KP2		= 258,
		/// <summary></summary>
		K_KP3		= 259,
		/// <summary></summary>
		K_KP4		= 260,
		/// <summary></summary>
		K_KP5		= 261,
		/// <summary></summary>
		K_KP6		= 262,
		/// <summary></summary>
		K_KP7		= 263,
		/// <summary></summary>
		K_KP8		= 264,
		/// <summary></summary>
		K_KP9		= 265,
		/// <summary></summary>
		K_KP_PERIOD	= 266,
		/// <summary></summary>
		K_KP_DIVIDE	= 267,
		/// <summary></summary>
		K_KP_MULTIPLY	= 268,
		/// <summary></summary>
		K_KP_MINUS	= 269,
		/// <summary></summary>
		K_KP_PLUS	= 270,
		/// <summary></summary>
		K_KP_ENTER	= 271,
		/// <summary></summary>
		K_KP_EQUALS	= 272,
		/// <summary></summary>
		K_UP		= 273,
		/// <summary></summary>
		K_DOWN		= 274,
		/// <summary></summary>
		K_RIGHT		= 275,
		/// <summary></summary>
		K_LEFT		= 276,
		/// <summary></summary>
		K_INSERT	= 277,
		/// <summary></summary>
		K_HOME		= 278,
		/// <summary></summary>
		K_END		= 279,
		/// <summary></summary>
		K_PAGEUP	= 280,
		/// <summary></summary>
		K_PAGEDOWN	= 281,
		/// <summary></summary>
		K_F1		= 282,
		/// <summary></summary>
		K_F2		= 283,
		/// <summary></summary>
		K_F3		= 284,
		/// <summary></summary>
		K_F4		= 285,
		/// <summary></summary>
		K_F5		= 286,
		/// <summary></summary>
		K_F6		= 287,
		/// <summary></summary>
		K_F7		= 288,
		/// <summary></summary>
		K_F8		= 289,
		/// <summary></summary>
		K_F9		= 290,
		/// <summary></summary>
		K_F10		= 291,
		/// <summary></summary>
		K_F11		= 292,
		/// <summary></summary>
		K_F12		= 293,
		/// <summary></summary>
		K_F13		= 294,
		/// <summary></summary>
		K_F14		= 295,
		/// <summary></summary>
		K_F15		= 296,
		/// <summary></summary>
		K_NUMLOCK	= 300,
		/// <summary></summary>
		K_CAPSLOCK	= 301,
		/// <summary></summary>
		K_SCROLLOCK	= 302,
		/// <summary></summary>
		K_RSHIFT	= 303,
		/// <summary></summary>
		K_LSHIFT	= 304,
		/// <summary></summary>
		K_RCTRL		= 305,
		/// <summary></summary>
		K_LCTRL		= 306,
		/// <summary></summary>
		K_RALT		= 307,
		/// <summary></summary>
		K_LALT		= 308,
		/// <summary></summary>
		K_RMETA		= 309,
		/// <summary></summary>
		K_LMETA		= 310,
		/// <summary></summary>
		K_LSUPER	= 311,
		/// <summary></summary>
		K_RSUPER	= 312,
		/// <summary></summary>
		K_MODE		= 313,
		/// <summary></summary>
		K_COMPOSE	= 314,
		/// <summary></summary>
		K_HELP		= 315,
		/// <summary></summary>
		K_PRINT		= 316,
		/// <summary></summary>
		K_SYSREQ	= 317,
		/// <summary></summary>
		K_BREAK		= 318,
		/// <summary></summary>
		K_MENU		= 319,
		/// <summary></summary>
		K_POWER		= 320,
		/// <summary></summary>
		K_EURO		= 321,
		/// <summary></summary>
		K_UNDO		= 322
	}
	/// <summary>
	/// Indicates which keyboard modifiers were pressed.
	/// Can be Or'd together
	/// </summary>
	/// <type>enum</type>
	public enum Mod {
		/// <summary>
		/// No modifiers
		/// </summary>
		None  = 0x0000,
		/// <summary>
		/// Left shift
		/// </summary>
		LShift= 0x0001,
		/// <summary>
		/// Right shift
		/// </summary>
		RShift= 0x0002,
		/// <summary>
		/// Left Ctrl
		/// </summary>
		LCtrl = 0x0040,
		/// <summary>
		/// Right Ctrl
		/// </summary>
		RCtrl = 0x0080,
		/// <summary>
		/// Left Alt
		/// </summary>
		LAlt  = 0x0100,
		/// <summary>
		/// Right Alt
		/// </summary>
		RAlt  = 0x0200,
		/// <summary>
		/// Left Meta
		/// </summary>
		LMeta = 0x0400,
		/// <summary>
		/// Right Meta
		/// </summary>
		RMeta = 0x0800,
		/// <summary>
		/// NumLock
		/// </summary>
		Num   = 0x1000,
		/// <summary>
		/// CapsLock
		/// </summary>
		Caps  = 0x2000,
		/// <summary>
		/// Mode
		/// </summary>
		Mode  = 0x4000
	}
	/// <summary>
	/// Indicates which mouse button the user pressed or released
	/// </summary>
	/// <type>enum</type>
	public enum MouseButton {
		/// <summary>
		/// The left mouse button
		/// </summary>
		Left = 1,
		/// <summary>
		/// The middle mouse button
		/// </summary>
		Middle = 2,
		/// <summary>
		/// The right mouse button
		/// </summary>
		Right = 3,
		/// <summary>
		/// Mouse wheel up
		/// </summary>
		WheelUp = 4,
		/// <summary>
		/// Mouse wheel down
		/// </summary>
		WheelDown = 5
	}

	/// <summary>
	/// Indicates which position a joystick hat is pressed in
	/// </summary>
	/// <type>enum</type>
	public enum HatPos {
		/// <summary></summary>
		Center = 0,
		/// <summary></summary>
		Up = 1,
		/// <summary></summary>
		UpRight = 3,
		/// <summary></summary>
		Right = 2,
		/// <summary></summary>
		DownRight = 6,
		/// <summary></summary>
		Down = 4,
		/// <summary></summary>
		DownLeft = 12,
		/// <summary></summary>
		Left = 8,
		/// <summary></summary>
		UpLeft = 9
	}
}
