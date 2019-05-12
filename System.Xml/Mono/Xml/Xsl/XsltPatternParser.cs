using Mono.Xml.Xsl.yydebug;
using Mono.Xml.Xsl.yyParser;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XsltPatternParser
	{
		internal IStaticXsltContext Context;

		private static int yacc_verbose_flag;

		public TextWriter ErrorOutput = Console.Out;

		public int eof_token;

		internal yyDebug debug;

		protected static int yyFinal = 7;

		protected static string[] yyNames = new string[]
		{
			"end-of-file",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"'$'",
			null,
			null,
			null,
			"'('",
			"')'",
			"'*'",
			"'+'",
			"','",
			"'-'",
			"'.'",
			"'/'",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"'<'",
			"'='",
			"'>'",
			null,
			"'@'",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"'['",
			null,
			"']'",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"'|'",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"ERROR",
			"EOF",
			"SLASH",
			"SLASH2",
			"\"//\"",
			"DOT",
			"DOT2",
			"\"..\"",
			"COLON2",
			"\"::\"",
			"COMMA",
			"AT",
			"FUNCTION_NAME",
			"BRACKET_OPEN",
			"BRACKET_CLOSE",
			"PAREN_OPEN",
			"PAREN_CLOSE",
			"AND",
			"\"and\"",
			"OR",
			"\"or\"",
			"DIV",
			"\"div\"",
			"MOD",
			"\"mod\"",
			"PLUS",
			"MINUS",
			"ASTERISK",
			"DOLLAR",
			"BAR",
			"EQ",
			"NE",
			"\"!=\"",
			"LE",
			"\"<=\"",
			"GE",
			"\">=\"",
			"LT",
			"GT",
			"ANCESTOR",
			"\"ancestor\"",
			"ANCESTOR_OR_SELF",
			"\"ancstor-or-self\"",
			"ATTRIBUTE",
			"\"attribute\"",
			"CHILD",
			"\"child\"",
			"DESCENDANT",
			"\"descendant\"",
			"DESCENDANT_OR_SELF",
			"\"descendant-or-self\"",
			"FOLLOWING",
			"\"following\"",
			"FOLLOWING_SIBLING",
			"\"sibling\"",
			"NAMESPACE",
			"\"NameSpace\"",
			"PARENT",
			"\"parent\"",
			"PRECEDING",
			"\"preceding\"",
			"PRECEDING_SIBLING",
			"\"preceding-sibling\"",
			"SELF",
			"\"self\"",
			"COMMENT",
			"\"comment\"",
			"TEXT",
			"\"text\"",
			"PROCESSING_INSTRUCTION",
			"\"processing-instruction\"",
			"NODE",
			"\"node\"",
			"MULTIPLY",
			"NUMBER",
			"LITERAL",
			"QName"
		};

		private int yyExpectingState;

		protected int yyMax;

		private static short[] yyLhs = new short[]
		{
			-1,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			3,
			3,
			2,
			2,
			2,
			4,
			5,
			5,
			5,
			7,
			7,
			10,
			11,
			11,
			12,
			12,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			16,
			16,
			16,
			16,
			17,
			17,
			18,
			18,
			19,
			19,
			19,
			19,
			20,
			20,
			23,
			23,
			23,
			22,
			22,
			22,
			24,
			24,
			6,
			6,
			6,
			27,
			27,
			26,
			26,
			7,
			7,
			25,
			25,
			8,
			8,
			28,
			28,
			28,
			28,
			21,
			21,
			31,
			31,
			31,
			31,
			31,
			32,
			33,
			33,
			34,
			34,
			9,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			30,
			29,
			29
		};

		private static short[] yyLen = new short[]
		{
			2,
			1,
			3,
			1,
			2,
			1,
			3,
			3,
			2,
			1,
			4,
			6,
			1,
			3,
			3,
			3,
			1,
			2,
			2,
			0,
			2,
			1,
			1,
			3,
			1,
			3,
			1,
			3,
			3,
			1,
			3,
			3,
			3,
			3,
			1,
			3,
			3,
			1,
			3,
			3,
			3,
			1,
			2,
			1,
			3,
			1,
			1,
			3,
			3,
			1,
			1,
			1,
			2,
			2,
			1,
			3,
			3,
			3,
			1,
			1,
			3,
			4,
			1,
			1,
			1,
			1,
			0,
			2,
			2,
			1,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			2,
			2,
			3,
			1,
			1,
			1,
			4,
			0,
			2,
			0,
			3,
			3,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			1
		};

		private static short[] yyDefRed = new short[]
		{
			0,
			0,
			0,
			71,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			12,
			0,
			16,
			0,
			0,
			0,
			18,
			17,
			0,
			0,
			0,
			0,
			0,
			62,
			72,
			73,
			0,
			75,
			63,
			19,
			59,
			0,
			0,
			2,
			13,
			14,
			0,
			0,
			0,
			0,
			0,
			0,
			10,
			103,
			0,
			0,
			20,
			60,
			0,
			61,
			0,
			0,
			64,
			65,
			0,
			0,
			0,
			0,
			89,
			90,
			91,
			92,
			93,
			94,
			95,
			96,
			97,
			98,
			99,
			100,
			101,
			81,
			80,
			69,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			37,
			0,
			43,
			45,
			0,
			0,
			50,
			54,
			0,
			58,
			0,
			76,
			82,
			11,
			0,
			0,
			0,
			0,
			42,
			78,
			88,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			77,
			0,
			0,
			19,
			68,
			0,
			0,
			79,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			39,
			40,
			38,
			44,
			0,
			0,
			55,
			56,
			0,
			0,
			85,
			83,
			0,
			87
		};

		protected static short[] yyDgoto = new short[]
		{
			7,
			8,
			9,
			10,
			11,
			12,
			30,
			40,
			74,
			47,
			75,
			76,
			77,
			78,
			79,
			80,
			81,
			82,
			83,
			84,
			85,
			86,
			87,
			88,
			89,
			90,
			91,
			31,
			32,
			45,
			92,
			93,
			94,
			125,
			147
		};

		protected static short[] yySindex = new short[]
		{
			-245,
			-251,
			-251,
			0,
			-261,
			-221,
			-218,
			-256,
			0,
			-231,
			-219,
			0,
			-225,
			0,
			-231,
			-231,
			-279,
			0,
			0,
			-245,
			-251,
			-251,
			-251,
			-251,
			0,
			0,
			0,
			-211,
			0,
			0,
			0,
			0,
			-209,
			-235,
			0,
			0,
			0,
			-231,
			-231,
			-247,
			-174,
			-167,
			-220,
			0,
			0,
			-159,
			-250,
			0,
			0,
			-157,
			0,
			216,
			216,
			0,
			0,
			-154,
			-250,
			-250,
			-213,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-149,
			-152,
			-148,
			-215,
			-16,
			-203,
			-244,
			0,
			-158,
			0,
			0,
			-239,
			-50,
			0,
			0,
			-225,
			0,
			-135,
			0,
			0,
			0,
			-50,
			-50,
			-250,
			-141,
			0,
			0,
			0,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-250,
			-185,
			216,
			216,
			0,
			216,
			216,
			0,
			0,
			-128,
			-131,
			0,
			-148,
			-215,
			-16,
			-16,
			-203,
			-203,
			-203,
			-203,
			-244,
			-244,
			0,
			0,
			0,
			0,
			-50,
			-50,
			0,
			0,
			-174,
			-250,
			0,
			0,
			-128,
			0
		};

		protected static short[] yyRindex = new short[]
		{
			-122,
			1,
			-122,
			0,
			0,
			0,
			0,
			0,
			0,
			3,
			4,
			0,
			0,
			0,
			5,
			6,
			0,
			0,
			0,
			-122,
			-122,
			-122,
			-122,
			-122,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			7,
			8,
			-124,
			2,
			0,
			0,
			0,
			0,
			0,
			-122,
			0,
			0,
			0,
			0,
			-97,
			-122,
			0,
			0,
			0,
			-122,
			-122,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-228,
			-115,
			-169,
			219,
			-53,
			264,
			0,
			193,
			0,
			0,
			-23,
			57,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			83,
			112,
			-257,
			0,
			0,
			0,
			0,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			-122,
			0,
			-122,
			-122,
			0,
			0,
			-119,
			0,
			0,
			-68,
			-133,
			388,
			407,
			10,
			342,
			352,
			378,
			290,
			316,
			0,
			0,
			0,
			0,
			138,
			167,
			0,
			0,
			-123,
			-122,
			0,
			0,
			-119,
			0
		};

		protected static short[] yyGindex = new short[]
		{
			0,
			126,
			87,
			0,
			192,
			0,
			76,
			46,
			548,
			89,
			-56,
			0,
			70,
			74,
			110,
			201,
			134,
			-20,
			0,
			64,
			0,
			0,
			-26,
			0,
			175,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			33
		};

		protected static short[] yyTable = new short[]
		{
			99,
			3,
			15,
			9,
			5,
			4,
			8,
			6,
			7,
			51,
			52,
			16,
			53,
			54,
			1,
			2,
			84,
			3,
			3,
			55,
			117,
			118,
			56,
			3,
			4,
			96,
			97,
			70,
			20,
			21,
			19,
			46,
			42,
			57,
			113,
			58,
			114,
			100,
			43,
			21,
			22,
			23,
			124,
			21,
			17,
			21,
			59,
			18,
			60,
			5,
			61,
			6,
			62,
			33,
			63,
			5,
			64,
			6,
			65,
			24,
			66,
			39,
			67,
			41,
			68,
			70,
			69,
			70,
			70,
			70,
			71,
			70,
			105,
			106,
			51,
			52,
			70,
			53,
			54,
			111,
			112,
			72,
			73,
			3,
			55,
			44,
			115,
			56,
			14,
			15,
			149,
			141,
			142,
			137,
			138,
			139,
			46,
			25,
			24,
			26,
			58,
			27,
			24,
			28,
			24,
			24,
			48,
			24,
			29,
			37,
			38,
			59,
			49,
			60,
			50,
			61,
			95,
			62,
			98,
			63,
			101,
			64,
			102,
			65,
			103,
			66,
			104,
			67,
			116,
			68,
			123,
			69,
			126,
			70,
			25,
			71,
			57,
			57,
			25,
			146,
			25,
			25,
			148,
			25,
			57,
			34,
			72,
			73,
			57,
			102,
			57,
			57,
			22,
			57,
			86,
			57,
			22,
			57,
			22,
			57,
			57,
			22,
			70,
			57,
			57,
			57,
			122,
			57,
			145,
			57,
			51,
			57,
			57,
			127,
			51,
			119,
			51,
			51,
			128,
			51,
			140,
			51,
			150,
			51,
			0,
			51,
			51,
			70,
			0,
			51,
			51,
			51,
			0,
			51,
			0,
			51,
			0,
			51,
			51,
			23,
			70,
			0,
			70,
			23,
			70,
			23,
			70,
			57,
			23,
			120,
			121,
			70,
			35,
			36,
			29,
			129,
			130,
			0,
			29,
			0,
			29,
			29,
			0,
			29,
			0,
			70,
			0,
			70,
			0,
			70,
			0,
			70,
			0,
			51,
			29,
			29,
			70,
			29,
			0,
			29,
			0,
			29,
			29,
			0,
			46,
			135,
			136,
			0,
			46,
			0,
			46,
			46,
			0,
			46,
			0,
			46,
			0,
			46,
			0,
			46,
			46,
			15,
			15,
			46,
			46,
			46,
			0,
			46,
			0,
			46,
			0,
			46,
			46,
			0,
			107,
			0,
			108,
			32,
			109,
			110,
			0,
			32,
			0,
			32,
			32,
			70,
			32,
			3,
			15,
			9,
			5,
			4,
			8,
			6,
			7,
			143,
			144,
			32,
			32,
			0,
			32,
			0,
			32,
			0,
			32,
			32,
			0,
			46,
			131,
			132,
			133,
			134,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			70,
			49,
			70,
			0,
			70,
			49,
			70,
			49,
			49,
			0,
			49,
			70,
			49,
			0,
			49,
			0,
			49,
			49,
			0,
			0,
			49,
			49,
			49,
			0,
			49,
			0,
			49,
			52,
			49,
			49,
			0,
			52,
			0,
			52,
			52,
			0,
			52,
			0,
			52,
			0,
			52,
			0,
			52,
			52,
			0,
			0,
			52,
			52,
			52,
			0,
			52,
			0,
			52,
			0,
			52,
			52,
			53,
			0,
			0,
			0,
			53,
			0,
			53,
			53,
			49,
			53,
			0,
			53,
			0,
			53,
			0,
			53,
			53,
			0,
			0,
			53,
			53,
			53,
			0,
			53,
			0,
			53,
			47,
			53,
			53,
			0,
			47,
			0,
			47,
			47,
			52,
			47,
			0,
			47,
			0,
			47,
			0,
			47,
			47,
			0,
			0,
			47,
			47,
			47,
			0,
			47,
			0,
			47,
			0,
			47,
			47,
			48,
			0,
			0,
			0,
			48,
			0,
			48,
			48,
			53,
			48,
			0,
			48,
			0,
			48,
			0,
			48,
			48,
			0,
			0,
			48,
			48,
			48,
			0,
			48,
			0,
			48,
			41,
			48,
			48,
			0,
			41,
			0,
			41,
			41,
			47,
			41,
			0,
			41,
			0,
			41,
			0,
			41,
			41,
			0,
			53,
			54,
			41,
			41,
			0,
			41,
			3,
			41,
			26,
			41,
			41,
			0,
			26,
			0,
			26,
			26,
			0,
			26,
			0,
			48,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			26,
			26,
			0,
			0,
			0,
			0,
			59,
			0,
			60,
			0,
			61,
			0,
			62,
			0,
			63,
			0,
			64,
			41,
			65,
			0,
			66,
			0,
			67,
			0,
			68,
			34,
			69,
			0,
			70,
			34,
			71,
			34,
			34,
			0,
			34,
			0,
			0,
			0,
			0,
			0,
			34,
			34,
			13,
			13,
			13,
			34,
			34,
			0,
			34,
			0,
			34,
			35,
			34,
			34,
			0,
			35,
			0,
			35,
			35,
			0,
			35,
			13,
			13,
			13,
			13,
			13,
			35,
			35,
			0,
			0,
			0,
			35,
			35,
			0,
			35,
			0,
			35,
			36,
			35,
			35,
			0,
			36,
			0,
			36,
			36,
			0,
			36,
			0,
			0,
			0,
			0,
			0,
			36,
			36,
			0,
			0,
			0,
			36,
			36,
			0,
			36,
			0,
			36,
			33,
			36,
			36,
			0,
			33,
			0,
			33,
			33,
			0,
			33,
			30,
			0,
			0,
			0,
			30,
			0,
			30,
			30,
			0,
			30,
			33,
			33,
			0,
			33,
			0,
			33,
			0,
			33,
			33,
			0,
			30,
			30,
			0,
			30,
			0,
			30,
			31,
			30,
			30,
			0,
			31,
			0,
			31,
			31,
			0,
			31,
			27,
			0,
			0,
			0,
			27,
			0,
			27,
			27,
			0,
			27,
			31,
			31,
			0,
			31,
			0,
			31,
			0,
			31,
			31,
			28,
			27,
			27,
			0,
			28,
			0,
			28,
			28,
			0,
			28,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			28,
			28
		};

		protected static short[] yyCheck = new short[]
		{
			56,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			259,
			260,
			272,
			262,
			263,
			259,
			260,
			273,
			268,
			268,
			269,
			259,
			260,
			272,
			268,
			269,
			51,
			52,
			284,
			259,
			260,
			286,
			270,
			267,
			283,
			278,
			285,
			280,
			57,
			273,
			267,
			259,
			260,
			98,
			271,
			265,
			273,
			296,
			265,
			298,
			300,
			300,
			302,
			302,
			332,
			304,
			300,
			306,
			302,
			308,
			284,
			310,
			272,
			312,
			272,
			314,
			322,
			316,
			324,
			318,
			326,
			320,
			328,
			287,
			288,
			259,
			260,
			333,
			262,
			263,
			282,
			283,
			331,
			332,
			268,
			269,
			332,
			330,
			272,
			1,
			2,
			146,
			117,
			118,
			113,
			114,
			115,
			270,
			322,
			267,
			324,
			285,
			326,
			271,
			328,
			273,
			274,
			273,
			276,
			333,
			22,
			23,
			296,
			332,
			298,
			273,
			300,
			273,
			302,
			272,
			304,
			333,
			306,
			271,
			308,
			276,
			310,
			274,
			312,
			286,
			314,
			265,
			316,
			273,
			318,
			267,
			320,
			259,
			260,
			271,
			267,
			273,
			274,
			273,
			276,
			267,
			19,
			331,
			332,
			271,
			273,
			273,
			274,
			267,
			276,
			273,
			278,
			271,
			280,
			273,
			282,
			283,
			276,
			284,
			286,
			287,
			288,
			90,
			290,
			122,
			292,
			267,
			294,
			295,
			103,
			271,
			86,
			273,
			274,
			104,
			276,
			116,
			278,
			149,
			280,
			-1,
			282,
			283,
			284,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			267,
			322,
			-1,
			324,
			271,
			326,
			273,
			328,
			330,
			276,
			259,
			260,
			333,
			20,
			21,
			267,
			105,
			106,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			322,
			-1,
			324,
			-1,
			326,
			-1,
			328,
			-1,
			330,
			287,
			288,
			333,
			290,
			-1,
			292,
			-1,
			294,
			295,
			-1,
			267,
			111,
			112,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			259,
			260,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			284,
			276,
			286,
			286,
			286,
			286,
			286,
			286,
			286,
			286,
			120,
			121,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			-1,
			330,
			107,
			108,
			109,
			110,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			322,
			267,
			324,
			-1,
			326,
			271,
			328,
			273,
			274,
			-1,
			276,
			333,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			267,
			-1,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			330,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			330,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			267,
			-1,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			330,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			330,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			-1,
			262,
			263,
			287,
			288,
			-1,
			290,
			268,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			330,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			287,
			288,
			-1,
			-1,
			-1,
			-1,
			296,
			-1,
			298,
			-1,
			300,
			-1,
			302,
			-1,
			304,
			-1,
			306,
			330,
			308,
			-1,
			310,
			-1,
			312,
			-1,
			314,
			267,
			316,
			-1,
			318,
			271,
			320,
			273,
			274,
			-1,
			276,
			-1,
			-1,
			-1,
			-1,
			-1,
			282,
			283,
			0,
			1,
			2,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			19,
			20,
			21,
			22,
			23,
			282,
			283,
			-1,
			-1,
			-1,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			-1,
			-1,
			-1,
			-1,
			282,
			283,
			-1,
			-1,
			-1,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			267,
			-1,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			-1,
			287,
			288,
			-1,
			290,
			-1,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			267,
			-1,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			287,
			288,
			-1,
			290,
			-1,
			292,
			-1,
			294,
			295,
			267,
			287,
			288,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			287,
			288
		};

		public XsltPatternParser() : this(null)
		{
		}

		internal XsltPatternParser(IStaticXsltContext context)
		{
			this.Context = context;
			this.ErrorOutput = TextWriter.Null;
		}

		internal Expression Compile(string xpath)
		{
			Expression result;
			try
			{
				Tokenizer yyLex = new Tokenizer(xpath);
				result = (Expression)this.yyparse(yyLex);
			}
			catch (XPathException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new XPathException("Error during parse of " + xpath, innerException);
			}
			return result;
		}

		private NodeSet CreateNodeTest(Axes axis, object nodeTest, ArrayList plist)
		{
			NodeSet nodeSet = this.CreateNodeTest(axis, nodeTest);
			if (plist != null)
			{
				for (int i = 0; i < plist.Count; i++)
				{
					nodeSet = new ExprFilter(nodeSet, (Expression)plist[i]);
				}
			}
			return nodeSet;
		}

		private NodeTest CreateNodeTest(Axes axis, object test)
		{
			if (test is XPathNodeType)
			{
				return new NodeTypeTest(axis, (XPathNodeType)((int)test), null);
			}
			if (test is string || test == null)
			{
				return new NodeTypeTest(axis, XPathNodeType.ProcessingInstruction, (string)test);
			}
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)test;
			if (xmlQualifiedName == XmlQualifiedName.Empty)
			{
				return new NodeTypeTest(axis);
			}
			return new NodeNameTest(axis, xmlQualifiedName, this.Context);
		}

		public void yyerror(string message)
		{
			this.yyerror(message, null);
		}

		public void yyerror(string message, string[] expected)
		{
			if (XsltPatternParser.yacc_verbose_flag > 0 && expected != null && expected.Length > 0)
			{
				this.ErrorOutput.Write(message + ", expecting");
				for (int i = 0; i < expected.Length; i++)
				{
					this.ErrorOutput.Write(" " + expected[i]);
				}
				this.ErrorOutput.WriteLine();
			}
			else
			{
				this.ErrorOutput.WriteLine(message);
			}
		}

		public static string yyname(int token)
		{
			if (token < 0 || token > XsltPatternParser.yyNames.Length)
			{
				return "[illegal]";
			}
			string result;
			if ((result = XsltPatternParser.yyNames[token]) != null)
			{
				return result;
			}
			return "[unknown]";
		}

		protected int[] yyExpectingTokens(int state)
		{
			int num = 0;
			bool[] array = new bool[XsltPatternParser.yyNames.Length];
			int i;
			int num2;
			if ((i = (int)XsltPatternParser.yySindex[state]) != 0)
			{
				num2 = ((i >= 0) ? 0 : (-i));
				while (num2 < XsltPatternParser.yyNames.Length && i + num2 < XsltPatternParser.yyTable.Length)
				{
					if ((int)XsltPatternParser.yyCheck[i + num2] == num2 && !array[num2] && XsltPatternParser.yyNames[num2] != null)
					{
						num++;
						array[num2] = true;
					}
					num2++;
				}
			}
			if ((i = (int)XsltPatternParser.yyRindex[state]) != 0)
			{
				num2 = ((i >= 0) ? 0 : (-i));
				while (num2 < XsltPatternParser.yyNames.Length && i + num2 < XsltPatternParser.yyTable.Length)
				{
					if ((int)XsltPatternParser.yyCheck[i + num2] == num2 && !array[num2] && XsltPatternParser.yyNames[num2] != null)
					{
						num++;
						array[num2] = true;
					}
					num2++;
				}
			}
			int[] array2 = new int[num];
			num2 = (i = 0);
			while (i < num)
			{
				if (array[num2])
				{
					array2[i++] = num2;
				}
				num2++;
			}
			return array2;
		}

		protected string[] yyExpecting(int state)
		{
			int[] array = this.yyExpectingTokens(state);
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i++] = XsltPatternParser.yyNames[array[i]];
			}
			return array2;
		}

		internal object yyparse(yyInput yyLex, object yyd)
		{
			this.debug = (yyDebug)yyd;
			return this.yyparse(yyLex);
		}

		protected object yyDefault(object first)
		{
			return first;
		}

		internal object yyparse(yyInput yyLex)
		{
			if (this.yyMax <= 0)
			{
				this.yyMax = 256;
			}
			int num = 0;
			int[] array = new int[this.yyMax];
			object obj = null;
			object[] array2 = new object[this.yyMax];
			int num2 = -1;
			int num3 = 0;
			int num4 = 0;
			XmlQualifiedName xmlQualifiedName;
			XmlQualifiedName xmlQualifiedName2;
			for (;;)
			{
				IL_EB3:
				if (num4 >= array.Length)
				{
					int[] array3 = new int[array.Length + this.yyMax];
					array.CopyTo(array3, 0);
					array = array3;
					object[] array4 = new object[array2.Length + this.yyMax];
					array2.CopyTo(array4, 0);
					array2 = array4;
				}
				array[num4] = num;
				array2[num4] = obj;
				if (this.debug != null)
				{
					this.debug.push(num, obj);
				}
				int num5;
				while ((num5 = (int)XsltPatternParser.yyDefRed[num]) == 0)
				{
					if (num2 < 0)
					{
						num2 = ((!yyLex.advance()) ? 0 : yyLex.token());
						if (this.debug != null)
						{
							this.debug.lex(num, num2, XsltPatternParser.yyname(num2), yyLex.value());
						}
					}
					if ((num5 = (int)XsltPatternParser.yySindex[num]) != 0 && (num5 += num2) >= 0 && num5 < XsltPatternParser.yyTable.Length && (int)XsltPatternParser.yyCheck[num5] == num2)
					{
						if (this.debug != null)
						{
							this.debug.shift(num, (int)XsltPatternParser.yyTable[num5], num3 - 1);
						}
						num = (int)XsltPatternParser.yyTable[num5];
						obj = yyLex.value();
						num2 = -1;
						if (num3 > 0)
						{
							num3--;
						}
					}
					else
					{
						if ((num5 = (int)XsltPatternParser.yyRindex[num]) != 0 && (num5 += num2) >= 0 && num5 < XsltPatternParser.yyTable.Length && (int)XsltPatternParser.yyCheck[num5] == num2)
						{
							num5 = (int)XsltPatternParser.yyTable[num5];
							break;
						}
						switch (num3)
						{
						case 0:
							this.yyExpectingState = num;
							if (this.debug != null)
							{
								this.debug.error("syntax error");
							}
							if (num2 == 0 || num2 == this.eof_token)
							{
								goto IL_224;
							}
							break;
						case 1:
						case 2:
							break;
						case 3:
							if (num2 == 0)
							{
								goto Block_29;
							}
							if (this.debug != null)
							{
								this.debug.discard(num, num2, XsltPatternParser.yyname(num2), yyLex.value());
							}
							num2 = -1;
							continue;
						default:
							goto IL_34B;
						}
						num3 = 3;
						while ((num5 = (int)XsltPatternParser.yySindex[array[num4]]) == 0 || (num5 += 256) < 0 || num5 >= XsltPatternParser.yyTable.Length || XsltPatternParser.yyCheck[num5] != 256)
						{
							if (this.debug != null)
							{
								this.debug.pop(array[num4]);
							}
							if (--num4 < 0)
							{
								goto Block_27;
							}
						}
						if (this.debug != null)
						{
							this.debug.shift(array[num4], (int)XsltPatternParser.yyTable[num5], 3);
						}
						num = (int)XsltPatternParser.yyTable[num5];
						obj = yyLex.value();
					}
					IL_EA8:
					num4++;
					goto IL_EB3;
				}
				IL_34B:
				int num6 = num4 + 1 - (int)XsltPatternParser.yyLen[num5];
				if (this.debug != null)
				{
					this.debug.reduce(num, array[num6 - 1], num5, XsltPatternParser.YYRules.getRule(num5), (int)XsltPatternParser.yyLen[num5]);
				}
				obj = this.yyDefault((num6 <= num4) ? array2[num6] : null);
				switch (num5)
				{
				case 2:
					obj = new ExprUNION((NodeSet)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 3:
					obj = new ExprRoot();
					break;
				case 4:
					obj = new ExprSLASH(new ExprRoot(), (NodeSet)array2[0 + num4]);
					break;
				case 6:
					obj = new ExprSLASH((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 7:
					obj = new ExprSLASH2((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 8:
					obj = new ExprSLASH2(new ExprRoot(), (NodeSet)array2[0 + num4]);
					break;
				case 10:
					xmlQualifiedName = (XmlQualifiedName)array2[-3 + num4];
					if (xmlQualifiedName.Name != "id" || xmlQualifiedName.Namespace != string.Empty)
					{
						goto IL_625;
					}
					obj = ExprFunctionCall.Factory(xmlQualifiedName, new FunctionArguments(new ExprLiteral((string)array2[-1 + num4]), null), this.Context);
					break;
				case 11:
					xmlQualifiedName2 = (XmlQualifiedName)array2[-5 + num4];
					if (xmlQualifiedName2.Name != "key" || xmlQualifiedName2.Namespace != string.Empty)
					{
						goto IL_69A;
					}
					obj = this.Context.TryGetFunction(xmlQualifiedName2, new FunctionArguments(new ExprLiteral((string)array2[-3 + num4]), new FunctionArguments(new ExprLiteral((string)array2[-1 + num4]), null)));
					break;
				case 13:
					obj = new ExprSLASH((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 14:
					obj = new ExprSLASH2((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 15:
					obj = this.CreateNodeTest((Axes)((int)array2[-2 + num4]), array2[-1 + num4], (ArrayList)array2[0 + num4]);
					break;
				case 17:
					obj = Axes.Child;
					break;
				case 18:
					obj = Axes.Attribute;
					break;
				case 19:
					obj = null;
					break;
				case 20:
				{
					ArrayList arrayList = (ArrayList)array2[-1 + num4];
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add((Expression)array2[0 + num4]);
					obj = arrayList;
					break;
				}
				case 23:
					obj = new ExprOR((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 25:
					obj = new ExprAND((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 27:
					obj = new ExprEQ((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 28:
					obj = new ExprNE((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 30:
					obj = new ExprLT((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 31:
					obj = new ExprGT((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 32:
					obj = new ExprLE((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 33:
					obj = new ExprGE((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 35:
					obj = new ExprPLUS((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 36:
					obj = new ExprMINUS((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 38:
					obj = new ExprMULT((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 39:
					obj = new ExprDIV((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 40:
					obj = new ExprMOD((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 42:
					obj = new ExprNEG((Expression)array2[0 + num4]);
					break;
				case 44:
					obj = new ExprUNION((Expression)array2[-2 + num4], (Expression)array2[0 + num4]);
					break;
				case 47:
					obj = new ExprSLASH((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 48:
					obj = new ExprSLASH2((Expression)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 51:
					obj = new ExprRoot();
					break;
				case 52:
					obj = new ExprSLASH(new ExprRoot(), (NodeSet)array2[0 + num4]);
					break;
				case 53:
					obj = new ExprSLASH2(new ExprRoot(), (NodeSet)array2[0 + num4]);
					break;
				case 55:
					obj = new ExprSLASH((NodeSet)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 56:
					obj = new ExprSLASH2((NodeSet)array2[-2 + num4], (NodeSet)array2[0 + num4]);
					break;
				case 57:
					obj = this.CreateNodeTest((Axes)((int)array2[-2 + num4]), array2[-1 + num4], (ArrayList)array2[0 + num4]);
					break;
				case 60:
					obj = (XPathNodeType)((int)array2[-2 + num4]);
					break;
				case 61:
					obj = (string)array2[-1 + num4];
					break;
				case 62:
					obj = XmlQualifiedName.Empty;
					break;
				case 64:
					obj = new NodeTypeTest(Axes.Self, XPathNodeType.All);
					break;
				case 65:
					obj = new NodeTypeTest(Axes.Parent, XPathNodeType.All);
					break;
				case 66:
					obj = null;
					break;
				case 67:
				{
					ArrayList arrayList2 = (ArrayList)array2[-1 + num4];
					if (arrayList2 == null)
					{
						arrayList2 = new ArrayList();
					}
					arrayList2.Add(array2[0 + num4]);
					obj = arrayList2;
					break;
				}
				case 68:
					obj = array2[-1 + num4];
					break;
				case 70:
					obj = Axes.Child;
					break;
				case 71:
					obj = Axes.Attribute;
					break;
				case 72:
					obj = XPathNodeType.Comment;
					break;
				case 73:
					obj = XPathNodeType.Text;
					break;
				case 74:
					obj = XPathNodeType.ProcessingInstruction;
					break;
				case 75:
					obj = XPathNodeType.All;
					break;
				case 77:
					obj = new ExprFilter((Expression)array2[-1 + num4], (Expression)array2[0 + num4]);
					break;
				case 78:
				{
					Expression expression = null;
					if (this.Context != null)
					{
						expression = this.Context.TryGetVariable(((XmlQualifiedName)array2[0 + num4]).ToString());
					}
					if (expression == null)
					{
						expression = new ExprVariable((XmlQualifiedName)array2[0 + num4], this.Context);
					}
					obj = expression;
					break;
				}
				case 79:
					obj = new ExprParens((Expression)array2[-1 + num4]);
					break;
				case 80:
					obj = new ExprLiteral((string)array2[0 + num4]);
					break;
				case 81:
					obj = new ExprNumber((double)array2[0 + num4]);
					break;
				case 83:
				{
					Expression expression2 = null;
					if (this.Context != null)
					{
						expression2 = this.Context.TryGetFunction((XmlQualifiedName)array2[-3 + num4], (FunctionArguments)array2[-1 + num4]);
					}
					if (expression2 == null)
					{
						expression2 = ExprFunctionCall.Factory((XmlQualifiedName)array2[-3 + num4], (FunctionArguments)array2[-1 + num4], this.Context);
					}
					obj = expression2;
					break;
				}
				case 85:
					obj = new FunctionArguments((Expression)array2[-1 + num4], (FunctionArguments)array2[0 + num4]);
					break;
				case 87:
					obj = new FunctionArguments((Expression)array2[-1 + num4], (FunctionArguments)array2[0 + num4]);
					break;
				case 88:
					obj = array2[-1 + num4];
					break;
				case 89:
					obj = Axes.Ancestor;
					break;
				case 90:
					obj = Axes.AncestorOrSelf;
					break;
				case 91:
					obj = Axes.Attribute;
					break;
				case 92:
					obj = Axes.Child;
					break;
				case 93:
					obj = Axes.Descendant;
					break;
				case 94:
					obj = Axes.DescendantOrSelf;
					break;
				case 95:
					obj = Axes.Following;
					break;
				case 96:
					obj = Axes.FollowingSibling;
					break;
				case 97:
					obj = Axes.Namespace;
					break;
				case 98:
					obj = Axes.Parent;
					break;
				case 99:
					obj = Axes.Preceding;
					break;
				case 100:
					obj = Axes.PrecedingSibling;
					break;
				case 101:
					obj = Axes.Self;
					break;
				}
				num4 -= (int)XsltPatternParser.yyLen[num5];
				num = array[num4];
				int num7 = (int)XsltPatternParser.yyLhs[num5];
				if (num != 0 || num7 != 0)
				{
					if ((num5 = (int)XsltPatternParser.yyGindex[num7]) != 0 && (num5 += num) >= 0 && num5 < XsltPatternParser.yyTable.Length && (int)XsltPatternParser.yyCheck[num5] == num)
					{
						num = (int)XsltPatternParser.yyTable[num5];
					}
					else
					{
						num = (int)XsltPatternParser.yyDgoto[num7];
					}
					if (this.debug != null)
					{
						this.debug.shift(array[num4], num);
					}
					goto IL_EA8;
				}
				if (this.debug != null)
				{
					this.debug.shift(0, XsltPatternParser.yyFinal);
				}
				num = XsltPatternParser.yyFinal;
				if (num2 < 0)
				{
					num2 = ((!yyLex.advance()) ? 0 : yyLex.token());
					if (this.debug != null)
					{
						this.debug.lex(num, num2, XsltPatternParser.yyname(num2), yyLex.value());
					}
				}
				if (num2 == 0)
				{
					goto Block_49;
				}
				goto IL_EA8;
			}
			IL_224:
			throw new yyUnexpectedEof();
			Block_27:
			if (this.debug != null)
			{
				this.debug.reject();
			}
			throw new yyException("irrecoverable syntax error");
			Block_29:
			if (this.debug != null)
			{
				this.debug.reject();
			}
			throw new yyException("irrecoverable syntax error at end-of-file");
			IL_625:
			throw new XPathException(string.Format("Expected 'id' but got '{0}'", xmlQualifiedName));
			IL_69A:
			throw new XPathException(string.Format("Expected 'key' but got '{0}'", xmlQualifiedName2));
			Block_49:
			if (this.debug != null)
			{
				this.debug.accept(obj);
			}
			return obj;
		}

		private class YYRules : MarshalByRefObject
		{
			public static string[] yyRule = new string[]
			{
				"$accept : Pattern",
				"Pattern : LocationPathPattern",
				"Pattern : Pattern BAR LocationPathPattern",
				"LocationPathPattern : SLASH",
				"LocationPathPattern : SLASH RelativePathPattern",
				"LocationPathPattern : IdKeyPattern",
				"LocationPathPattern : IdKeyPattern SLASH RelativePathPattern",
				"LocationPathPattern : IdKeyPattern SLASH2 RelativePathPattern",
				"LocationPathPattern : SLASH2 RelativePathPattern",
				"LocationPathPattern : RelativePathPattern",
				"IdKeyPattern : FUNCTION_NAME PAREN_OPEN LITERAL PAREN_CLOSE",
				"IdKeyPattern : FUNCTION_NAME PAREN_OPEN LITERAL COMMA LITERAL PAREN_CLOSE",
				"RelativePathPattern : StepPattern",
				"RelativePathPattern : RelativePathPattern SLASH StepPattern",
				"RelativePathPattern : RelativePathPattern SLASH2 StepPattern",
				"StepPattern : ChildOrAttributeAxisSpecifier NodeTest Predicates",
				"ChildOrAttributeAxisSpecifier : AbbreviatedAxisSpecifier",
				"ChildOrAttributeAxisSpecifier : CHILD COLON2",
				"ChildOrAttributeAxisSpecifier : ATTRIBUTE COLON2",
				"Predicates :",
				"Predicates : Predicates Predicate",
				"Expr : OrExpr",
				"OrExpr : AndExpr",
				"OrExpr : OrExpr OR AndExpr",
				"AndExpr : EqualityExpr",
				"AndExpr : AndExpr AND EqualityExpr",
				"EqualityExpr : RelationalExpr",
				"EqualityExpr : EqualityExpr EQ RelationalExpr",
				"EqualityExpr : EqualityExpr NE RelationalExpr",
				"RelationalExpr : AdditiveExpr",
				"RelationalExpr : RelationalExpr LT AdditiveExpr",
				"RelationalExpr : RelationalExpr GT AdditiveExpr",
				"RelationalExpr : RelationalExpr LE AdditiveExpr",
				"RelationalExpr : RelationalExpr GE AdditiveExpr",
				"AdditiveExpr : MultiplicativeExpr",
				"AdditiveExpr : AdditiveExpr PLUS MultiplicativeExpr",
				"AdditiveExpr : AdditiveExpr MINUS MultiplicativeExpr",
				"MultiplicativeExpr : UnaryExpr",
				"MultiplicativeExpr : MultiplicativeExpr MULTIPLY UnaryExpr",
				"MultiplicativeExpr : MultiplicativeExpr DIV UnaryExpr",
				"MultiplicativeExpr : MultiplicativeExpr MOD UnaryExpr",
				"UnaryExpr : UnionExpr",
				"UnaryExpr : MINUS UnaryExpr",
				"UnionExpr : PathExpr",
				"UnionExpr : UnionExpr BAR PathExpr",
				"PathExpr : LocationPath",
				"PathExpr : FilterExpr",
				"PathExpr : FilterExpr SLASH RelativeLocationPath",
				"PathExpr : FilterExpr SLASH2 RelativeLocationPath",
				"LocationPath : RelativeLocationPath",
				"LocationPath : AbsoluteLocationPath",
				"AbsoluteLocationPath : SLASH",
				"AbsoluteLocationPath : SLASH RelativeLocationPath",
				"AbsoluteLocationPath : SLASH2 RelativeLocationPath",
				"RelativeLocationPath : Step",
				"RelativeLocationPath : RelativeLocationPath SLASH Step",
				"RelativeLocationPath : RelativeLocationPath SLASH2 Step",
				"Step : AxisSpecifier NodeTest Predicates",
				"Step : AbbreviatedStep",
				"NodeTest : NameTest",
				"NodeTest : NodeType PAREN_OPEN PAREN_CLOSE",
				"NodeTest : PROCESSING_INSTRUCTION PAREN_OPEN OptionalLiteral PAREN_CLOSE",
				"NameTest : ASTERISK",
				"NameTest : QName",
				"AbbreviatedStep : DOT",
				"AbbreviatedStep : DOT2",
				"Predicates :",
				"Predicates : Predicates Predicate",
				"AxisSpecifier : AxisName COLON2",
				"AxisSpecifier : AbbreviatedAxisSpecifier",
				"AbbreviatedAxisSpecifier :",
				"AbbreviatedAxisSpecifier : AT",
				"NodeType : COMMENT",
				"NodeType : TEXT",
				"NodeType : PROCESSING_INSTRUCTION",
				"NodeType : NODE",
				"FilterExpr : PrimaryExpr",
				"FilterExpr : FilterExpr Predicate",
				"PrimaryExpr : DOLLAR QName",
				"PrimaryExpr : PAREN_OPEN Expr PAREN_CLOSE",
				"PrimaryExpr : LITERAL",
				"PrimaryExpr : NUMBER",
				"PrimaryExpr : FunctionCall",
				"FunctionCall : FUNCTION_NAME PAREN_OPEN OptionalArgumentList PAREN_CLOSE",
				"OptionalArgumentList :",
				"OptionalArgumentList : Expr OptionalArgumentListTail",
				"OptionalArgumentListTail :",
				"OptionalArgumentListTail : COMMA Expr OptionalArgumentListTail",
				"Predicate : BRACKET_OPEN Expr BRACKET_CLOSE",
				"AxisName : ANCESTOR",
				"AxisName : ANCESTOR_OR_SELF",
				"AxisName : ATTRIBUTE",
				"AxisName : CHILD",
				"AxisName : DESCENDANT",
				"AxisName : DESCENDANT_OR_SELF",
				"AxisName : FOLLOWING",
				"AxisName : FOLLOWING_SIBLING",
				"AxisName : NAMESPACE",
				"AxisName : PARENT",
				"AxisName : PRECEDING",
				"AxisName : PRECEDING_SIBLING",
				"AxisName : SELF",
				"OptionalLiteral :",
				"OptionalLiteral : LITERAL"
			};

			public static string getRule(int index)
			{
				return XsltPatternParser.YYRules.yyRule[index];
			}
		}
	}
}
