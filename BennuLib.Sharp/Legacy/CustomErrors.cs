namespace BennuLib
{
	static class CustomErrors
	{



		//===============================================================================
		// MODULE OVERVIEW
		//===============================================================================
		// SpritePocket error constants and utility functions.
		//===============================================================================

		// Error messages are defined in the ./res/SpritePocket.res resource file and are
		// related to these error constants.
		public const int ERR_CANNOTOPENFILE = 101;
		public const int ERR_NOTANFPGFILE = 102;
		public const int ERR_UNSUPPORTEDVERSION = 103;
		public const int ERR_PALETTETRUNCATED = 104;
		public const int ERR_GAMMATRUNCATED = 105;
		public const int ERR_CPINFOTRUNCATED = 106;
		public const int ERR_ANIMATIONFLAGACTIVE = 107;
		public const int ERR_BITMAPDATATRUNCATED = 108;
		// Public Const ERR_OBJECTUNAVAILABLE As Long = 109
		public const int ERR_NOTAMAPFILE = 110;
		public const int ERR_NOTAPALFILE = 111;
		public const int ERR_MAPINDEXOUTOFRANGE = 112;
		public const int ERR_DIFFERENTDEPTHS = 113;
		public const int ERR_INVALIDCODE = 114;
		public const int ERR_PALETTEREQUIRED = 115;
		public const int ERR_UNSUPPORTEDDEPTH = 116;
		public const int ERR_SETPALETTEINVALIDDEPTH = 117;
		public const int ERR_INVALIDCPID = 118;
		public const int ERR_INVALIDMAPWIDTH = 119;
		public const int ERR_INVALIDMAPHEIGHT = 120;
		public const int ERR_INVALIDCOLORINDEX = 121;
		public const int ERR_INSUFFICIENTARRAYELEMENTS = 122;
		public const int ERR_FILEDOESNOTCONTAINPALETTE = 123;
		public const int ERR_CANNOTCONVERTMAPINFPG = 124;
		public const int ERR_INVALIDDEPTH = 125;
		public const int ERR_NOTANFNTFILE = 126;
		public const int ERR_SERIALIZEDMAPINVALID = 127;
		public const int ERR_INVALIDCOMPRESSIONLEVEL = 128;
		public const int ERR_ZLIB = 129;

		public const int ERR_FILEWRITE = 130;
		// FreeImage error constants
		public const int ERR_FREEIMAGE = 2000;

		public const int ERR_FREEIMAGE_NOTKNOWNFILE = 2001;
		// As opposed to errors, warnings do not represent a SpritePocket failure and don't
		// need to be handled
		public const int WARNING_DUPLICATEDCODE = 1001;
		public const int WARNING_UNSUPPORTEDOPERATION = 1002;
		public const int WARNING_CANNOTCREATEOBJECT = 1003;

		public const int WARNING_NOTSTANDARDDECODER = 1004;
	}
}
