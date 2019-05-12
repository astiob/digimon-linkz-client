using Mono.Xml.XPath.yydebug;
using Mono.Xml.XPath.yyParser;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal class XPathParser
	{
		internal IStaticXsltContext Context;

		private static int yacc_verbose_flag;

		public TextWriter ErrorOutput = Console.Out;

		public int eof_token;

		internal yyDebug debug;

		protected static int yyFinal = 25;

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
			1,
			1,
			2,
			2,
			2,
			2,
			2,
			2,
			2,
			4,
			4,
			3,
			3,
			3,
			5,
			6,
			6,
			6,
			8,
			8,
			0,
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
			7,
			7,
			7,
			27,
			27,
			26,
			26,
			8,
			8,
			25,
			25,
			9,
			9,
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
			10,
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
			64,
			65,
			71,
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
			0,
			69,
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
			0,
			0,
			0,
			0,
			42,
			78,
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
			77,
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
			0,
			55,
			56,
			0,
			0,
			0,
			0,
			85,
			83,
			88,
			103,
			0,
			20,
			60,
			0,
			61,
			87
		};

		protected static short[] yyDgoto = new short[]
		{
			25,
			0,
			0,
			0,
			0,
			0,
			0,
			78,
			105,
			26,
			69,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			79,
			80,
			112,
			43,
			44,
			45,
			83,
			108
		};

		protected static short[] yySindex = new short[]
		{
			-254,
			-130,
			-130,
			0,
			0,
			0,
			-270,
			-254,
			-254,
			-326,
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
			-266,
			-262,
			-271,
			-256,
			-201,
			-267,
			0,
			-258,
			0,
			0,
			-238,
			-169,
			0,
			0,
			-227,
			0,
			-245,
			0,
			0,
			-169,
			-169,
			-254,
			-243,
			0,
			0,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-254,
			-189,
			-130,
			-130,
			-254,
			0,
			-130,
			-130,
			0,
			0,
			0,
			-237,
			0,
			0,
			0,
			0,
			-232,
			0,
			-224,
			-228,
			0,
			-262,
			-271,
			-256,
			-256,
			-201,
			-201,
			-201,
			-201,
			-267,
			-267,
			0,
			0,
			0,
			0,
			-169,
			-169,
			-222,
			0,
			0,
			-285,
			-219,
			-220,
			-254,
			0,
			0,
			0,
			0,
			-218,
			0,
			0,
			-224,
			0,
			0
		};

		protected static short[] yyRindex = new short[]
		{
			-176,
			1,
			-176,
			0,
			0,
			0,
			0,
			-176,
			-176,
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
			19,
			93,
			37,
			27,
			357,
			276,
			0,
			250,
			0,
			0,
			85,
			114,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			140,
			169,
			-198,
			0,
			0,
			0,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			-176,
			0,
			-176,
			-176,
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
			-208,
			0,
			0,
			336,
			484,
			458,
			476,
			383,
			393,
			419,
			429,
			302,
			328,
			0,
			0,
			0,
			0,
			195,
			224,
			0,
			0,
			0,
			-206,
			59,
			0,
			-176,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-208,
			0,
			0
		};

		protected static short[] yyGindex = new short[]
		{
			-7,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-29,
			0,
			20,
			31,
			48,
			-33,
			44,
			25,
			0,
			29,
			0,
			0,
			2,
			0,
			66,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-23
		};

		protected static short[] yyTable = new short[]
		{
			49,
			51,
			48,
			46,
			47,
			1,
			2,
			51,
			3,
			4,
			52,
			62,
			53,
			63,
			5,
			6,
			54,
			55,
			7,
			21,
			81,
			66,
			67,
			89,
			90,
			91,
			92,
			26,
			65,
			8,
			84,
			9,
			68,
			50,
			56,
			104,
			57,
			24,
			58,
			59,
			106,
			82,
			10,
			107,
			11,
			109,
			12,
			111,
			13,
			110,
			14,
			68,
			15,
			114,
			16,
			116,
			17,
			72,
			18,
			57,
			19,
			101,
			20,
			64,
			21,
			86,
			22,
			102,
			99,
			100,
			1,
			2,
			85,
			3,
			4,
			84,
			113,
			23,
			24,
			5,
			6,
			60,
			61,
			7,
			86,
			46,
			70,
			95,
			96,
			97,
			70,
			71,
			117,
			22,
			98,
			73,
			9,
			74,
			0,
			75,
			115,
			76,
			87,
			88,
			93,
			94,
			77,
			10,
			70,
			11,
			0,
			12,
			0,
			13,
			49,
			14,
			0,
			15,
			0,
			16,
			0,
			17,
			0,
			18,
			70,
			19,
			70,
			20,
			70,
			21,
			70,
			22,
			3,
			4,
			0,
			70,
			102,
			103,
			5,
			0,
			52,
			0,
			23,
			24,
			0,
			0,
			70,
			0,
			70,
			0,
			70,
			0,
			70,
			0,
			0,
			0,
			0,
			70,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			10,
			0,
			11,
			53,
			12,
			0,
			13,
			0,
			14,
			0,
			15,
			0,
			16,
			0,
			17,
			0,
			18,
			0,
			19,
			0,
			20,
			0,
			21,
			0,
			22,
			0,
			0,
			0,
			0,
			47,
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
			48,
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
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			41,
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
			51,
			0,
			0,
			0,
			51,
			0,
			51,
			51,
			34,
			51,
			0,
			51,
			0,
			51,
			0,
			51,
			51,
			70,
			21,
			51,
			51,
			51,
			21,
			51,
			21,
			51,
			26,
			51,
			51,
			0,
			26,
			0,
			26,
			26,
			35,
			26,
			24,
			0,
			0,
			0,
			24,
			0,
			24,
			24,
			0,
			24,
			26,
			26,
			0,
			0,
			57,
			57,
			0,
			0,
			0,
			70,
			0,
			70,
			57,
			70,
			36,
			70,
			57,
			51,
			57,
			57,
			70,
			57,
			23,
			57,
			0,
			57,
			0,
			57,
			57,
			0,
			0,
			57,
			57,
			57,
			0,
			57,
			0,
			57,
			46,
			57,
			57,
			0,
			46,
			29,
			46,
			46,
			22,
			46,
			0,
			46,
			22,
			46,
			22,
			46,
			46,
			22,
			0,
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
			49,
			0,
			32,
			0,
			49,
			0,
			49,
			49,
			57,
			49,
			0,
			49,
			33,
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
			46,
			52,
			0,
			52,
			30,
			52,
			0,
			52,
			52,
			0,
			0,
			52,
			52,
			52,
			31,
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
			27,
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
			28,
			47,
			47,
			0,
			0,
			47,
			47,
			47,
			25,
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
			0,
			0,
			41,
			41,
			0,
			41,
			0,
			41,
			34,
			41,
			41,
			0,
			34,
			0,
			34,
			34,
			0,
			34,
			0,
			48,
			0,
			0,
			0,
			34,
			34,
			0,
			0,
			0,
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
			0,
			41,
			0,
			0,
			0,
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
			23,
			36,
			0,
			0,
			23,
			0,
			23,
			36,
			36,
			23,
			0,
			0,
			36,
			36,
			0,
			36,
			0,
			36,
			0,
			36,
			36,
			29,
			0,
			0,
			0,
			29,
			0,
			29,
			29,
			0,
			29,
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
			29,
			29,
			0,
			29,
			0,
			29,
			32,
			29,
			29,
			0,
			32,
			0,
			32,
			32,
			0,
			32,
			33,
			0,
			0,
			0,
			33,
			0,
			33,
			33,
			0,
			33,
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
			33,
			33,
			0,
			33,
			0,
			33,
			30,
			33,
			33,
			0,
			30,
			0,
			30,
			30,
			0,
			30,
			31,
			0,
			0,
			0,
			31,
			0,
			31,
			31,
			0,
			31,
			30,
			30,
			0,
			30,
			0,
			30,
			0,
			30,
			30,
			0,
			31,
			31,
			0,
			31,
			0,
			31,
			0,
			31,
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
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			28,
			0,
			27,
			27,
			28,
			0,
			28,
			28,
			25,
			28,
			0,
			0,
			25,
			0,
			25,
			25,
			0,
			25,
			0,
			0,
			28,
			28
		};

		protected static short[] yyCheck = new short[]
		{
			7,
			0,
			272,
			1,
			2,
			259,
			260,
			333,
			262,
			263,
			276,
			278,
			274,
			280,
			268,
			269,
			287,
			288,
			272,
			0,
			265,
			259,
			260,
			56,
			57,
			58,
			59,
			0,
			286,
			283,
			273,
			285,
			270,
			8,
			290,
			272,
			292,
			0,
			294,
			295,
			272,
			48,
			296,
			267,
			298,
			273,
			300,
			332,
			302,
			271,
			304,
			270,
			306,
			273,
			308,
			273,
			310,
			284,
			312,
			0,
			314,
			68,
			316,
			330,
			318,
			273,
			320,
			273,
			66,
			67,
			259,
			260,
			52,
			262,
			263,
			273,
			105,
			331,
			332,
			268,
			269,
			282,
			283,
			272,
			53,
			0,
			284,
			62,
			63,
			64,
			259,
			260,
			115,
			0,
			65,
			322,
			285,
			324,
			-1,
			326,
			107,
			328,
			54,
			55,
			60,
			61,
			333,
			296,
			284,
			298,
			-1,
			300,
			-1,
			302,
			0,
			304,
			-1,
			306,
			-1,
			308,
			-1,
			310,
			-1,
			312,
			322,
			314,
			324,
			316,
			326,
			318,
			328,
			320,
			262,
			263,
			-1,
			333,
			70,
			71,
			268,
			-1,
			0,
			-1,
			331,
			332,
			-1,
			-1,
			322,
			-1,
			324,
			-1,
			326,
			-1,
			328,
			-1,
			-1,
			-1,
			-1,
			333,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			296,
			-1,
			298,
			0,
			300,
			-1,
			302,
			-1,
			304,
			-1,
			306,
			-1,
			308,
			-1,
			310,
			-1,
			312,
			-1,
			314,
			-1,
			316,
			-1,
			318,
			-1,
			320,
			-1,
			-1,
			-1,
			-1,
			0,
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
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			0,
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
			-1,
			-1,
			-1,
			0,
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
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			267,
			-1,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			0,
			276,
			-1,
			278,
			-1,
			280,
			-1,
			282,
			283,
			284,
			267,
			286,
			287,
			288,
			271,
			290,
			273,
			292,
			267,
			294,
			295,
			-1,
			271,
			-1,
			273,
			274,
			0,
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
			-1,
			259,
			260,
			-1,
			-1,
			-1,
			322,
			-1,
			324,
			267,
			326,
			0,
			328,
			271,
			330,
			273,
			274,
			333,
			276,
			0,
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
			0,
			273,
			274,
			267,
			276,
			-1,
			278,
			271,
			280,
			273,
			282,
			283,
			276,
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
			0,
			-1,
			271,
			-1,
			273,
			274,
			330,
			276,
			-1,
			278,
			0,
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
			0,
			280,
			-1,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			0,
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
			0,
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
			0,
			282,
			283,
			-1,
			-1,
			286,
			287,
			288,
			0,
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
			330,
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
			-1,
			330,
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
			267,
			276,
			-1,
			-1,
			271,
			-1,
			273,
			282,
			283,
			276,
			-1,
			-1,
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
			-1,
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
			267,
			-1,
			287,
			288,
			271,
			-1,
			273,
			274,
			267,
			276,
			-1,
			-1,
			271,
			-1,
			273,
			274,
			-1,
			276,
			-1,
			-1,
			287,
			288
		};

		public XPathParser() : this(null)
		{
		}

		internal XPathParser(IStaticXsltContext context)
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
			if (XPathParser.yacc_verbose_flag > 0 && expected != null && expected.Length > 0)
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
			if (token < 0 || token > XPathParser.yyNames.Length)
			{
				return "[illegal]";
			}
			string result;
			if ((result = XPathParser.yyNames[token]) != null)
			{
				return result;
			}
			return "[unknown]";
		}

		protected int[] yyExpectingTokens(int state)
		{
			int num = 0;
			bool[] array = new bool[XPathParser.yyNames.Length];
			int i;
			int num2;
			if ((i = (int)XPathParser.yySindex[state]) != 0)
			{
				num2 = ((i >= 0) ? 0 : (-i));
				while (num2 < XPathParser.yyNames.Length && i + num2 < XPathParser.yyTable.Length)
				{
					if ((int)XPathParser.yyCheck[i + num2] == num2 && !array[num2] && XPathParser.yyNames[num2] != null)
					{
						num++;
						array[num2] = true;
					}
					num2++;
				}
			}
			if ((i = (int)XPathParser.yyRindex[state]) != 0)
			{
				num2 = ((i >= 0) ? 0 : (-i));
				while (num2 < XPathParser.yyNames.Length && i + num2 < XPathParser.yyTable.Length)
				{
					if ((int)XPathParser.yyCheck[i + num2] == num2 && !array[num2] && XPathParser.yyNames[num2] != null)
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
				array2[i++] = XPathParser.yyNames[array[i]];
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
				while ((num5 = (int)XPathParser.yyDefRed[num]) == 0)
				{
					if (num2 < 0)
					{
						num2 = ((!yyLex.advance()) ? 0 : yyLex.token());
						if (this.debug != null)
						{
							this.debug.lex(num, num2, XPathParser.yyname(num2), yyLex.value());
						}
					}
					if ((num5 = (int)XPathParser.yySindex[num]) != 0 && (num5 += num2) >= 0 && num5 < XPathParser.yyTable.Length && (int)XPathParser.yyCheck[num5] == num2)
					{
						if (this.debug != null)
						{
							this.debug.shift(num, (int)XPathParser.yyTable[num5], num3 - 1);
						}
						num = (int)XPathParser.yyTable[num5];
						obj = yyLex.value();
						num2 = -1;
						if (num3 > 0)
						{
							num3--;
						}
					}
					else
					{
						if ((num5 = (int)XPathParser.yyRindex[num]) != 0 && (num5 += num2) >= 0 && num5 < XPathParser.yyTable.Length && (int)XPathParser.yyCheck[num5] == num2)
						{
							num5 = (int)XPathParser.yyTable[num5];
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
								this.debug.discard(num, num2, XPathParser.yyname(num2), yyLex.value());
							}
							num2 = -1;
							continue;
						default:
							goto IL_34B;
						}
						num3 = 3;
						while ((num5 = (int)XPathParser.yySindex[array[num4]]) == 0 || (num5 += 256) < 0 || num5 >= XPathParser.yyTable.Length || XPathParser.yyCheck[num5] != 256)
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
							this.debug.shift(array[num4], (int)XPathParser.yyTable[num5], 3);
						}
						num = (int)XPathParser.yyTable[num5];
						obj = yyLex.value();
					}
					IL_EA8:
					num4++;
					goto IL_EB3;
				}
				IL_34B:
				int num6 = num4 + 1 - (int)XPathParser.yyLen[num5];
				if (this.debug != null)
				{
					this.debug.reduce(num, array[num6 - 1], num5, XPathParser.YYRules.getRule(num5), (int)XPathParser.yyLen[num5]);
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
				num4 -= (int)XPathParser.yyLen[num5];
				num = array[num4];
				int num7 = (int)XPathParser.yyLhs[num5];
				if (num != 0 || num7 != 0)
				{
					if ((num5 = (int)XPathParser.yyGindex[num7]) != 0 && (num5 += num) >= 0 && num5 < XPathParser.yyTable.Length && (int)XPathParser.yyCheck[num5] == num)
					{
						num = (int)XPathParser.yyTable[num5];
					}
					else
					{
						num = (int)XPathParser.yyDgoto[num7];
					}
					if (this.debug != null)
					{
						this.debug.shift(array[num4], num);
					}
					goto IL_EA8;
				}
				if (this.debug != null)
				{
					this.debug.shift(0, XPathParser.yyFinal);
				}
				num = XPathParser.yyFinal;
				if (num2 < 0)
				{
					num2 = ((!yyLex.advance()) ? 0 : yyLex.token());
					if (this.debug != null)
					{
						this.debug.lex(num, num2, XPathParser.yyname(num2), yyLex.value());
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
				"$accept : Expr",
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
				return XPathParser.YYRules.yyRule[index];
			}
		}
	}
}
