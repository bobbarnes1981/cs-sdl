/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Runtime.InteropServices;

using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Enum for values that are returned by the SDL C library functions.
	/// This reduces the amount of "magic numbers" in the code.
	/// </summary>
	public enum SdlFlag
	{
		/// <summary>
		/// 
		/// </summary>
		Error = -1,
		/// <summary>
		/// 
		/// </summary>
		Success = 0,
		/// <summary>
		/// 
		/// </summary>
		InfiniteLoops = -1,
		/// <summary>
		/// 
		/// </summary>
		PlayForever = -1,
		/// <summary>
		/// 
		/// </summary>
		Error2 = 1,
		/// <summary>
		/// 
		/// </summary>
		Success2 = 1,
		/// <summary>
		/// 
		/// </summary>
		None = 0,
		/// <summary>
		/// 
		/// </summary>
		FirstFreeChannel = -1,
		/// <summary>
		/// 
		/// </summary>
		TrueValue = 1,
		/// <summary>
		/// 
		/// </summary>
		FalseValue = 0,
		/// <summary>
		/// 
		/// </summary>
		LoopMusic = -1
	}

	#region CDstatus
	/// <summary>
	/// The possible states which a CD-ROM drive can be in.
	/// </summary>
	public enum CDStatus 
	{
		/// <summary>
		/// The CD tray is empty.
		/// </summary>
		TrayEmpty = Sdl.CD_TRAYEMPTY,
		/// <summary>
		/// The CD has stopped playing.
		/// </summary>
		Stopped = Sdl.CD_STOPPED,
		/// <summary>
		/// The CD is playing.
		/// </summary>
		Playing = Sdl.CD_PLAYING,
		/// <summary>
		/// The CD has been paused.
		/// </summary>
		Paused = Sdl.CD_PAUSED,
		/// <summary>
		/// An error occured while getting the status.
		/// </summary>
		Error = Sdl.CD_ERROR
	}
	#endregion CDstatus

	/// <summary>
	/// 
	/// </summary>
	[FlagsAttribute]
	public enum Alphas
	{
		/// <summary>
		/// Equivalent to SDL_RLEACC
		/// </summary>
		RleEncoded = Sdl.SDL_RLEACCEL,

		/// <summary>
		/// Equivalent to SDL_SRCALPHA
		/// </summary>
		SourceAlphaBlending  = Sdl.SDL_SRCALPHA
	}

	/// <summary>
	/// 
	/// </summary>
	public enum MovieStatus
	{
		/// <summary>
		/// 
		/// </summary>
		Playing = Smpeg.SMPEG_PLAYING,
		/// <summary>
		/// 
		/// </summary>
		Stopped = Smpeg.SMPEG_STOPPED,
		/// <summary>
		/// 
		/// </summary>
		Error = Smpeg.SMPEG_ERROR
	}

	#region OpenGLAttribute
	/// <summary>
	/// Public enumeration for setting the OpenGL window Attributes
	/// </summary>
	/// <remarks>
	/// While you can set most OpenGL attributes normally, 
	/// the attributes list above must be known before SDL 
	/// sets the video mode.
	/// </remarks>
	public enum OpenGLAttr
	{
		/// <summary>
		/// Size of the framebuffer red component, in bits
		/// </summary>
		RedSize = Sdl.SDL_GL_RED_SIZE,
		/// <summary>
		/// Size of the framebuffer green component, in bits
		/// </summary>
		GreenSize = Sdl.SDL_GL_GREEN_SIZE,
		/// <summary>
		/// Size of the framebuffer blue component, in bits
		/// </summary>
		BlueSize = Sdl.SDL_GL_BLUE_SIZE,
		/// <summary>
		/// Size of the framebuffer alpha component, in bits
		/// </summary>
		AlphaSize = Sdl.SDL_GL_ALPHA_SIZE,
		/// <summary>
		/// Size of the framebuffer, in bits
		/// </summary>
		BufferSize = Sdl.SDL_GL_BUFFER_SIZE,
		/// <summary>
		/// 0 or 1, enable or disable double buffering
		/// </summary>
		DoubleBuffer = Sdl.SDL_GL_DOUBLEBUFFER,
		/// <summary>
		/// Size of the depth buffer, in bits
		/// </summary>
		DepthSize = Sdl.SDL_GL_DEPTH_SIZE,
		/// <summary>
		/// Size of the stencil buffer, in bits.
		/// </summary>
		StencilSize = Sdl.SDL_GL_STENCIL_SIZE,
		/// <summary>
		/// Size of the accumulation buffer red component, in bits.
		/// </summary>
		AccumulationRedSize = Sdl.SDL_GL_ACCUM_RED_SIZE,
		/// <summary>
		/// Size of the accumulation buffer green component, in bits.
		/// </summary>
		AccumulationGreenSize = Sdl.SDL_GL_ACCUM_GREEN_SIZE,
		/// <summary>
		/// Size of the accumulation buffer blue component, in bits.
		/// </summary>
		AccumulationBlueSize = Sdl.SDL_GL_ACCUM_BLUE_SIZE,
		/// <summary>
		/// Size of the accumulation buffer alpha component, in bits.
		/// </summary>
		AccumulationAlphaSize = Sdl.SDL_GL_ACCUM_ALPHA_SIZE,
		/// <summary>
		/// 
		/// </summary>
		Stereo = Sdl.SDL_GL_STEREO,
		/// <summary>
		/// 
		/// </summary>
		MultiSampleBuffers = Sdl.SDL_GL_MULTISAMPLEBUFFERS,
		/// <summary>
		/// 
		/// </summary>
		MultiSampleSamples = Sdl.SDL_GL_MULTISAMPLESAMPLES
	}
	#endregion OpenGLAttribute

	/// <summary>
	/// Specifies an audio format to mix audio in
	/// </summary>
	public enum AudioFormat
	{
		/// <summary>
		/// Unsigned 8-bit
		/// </summary>
		Unsigned8 = Sdl.AUDIO_U8,
		/// <summary>
		/// Signed 8-bit
		/// </summary>
		Signed8 = Sdl.AUDIO_S8,
		/// <summary>
		/// Unsigned 16-bit, little-endian
		/// </summary>
		Unsigned16Little = Sdl.AUDIO_U16LSB,
		/// <summary>
		/// Signed 16-bit, little-endian
		/// </summary>
		Signed16Little = Sdl.AUDIO_S16LSB,
		/// <summary>
		/// Unsigned 16-bit, big-endian
		/// </summary>
		Unsigned16Big = Sdl.AUDIO_U16MSB,
		/// <summary>
		/// Signed 16-bit, big-endian
		/// </summary>
		Signed16Big = Sdl.AUDIO_S16MSB,
		/// <summary>
		/// Default, equal to Signed16Little
		/// </summary>
		Default = Sdl.AUDIO_S16LSB
	}

	/// <summary>
	/// Indicates the current fading status of a sound
	/// </summary>
	public enum FadingStatus
	{
		/// <summary>
		/// Sound is not fading
		/// </summary>
		NoFading = SdlMixer.MIX_NO_FADING,
		/// <summary>
		/// Sound is fading out
		/// </summary>
		FadingOut = SdlMixer.MIX_FADING_OUT,
		/// <summary>
		/// Sound is fading in
		/// </summary>
		FadingIn = SdlMixer.MIX_FADING_IN
	}

	/// <summary>
	/// Text Style
	/// </summary>
	[FlagsAttribute]
	public enum Styles
	{
		/// <summary>
		/// Normal
		/// </summary>
		Normal = SdlTtf.TTF_STYLE_NORMAL,
		/// <summary>
		/// Bold
		/// </summary>
		Bold = SdlTtf.TTF_STYLE_BOLD,
		/// <summary>
		/// Italic
		/// </summary>
		Italic = SdlTtf.TTF_STYLE_ITALIC,
		/// <summary>
		/// Underline
		/// </summary>
		Underline = SdlTtf.TTF_STYLE_UNDERLINE
	}

	#region Keys
	/// <summary>
	/// What we really want is a mapping of every raw key on the keyboard.
	/// To support international keyboards, we use the range 0xA1 - 0xFF
	/// as international virtual keycodes.  
	/// We'll follow in the footsteps of X11...
	/// The keyboard syms have been cleverly chosen to map to ASCII
	/// </summary>
	public enum Key
	{
		/// <summary>
		///
		/// </summary>
		Unknown = Sdl.SDLK_UNKNOWN ,
		/// <summary>
		///
		/// </summary>
		First = Sdl.SDLK_FIRST ,
		/// <summary>
		/// backspace. '\b'
		/// </summary>
		Backspace = Sdl.SDLK_BACKSPACE ,
		/// <summary>
		/// tab. '\t'
		/// </summary>
		Tab = Sdl.SDLK_TAB ,
		/// <summary>
		/// clear
		/// </summary>
		Clear = Sdl.SDLK_CLEAR ,
		/// <summary>
		/// return. '\r'
		/// </summary>
		Return = Sdl.SDLK_RETURN ,
		/// <summary>
		/// pause
		/// </summary>
		Pause = Sdl.SDLK_PAUSE ,
		/// <summary>
		/// escape. '^['
		/// </summary>
		Escape = Sdl.SDLK_ESCAPE ,
		/// <summary>
		/// space. ' '
		/// </summary>
		Space = Sdl.SDLK_SPACE ,
		/// <summary>
		/// exclaim. '!'
		/// </summary>
		ExclamationPoint = Sdl.SDLK_EXCLAIM ,
		/// <summary>
		/// quotedbl. '"'
		/// </summary>
		DoubleQuote = Sdl.SDLK_QUOTEDBL ,
		/// <summary>
		/// hash. '#'
		/// </summary>
		Hash = Sdl.SDLK_HASH ,
		/// <summary>
		/// dollar. '$'
		/// </summary>
		DollarSign = Sdl.SDLK_DOLLAR ,
		/// <summary>
		/// ampersand. '&amp;'
		/// </summary>
		Ampersand = Sdl.SDLK_AMPERSAND ,
		/// <summary>
		/// quote. '''
		/// </summary>
		Quote = Sdl.SDLK_QUOTE ,
		/// <summary>
		/// left parenthesis. '('
		/// </summary>
		LeftParenthesis = Sdl.SDLK_LEFTPAREN ,
		/// <summary>
		/// right parenthesis. ')'
		/// </summary>
		RightParenthesis = Sdl.SDLK_RIGHTPAREN ,
		/// <summary>
		/// asterisk. '*'
		/// </summary>
		Asterisk = Sdl.SDLK_ASTERISK ,
		/// <summary>
		/// plus sign. '+'
		/// </summary>
		Plus = Sdl.SDLK_PLUS ,
		/// <summary>
		/// comma. ','
		/// </summary>
		Comma = Sdl.SDLK_COMMA ,
		/// <summary>
		/// minus sign. '-'
		/// </summary>
		Minus = Sdl.SDLK_MINUS ,
		/// <summary>
		/// period. '.'
		/// </summary>
		Period = Sdl.SDLK_PERIOD ,
		/// <summary>
		/// forward slash. '/'
		/// </summary>
		Slash = Sdl.SDLK_SLASH ,
		/// <summary>
		/// 0
		/// </summary>
		Zero = Sdl.SDLK_0 ,
		/// <summary>
		/// 1
		/// </summary>
		One = Sdl.SDLK_1 ,
		/// <summary>
		/// 2
		/// </summary>
		Two = Sdl.SDLK_2 ,
		/// <summary>
		/// 3
		/// </summary>
		Three = Sdl.SDLK_3 ,
		/// <summary>
		/// 4
		/// </summary>
		Four = Sdl.SDLK_4 ,
		/// <summary>
		/// 5
		/// </summary>
		Five = Sdl.SDLK_5 ,
		/// <summary>
		/// 6
		/// </summary>
		Six = Sdl.SDLK_6 ,
		/// <summary>
		/// 7
		/// </summary>
		Seven = Sdl.SDLK_7 ,
		/// <summary>
		/// 8
		/// </summary>
		Eight = Sdl.SDLK_8 ,
		/// <summary>
		/// 9
		/// </summary>
		Nine = Sdl.SDLK_9 ,
		/// <summary>
		/// colon. ':'
		/// </summary>
		Colon = Sdl.SDLK_COLON ,
		/// <summary>
		/// semicolon. ';'
		/// </summary>
		Semicolon = Sdl.SDLK_SEMICOLON ,
		/// <summary>
		/// less-than sign. '&lt;'
		/// </summary>
		LessThan = Sdl.SDLK_LESS ,
		/// <summary>
		/// equals sign. '='
		/// </summary>
		Equals = Sdl.SDLK_EQUALS ,
		/// <summary>
		/// greater-than sign. '&gt;'
		/// </summary>
		GreaterThan = Sdl.SDLK_GREATER ,
		/// <summary>
		/// question mark. '?'
		/// </summary>
		QuestionMark = Sdl.SDLK_QUESTION ,
		/// <summary>
		/// at. '@'
		/// </summary>
		At = Sdl.SDLK_AT ,
		/*
		Skip uppercase letters
		*/
		/// <summary>
		/// left bracket. '['
		/// </summary>
		LeftBracket = Sdl.SDLK_LEFTBRACKET ,
		/// <summary>
		/// backslash. '\'
		/// </summary>
		Backslash = Sdl.SDLK_BACKSLASH ,
		/// <summary>
		/// right bracket. ']'
		/// </summary>
		RightBracket = Sdl.SDLK_RIGHTBRACKET ,
		/// <summary>
		/// caret. '^'
		/// </summary>
		Caret = Sdl.SDLK_CARET ,
		/// <summary>
		/// underscore.'_'
		/// </summary>
		Underscore = Sdl.SDLK_UNDERSCORE ,
		/// <summary>
		/// grave. '`'
		/// </summary>
		BackQuote = Sdl.SDLK_BACKQUOTE ,
		/// <summary>
		/// a
		/// </summary>
		A = Sdl.SDLK_a ,
		/// <summary>
		/// b
		/// </summary>
		B = Sdl.SDLK_b ,
		/// <summary>
		/// c
		/// </summary>
		C = Sdl.SDLK_c ,
		/// <summary>
		/// d
		/// </summary>
		D = Sdl.SDLK_d ,
		/// <summary>
		/// e
		/// </summary>
		E = Sdl.SDLK_e ,
		/// <summary>
		/// f
		/// </summary>
		F = Sdl.SDLK_f ,
		/// <summary>
		/// g
		/// </summary>
		G = Sdl.SDLK_g ,
		/// <summary>
		/// h
		/// </summary>
		H = Sdl.SDLK_h ,
		/// <summary>
		/// i
		/// </summary>
		I = Sdl.SDLK_i ,
		/// <summary>
		/// j
		/// </summary>
		J = Sdl.SDLK_j ,
		/// <summary>
		/// k
		/// </summary>
		K = Sdl.SDLK_k ,
		/// <summary>
		/// l
		/// </summary>
		L = Sdl.SDLK_l ,
		/// <summary>
		/// m
		/// </summary>
		M = Sdl.SDLK_m ,
		/// <summary>
		/// n
		/// </summary>
		N = Sdl.SDLK_n ,
		/// <summary>
		/// o
		/// </summary>
		O = Sdl.SDLK_o ,
		/// <summary>
		/// p
		/// </summary>
		P = Sdl.SDLK_p ,
		/// <summary>
		/// q
		/// </summary>
		Q = Sdl.SDLK_q ,
		/// <summary>
		/// r
		/// </summary>
		R = Sdl.SDLK_r ,
		/// <summary>
		/// s
		/// </summary>
		S = Sdl.SDLK_s ,
		/// <summary>
		/// t
		/// </summary>
		T = Sdl.SDLK_t ,
		/// <summary>
		/// u
		/// </summary>
		U = Sdl.SDLK_u ,
		/// <summary>
		/// v
		/// </summary>
		V = Sdl.SDLK_v ,
		/// <summary>
		/// w
		/// </summary>
		W = Sdl.SDLK_w ,
		/// <summary>
		/// x
		/// </summary>
		X = Sdl.SDLK_x ,
		/// <summary>
		/// y
		/// </summary>
		Y = Sdl.SDLK_y ,
		/// <summary>
		/// z
		/// </summary>
		Z = Sdl.SDLK_z ,
		/// <summary>
		/// delete. '^?'
		/// </summary>
		Delete = Sdl.SDLK_DELETE ,
		/* End of ASCII mapped keysyms */

		/* International keyboard syms */
		/// <summary>
		/// 0xA0
		/// </summary>
		World0 = Sdl.SDLK_WORLD_0 ,
		/// <summary>
		///
		/// </summary>
		World1 = Sdl.SDLK_WORLD_1 ,
		/// <summary>
		///
		/// </summary>
		World2 = Sdl.SDLK_WORLD_2 ,
		/// <summary>
		///
		/// </summary>
		World3 = Sdl.SDLK_WORLD_3 ,
		/// <summary>
		///
		/// </summary>
		World4 = Sdl.SDLK_WORLD_4 ,
		/// <summary>
		///
		/// </summary>
		World5 = Sdl.SDLK_WORLD_5 ,
		/// <summary>
		///
		/// </summary>
		World6 = Sdl.SDLK_WORLD_6 ,
		/// <summary>
		///
		/// </summary>
		World7 = Sdl.SDLK_WORLD_7 ,
		/// <summary>
		///
		/// </summary>
		World8 = Sdl.SDLK_WORLD_8 ,
		/// <summary>
		///
		/// </summary>
		World9 = Sdl.SDLK_WORLD_9 ,
		/// <summary>
		///
		/// </summary>
		World10 = Sdl.SDLK_WORLD_10 ,
		/// <summary>
		///
		/// </summary>
		World11 = Sdl.SDLK_WORLD_11 ,
		/// <summary>
		///
		/// </summary>
		World12 = Sdl.SDLK_WORLD_12 ,
		/// <summary>
		///
		/// </summary>
		World13 = Sdl.SDLK_WORLD_13 ,
		/// <summary>
		///
		/// </summary>
		World14 = Sdl.SDLK_WORLD_14 ,
		/// <summary>
		///
		/// </summary>
		World15 = Sdl.SDLK_WORLD_15 ,
		/// <summary>
		///
		/// </summary>
		World16 = Sdl.SDLK_WORLD_16 ,
		/// <summary>
		///
		/// </summary>
		World17 = Sdl.SDLK_WORLD_17 ,
		/// <summary>
		///
		/// </summary>
		World18 = Sdl.SDLK_WORLD_18 ,
		/// <summary>
		///
		/// </summary>
		World19 = Sdl.SDLK_WORLD_19 ,
		/// <summary>
		///
		/// </summary>
		World20 = Sdl.SDLK_WORLD_20 ,
		/// <summary>
		///
		/// </summary>
		World21 = Sdl.SDLK_WORLD_21 ,
		/// <summary>
		///
		/// </summary>
		World22 = Sdl.SDLK_WORLD_22 ,
		/// <summary>
		///
		/// </summary>
		World23 = Sdl.SDLK_WORLD_23 ,
		/// <summary>
		///
		/// </summary>
		World24 = Sdl.SDLK_WORLD_24 ,
		/// <summary>
		///
		/// </summary>
		World25 = Sdl.SDLK_WORLD_25 ,
		/// <summary>
		///
		/// </summary>
		World26 = Sdl.SDLK_WORLD_26 ,
		/// <summary>
		///
		/// </summary>
		World27 = Sdl.SDLK_WORLD_27 ,
		/// <summary>
		///
		/// </summary>
		World28 = Sdl.SDLK_WORLD_28 ,
		/// <summary>
		///
		/// </summary>
		World29 = Sdl.SDLK_WORLD_29 ,
		/// <summary>
		///
		/// </summary>
		World30 = Sdl.SDLK_WORLD_30 ,
		/// <summary>
		///
		/// </summary>
		World31 = Sdl.SDLK_WORLD_31 ,
		/// <summary>
		///
		/// </summary>
		World32 = Sdl.SDLK_WORLD_32 ,
		/// <summary>
		///
		/// </summary>
		World33 = Sdl.SDLK_WORLD_33 ,
		/// <summary>
		///
		/// </summary>
		World34 = Sdl.SDLK_WORLD_34 ,
		/// <summary>
		///
		/// </summary>
		World35 = Sdl.SDLK_WORLD_35 ,
		/// <summary>
		///
		/// </summary>
		World36 = Sdl.SDLK_WORLD_36 ,
		/// <summary>
		///
		/// </summary>
		World37 = Sdl.SDLK_WORLD_37 ,
		/// <summary>
		///
		/// </summary>
		World38 = Sdl.SDLK_WORLD_38 ,
		/// <summary>
		///
		/// </summary>
		World39 = Sdl.SDLK_WORLD_39 ,
		/// <summary>
		///
		/// </summary>
		World40 = Sdl.SDLK_WORLD_40 ,
		/// <summary>
		///
		/// </summary>
		World41 = Sdl.SDLK_WORLD_41 ,
		/// <summary>
		///
		/// </summary>
		World42 = Sdl.SDLK_WORLD_42 ,
		/// <summary>
		///
		/// </summary>
		World43 = Sdl.SDLK_WORLD_43 ,
		/// <summary>
		///
		/// </summary>
		World44 = Sdl.SDLK_WORLD_44 ,
		/// <summary>
		///
		/// </summary>
		World45 = Sdl.SDLK_WORLD_45 ,
		/// <summary>
		///
		/// </summary>
		World46 = Sdl.SDLK_WORLD_46 ,
		/// <summary>
		///
		/// </summary>
		World47 = Sdl.SDLK_WORLD_47 ,
		/// <summary>
		///
		/// </summary>
		World48 = Sdl.SDLK_WORLD_48 ,
		/// <summary>
		///
		/// </summary>
		World49 = Sdl.SDLK_WORLD_49 ,
		/// <summary>
		///
		/// </summary>
		World50 = Sdl.SDLK_WORLD_50 ,
		/// <summary>
		///
		/// </summary>
		World51 = Sdl.SDLK_WORLD_51 ,
		/// <summary>
		///
		/// </summary>
		World52 = Sdl.SDLK_WORLD_52 ,
		/// <summary>
		///
		/// </summary>
		World53 = Sdl.SDLK_WORLD_53 ,
		/// <summary>
		///
		/// </summary>
		World54 = Sdl.SDLK_WORLD_54 ,
		/// <summary>
		///
		/// </summary>
		World55 = Sdl.SDLK_WORLD_55 ,
		/// <summary>
		///
		/// </summary>
		World56 = Sdl.SDLK_WORLD_56 ,
		/// <summary>
		///
		/// </summary>
		World57 = Sdl.SDLK_WORLD_57 ,
		/// <summary>
		///
		/// </summary>
		World58 = Sdl.SDLK_WORLD_58 ,
		/// <summary>
		///
		/// </summary>
		World59 = Sdl.SDLK_WORLD_59 ,
		/// <summary>
		///
		/// </summary>
		World60 = Sdl.SDLK_WORLD_60 ,
		/// <summary>
		///
		/// </summary>
		World61 = Sdl.SDLK_WORLD_61 ,
		/// <summary>
		///
		/// </summary>
		World62 = Sdl.SDLK_WORLD_62 ,
		/// <summary>
		///
		/// </summary>
		World63 = Sdl.SDLK_WORLD_63 ,
		/// <summary>
		///
		/// </summary>
		World64 = Sdl.SDLK_WORLD_64 ,
		/// <summary>
		///
		/// </summary>
		World65 = Sdl.SDLK_WORLD_65 ,
		/// <summary>
		///
		/// </summary>
		World66 = Sdl.SDLK_WORLD_66 ,
		/// <summary>
		///
		/// </summary>
		World67 = Sdl.SDLK_WORLD_67 ,
		/// <summary>
		///
		/// </summary>
		World68 = Sdl.SDLK_WORLD_68 ,
		/// <summary>
		///
		/// </summary>
		World69 = Sdl.SDLK_WORLD_69 ,
		/// <summary>
		///
		/// </summary>
		World70 = Sdl.SDLK_WORLD_70 ,
		/// <summary>
		///
		/// </summary>
		World71 = Sdl.SDLK_WORLD_71 ,
		/// <summary>
		///
		/// </summary>
		World72 = Sdl.SDLK_WORLD_72 ,
		/// <summary>
		///
		/// </summary>
		World73 = Sdl.SDLK_WORLD_73 ,
		/// <summary>
		///
		/// </summary>
		World74 = Sdl.SDLK_WORLD_74 ,
		/// <summary>
		///
		/// </summary>
		World75 = Sdl.SDLK_WORLD_75 ,
		/// <summary>
		///
		/// </summary>
		World76 = Sdl.SDLK_WORLD_76 ,
		/// <summary>
		///
		/// </summary>
		World77 = Sdl.SDLK_WORLD_77 ,
		/// <summary>
		///
		/// </summary>
		World78 = Sdl.SDLK_WORLD_78 ,
		/// <summary>
		///
		/// </summary>
		World79 = Sdl.SDLK_WORLD_79 ,
		/// <summary>
		///
		/// </summary>
		World80 = Sdl.SDLK_WORLD_80 ,
		/// <summary>
		///
		/// </summary>
		World81 = Sdl.SDLK_WORLD_81 ,
		/// <summary>
		///
		/// </summary>
		World82 = Sdl.SDLK_WORLD_82 ,
		/// <summary>
		///
		/// </summary>
		World83 = Sdl.SDLK_WORLD_83 ,
		/// <summary>
		///
		/// </summary>
		World84 = Sdl.SDLK_WORLD_84 ,
		/// <summary>
		///
		/// </summary>
		World85 = Sdl.SDLK_WORLD_85 ,
		/// <summary>
		///
		/// </summary>
		World86 = Sdl.SDLK_WORLD_86 ,
		/// <summary>
		///
		/// </summary>
		World87 = Sdl.SDLK_WORLD_87 ,
		/// <summary>
		///
		/// </summary>
		World88 = Sdl.SDLK_WORLD_88 ,
		/// <summary>
		///
		/// </summary>
		World89 = Sdl.SDLK_WORLD_89 ,
		/// <summary>
		///
		/// </summary>
		World90 = Sdl.SDLK_WORLD_90 ,
		/// <summary>
		///
		/// </summary>
		World91 = Sdl.SDLK_WORLD_91 ,
		/// <summary>
		///
		/// </summary>
		World92 = Sdl.SDLK_WORLD_92 ,
		/// <summary>
		///
		/// </summary>
		World93 = Sdl.SDLK_WORLD_93 ,
		/// <summary>
		///
		/// </summary>
		World94 = Sdl.SDLK_WORLD_94 ,
		/// <summary>
		/// 0xFF
		/// </summary>
		World95 = Sdl.SDLK_WORLD_95 ,

		/* Numeric keypad */
		/// <summary>
		/// keypad 0
		/// </summary>
		Keypad0 = Sdl.SDLK_KP0 ,
		/// <summary>
		/// keypad 1
		/// </summary>
		Keypad1 = Sdl.SDLK_KP1 ,
		/// <summary>
		/// keypad 2
		/// </summary>
		Keypad2 = Sdl.SDLK_KP2 ,
		/// <summary>
		/// keypad 3
		/// </summary>
		Keypad3 = Sdl.SDLK_KP3 ,
		/// <summary>
		/// keypad 4
		/// </summary>
		Keypad4 = Sdl.SDLK_KP4 ,
		/// <summary>
		/// keypad 5
		/// </summary>
		Keypad5 = Sdl.SDLK_KP5 ,
		/// <summary>
		/// keypad 6
		/// </summary>
		Keypad6 = Sdl.SDLK_KP6 ,
		/// <summary>
		/// keypad 7
		/// </summary>
		Keypad7 = Sdl.SDLK_KP7 ,
		/// <summary>
		/// keypad 8
		/// </summary>
		Keypad8 = Sdl.SDLK_KP8 ,
		/// <summary>
		/// keypad 9
		/// </summary>
		Keypad9 = Sdl.SDLK_KP9 ,
		/// <summary>
		/// keypad period. '.'
		/// </summary>
		KeypadPeriod = Sdl.SDLK_KP_PERIOD ,
		/// <summary>
		/// keypad divide. '/'
		/// </summary>
		KeypadDivide = Sdl.SDLK_KP_DIVIDE ,
		/// <summary>
		/// keypad multiply. '*'
		/// </summary>
		KeypadMultiply = Sdl.SDLK_KP_MULTIPLY ,
		/// <summary>
		/// keypad minus. '-'
		/// </summary>
		KeypadMinus = Sdl.SDLK_KP_MINUS ,
		/// <summary>
		/// keypad plus. '+'
		/// </summary>
		KeypadPlus = Sdl.SDLK_KP_PLUS ,
		/// <summary>
		/// keypad enter. '\r'
		/// </summary>
		KeypadEnter = Sdl.SDLK_KP_ENTER ,
		/// <summary>
		/// keypad equals. '='
		/// </summary>
		KeypadEquals = Sdl.SDLK_KP_EQUALS ,

		/* Arrows + Home/End pad */
		/// <summary>
		/// up arrow
		/// </summary>
		UpArrow = Sdl.SDLK_UP ,
		/// <summary>
		/// down arrow
		/// </summary>
		DownArrow = Sdl.SDLK_DOWN ,
		/// <summary>
		/// right arrow
		/// </summary>
		RightArrow = Sdl.SDLK_RIGHT ,
		/// <summary>
		/// left arrow
		/// </summary>
		LeftArrow = Sdl.SDLK_LEFT ,
		/// <summary>
		/// insert
		/// </summary>
		Insert = Sdl.SDLK_INSERT ,
		/// <summary>
		/// home
		/// </summary>
		Home = Sdl.SDLK_HOME ,
		/// <summary>
		/// end
		/// </summary>
		End = Sdl.SDLK_END ,
		/// <summary>
		/// page up
		/// </summary>
		PageUp = Sdl.SDLK_PAGEUP ,
		/// <summary>
		/// page down
		/// </summary>
		PageDown = Sdl.SDLK_PAGEDOWN ,

		/* Function keys */
		/// <summary>
		/// F1
		/// </summary>
		F1 = Sdl.SDLK_F1 ,
		/// <summary>
		/// F2
		/// </summary>
		F2 = Sdl.SDLK_F2 ,
		/// <summary>
		/// F3
		/// </summary>
		F3 = Sdl.SDLK_F3 ,
		/// <summary>
		/// F4
		/// </summary>
		F4 = Sdl.SDLK_F4 ,
		/// <summary>
		/// F5
		/// </summary>
		F5 = Sdl.SDLK_F5 ,
		/// <summary>
		/// F6
		/// </summary>
		F6 = Sdl.SDLK_F6 ,
		/// <summary>
		/// F7
		/// </summary>
		F7 = Sdl.SDLK_F7 ,
		/// <summary>
		/// F8
		/// </summary>
		F8 = Sdl.SDLK_F8 ,
		/// <summary>
		/// F9
		/// </summary>
		F9 = Sdl.SDLK_F9 ,
		/// <summary>
		/// F10
		/// </summary>
		F10 = Sdl.SDLK_F10 ,
		/// <summary>
		/// F11
		/// </summary>
		F11 = Sdl.SDLK_F11 ,
		/// <summary>
		/// F12
		/// </summary>
		F12 = Sdl.SDLK_F12 ,
		/// <summary>
		/// F13
		/// </summary>
		F13 = Sdl.SDLK_F13 ,
		/// <summary>
		/// F14
		/// </summary>
		F14 = Sdl.SDLK_F14 ,
		/// <summary>
		/// F15
		/// </summary>
		F15 = Sdl.SDLK_F15 ,

		/* Key state modifier keys */
		/// <summary>
		/// numlock
		/// </summary>
		NumLock = Sdl.SDLK_NUMLOCK ,
		/// <summary>
		/// capslock
		/// </summary>
		CapsLock = Sdl.SDLK_CAPSLOCK ,
		/// <summary>
		/// scrollock
		/// </summary>
		ScrollLock = Sdl.SDLK_SCROLLOCK ,
		/// <summary>
		/// right shift
		/// </summary>
		RightShift = Sdl.SDLK_RSHIFT ,
		/// <summary>
		/// left shift
		/// </summary>
		LeftShift = Sdl.SDLK_LSHIFT ,
		/// <summary>
		/// right ctrl
		/// </summary>
		RightControl = Sdl.SDLK_RCTRL ,
		/// <summary>
		/// left ctrl
		/// </summary>
		LeftControl = Sdl.SDLK_LCTRL ,
		/// <summary>
		/// right alt
		/// </summary>
		RightAlt = Sdl.SDLK_RALT ,
		/// <summary>
		/// left alt
		/// </summary>
		LeftAlt = Sdl.SDLK_LALT ,
		/// <summary>
		/// right meta
		/// </summary>
		RightMeta = Sdl.SDLK_RMETA ,
		/// <summary>
		/// left meta
		/// </summary>
		LeftMeta = Sdl.SDLK_LMETA ,
		/// <summary>
		/// Left "Windows" key
		/// </summary>
		LeftWindows = Sdl.SDLK_LSUPER ,
		/// <summary>
		/// Right "Windows" key
		/// </summary>
		RightWindows = Sdl.SDLK_RSUPER ,
		/// <summary>
		/// "Alt Gr" key. Mode key
		/// </summary>
		Mode = Sdl.SDLK_MODE ,
		/// <summary>
		/// Multi-key compose key
		/// </summary>
		Compose = Sdl.SDLK_COMPOSE ,

		// Miscellaneous function keys
		/// <summary>
		/// help
		/// </summary>
		Help = Sdl.SDLK_HELP ,
		/// <summary>
		/// print-screen
		/// </summary>
		Print = Sdl.SDLK_PRINT ,
		/// <summary>
		/// SysRq
		/// </summary>
		SysRQ = Sdl.SDLK_SYSREQ ,
		/// <summary>
		/// break
		/// </summary>
		Break = Sdl.SDLK_BREAK ,
		/// <summary>
		/// menu
		/// </summary>
		Menu = Sdl.SDLK_MENU ,
		/// <summary>
		/// Power Macintosh power key
		/// </summary>
		Power = Sdl.SDLK_POWER ,
		/// <summary>
		/// Some european keyboards
		/// </summary>
		Euro = Sdl.SDLK_EURO ,
		/// <summary>
		/// Atari keyboard has Undo
		/// </summary>
		Undo = Sdl.SDLK_UNDO ,

		// Add any other keys here
		/// <summary>
		///
		/// </summary>
		Last = Sdl.SDLK_LAST
	}
	#endregion Keys

	#region ModifierKeys
	/// <summary>
	/// Enumeration of valid key mods (possibly OR'd together) 
	/// </summary>
	[FlagsAttribute]
	public enum ModifierKeys
	{
		/// <summary>
		/// No modifiers applicable
		/// </summary>
		None = Sdl.KMOD_NONE,
		/// <summary>
		/// Left Shift is down
		/// </summary>
		LeftShift = Sdl.KMOD_LSHIFT,
		/// <summary>
		/// Right Shift is down
		/// </summary>
		RightShift = Sdl.KMOD_RSHIFT,
		/// <summary>
		/// Left Control is down
		/// </summary>
		LeftControl = Sdl.KMOD_LCTRL,
		/// <summary>
		/// Right Control is down
		/// </summary>
		RightControl = Sdl.KMOD_RCTRL,
		/// <summary>
		/// Left Alt is down
		/// </summary>
		LeftAlt = Sdl.KMOD_LALT,
		/// <summary>
		/// Right Alt is down
		/// </summary>
		RightAlt = Sdl.KMOD_RALT,
		/// <summary>
		/// Left Meta is down
		/// </summary>
		LeftMeta = Sdl.KMOD_LMETA,
		/// <summary>
		/// Right Meta is down
		/// </summary>
		RightMeta = Sdl.KMOD_RMETA,
		/// <summary>
		/// Numlock is down
		/// </summary>
		NumLock = Sdl.KMOD_NUM,
		/// <summary>
		/// Capslock is down
		/// </summary>
		Caps = Sdl.KMOD_CAPS,
		/// <summary>
		/// 
		/// </summary>
		Mode = Sdl.KMOD_MODE,
		/// <summary>
		/// 
		/// </summary>
		Reserved = Sdl.KMOD_RESERVED,
		/// <summary>
		/// Both CTRL Keys
		/// </summary>
		ControlKeys = (Sdl.KMOD_LCTRL|Sdl.KMOD_RCTRL),
		/// <summary>
		/// Both SHIFT keys.
		/// </summary>
		ShiftKeys = (Sdl.KMOD_LSHIFT|Sdl.KMOD_RSHIFT),

		/// <summary>
		/// Both ALT keys.
		/// </summary>
		AltKeys = (Sdl.KMOD_LALT|Sdl.KMOD_RALT),

		/// <summary>
		/// Both META keys.
		/// </summary>
		MetaKeys = (Sdl.KMOD_LMETA|Sdl.KMOD_RMETA)
	}
	#endregion ModifierKeys

	/// <summary>
	/// 
	/// </summary>
	public enum SoundChannel
	{
		/// <summary>
		/// 
		/// </summary>
		Mono = 1,
		/// <summary>
		/// 
		/// </summary>
		Stereo = 2
	}

	
	/// <summary>
	/// 
	/// </summary>
	[FlagsAttribute]
	public enum CDTrackTypes
	{
		/// <summary>
		/// 
		/// </summary>
		Audio = Sdl.SDL_AUDIO_TRACK,
		/// <summary>
		/// 
		/// </summary>
		Data = Sdl.SDL_DATA_TRACK
	}

	/// <summary>
	/// 
	/// </summary>
	[FlagsAttribute]
	public enum JoystickAxes
	{
		/// <summary>
		/// 
		/// </summary>
		Horizontal = 0,
		/// <summary>
		/// 
		/// </summary>
		Vertical = 1
	}

	/// <summary>
	/// 
	/// </summary>
	public enum JoystickButtonState:byte
	{
		/// <summary>
		/// 
		/// </summary>
		NotPressed = Sdl.SDL_RELEASED,
		/// <summary>
		/// 
		/// </summary>
		Pressed = Sdl.SDL_PRESSED
	}

	/// <summary>
	/// 
	/// </summary>
	public enum JoystickHatState
	{
		/// <summary>
		/// 
		/// </summary>
		Centered = Sdl.SDL_HAT_CENTERED,
		/// <summary>
		/// 
		/// </summary>
		Up = Sdl.SDL_HAT_UP,
		/// <summary>
		/// 
		/// </summary>
		Right = Sdl.SDL_HAT_RIGHT,
		/// <summary>
		/// 
		/// </summary>
		Down = Sdl.SDL_HAT_DOWN,
		/// <summary>
		/// 
		/// </summary>
		Left = Sdl.SDL_HAT_LEFT,
		/// <summary>
		/// 
		/// </summary>
		RightUp = Sdl.SDL_HAT_RIGHTUP,
		/// <summary>
		/// 
		/// </summary>
		RightDown = Sdl.SDL_HAT_RIGHTDOWN,
		/// <summary>
		/// 
		/// </summary>
		LeftUp = Sdl.SDL_HAT_LEFTUP,
		/// <summary>
		/// 
		/// </summary>
		LeftDown = Sdl.SDL_HAT_LEFTDOWN
	}

	/// <summary>
	/// 
	/// </summary>
	public enum SoundAction
	{
		/// <summary>
		/// 
		/// </summary>
		Stop,
		/// <summary>
		/// 
		/// </summary>
		Fadeout
	}

	/// <summary>
	/// 
	/// </summary>
	public enum MusicTypes
	{
		/// <summary>
		/// 
		/// </summary>
		None = SdlMixer.MUS_NONE,
		/// <summary>
		/// 
		/// </summary>
		ExternalCommand = SdlMixer.MUS_CMD,
		/// <summary>
		/// 
		/// </summary>
		Wave = SdlMixer.MUS_WAV,
		/// <summary>
		/// 
		/// </summary>
		Mod = SdlMixer.MUS_MOD,
		/// <summary>
		/// 
		/// </summary>
		Midi = SdlMixer.MUS_MID,
		/// <summary>
		/// 
		/// </summary>
		Ogg = SdlMixer.MUS_OGG,
		/// <summary>
		/// 
		/// </summary>
		Mp3 = SdlMixer.MUS_MP3
	}
}
