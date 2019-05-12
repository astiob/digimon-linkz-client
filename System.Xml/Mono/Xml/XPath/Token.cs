using System;

namespace Mono.Xml.XPath
{
	internal class Token
	{
		public const int ERROR = 257;

		public const int EOF = 258;

		public const int SLASH = 259;

		public const int SLASH2 = 260;

		public const int DOT = 262;

		public const int DOT2 = 263;

		public const int COLON2 = 265;

		public const int COMMA = 267;

		public const int AT = 268;

		public const int FUNCTION_NAME = 269;

		public const int BRACKET_OPEN = 270;

		public const int BRACKET_CLOSE = 271;

		public const int PAREN_OPEN = 272;

		public const int PAREN_CLOSE = 273;

		public const int AND = 274;

		public const int and = 275;

		public const int OR = 276;

		public const int or = 277;

		public const int DIV = 278;

		public const int div = 279;

		public const int MOD = 280;

		public const int mod = 281;

		public const int PLUS = 282;

		public const int MINUS = 283;

		public const int ASTERISK = 284;

		public const int DOLLAR = 285;

		public const int BAR = 286;

		public const int EQ = 287;

		public const int NE = 288;

		public const int LE = 290;

		public const int GE = 292;

		public const int LT = 294;

		public const int GT = 295;

		public const int ANCESTOR = 296;

		public const int ancestor = 297;

		public const int ANCESTOR_OR_SELF = 298;

		public const int ATTRIBUTE = 300;

		public const int attribute = 301;

		public const int CHILD = 302;

		public const int child = 303;

		public const int DESCENDANT = 304;

		public const int descendant = 305;

		public const int DESCENDANT_OR_SELF = 306;

		public const int FOLLOWING = 308;

		public const int following = 309;

		public const int FOLLOWING_SIBLING = 310;

		public const int sibling = 311;

		public const int NAMESPACE = 312;

		public const int NameSpace = 313;

		public const int PARENT = 314;

		public const int parent = 315;

		public const int PRECEDING = 316;

		public const int preceding = 317;

		public const int PRECEDING_SIBLING = 318;

		public const int SELF = 320;

		public const int self = 321;

		public const int COMMENT = 322;

		public const int comment = 323;

		public const int TEXT = 324;

		public const int text = 325;

		public const int PROCESSING_INSTRUCTION = 326;

		public const int NODE = 328;

		public const int node = 329;

		public const int MULTIPLY = 330;

		public const int NUMBER = 331;

		public const int LITERAL = 332;

		public const int QName = 333;

		public const int yyErrorCode = 256;
	}
}
