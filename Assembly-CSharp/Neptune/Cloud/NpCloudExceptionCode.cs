using System;

namespace Neptune.Cloud
{
	public class NpCloudExceptionCode
	{
		public const short None = 0;

		public const string NoneMsg = "通常終了";

		public const short Exception = 700;

		public const string ExceptionMsg = "Message = {0}\nStackTrace = {1}";

		public const short SwitchCaseOther = 701;

		public const string SwitchCaseOtherMsg = "Switchのcaseがdefaultだったよ = {0}";

		public const short LockConnectAbnormality = 702;

		public const string LockConnectAbnormalityMsg = "サーバーとの接続が切れている = {0}";

		public const short ConnectFailure = 711;

		public const string ConnectFailureMsg = "Socketの接続に失敗しました。";

		public const short ConnectServerIncorrect = 712;

		public const short TimeOut = 720;

		public const string TimeOutMsg = "接続リクエストのタイムアウトです。";

		public const short LoginFailure = 721;

		public const string LoginFailureMsg = "サーバーへの接続に失敗しました。";

		public const short ReadBufferAbnormality = 730;

		public const string ReadBufferAbnormalityMsg = "読み込んだデータが異常でした。";

		public const short SendBufferAbnormality = 731;

		public const string SendBufferAbnormalityMsg = "送信データのバッファが異常でした";

		public const short NotUnpackBodyParameter = 732;

		public const string NotUnpackBodyParameterMsg = "Bodyパラメーターが存在しない";

		public const short ResponesOtherCommand = 733;

		public const string ResponseOtherCommandMsg = "想定外のコマンドがレスポンスで返ってきました。";

		public const short OnApplicationQuit = 740;

		public const string OnApplicationQuitMessage = "OnApplicationQuit";

		public const short OnApplicationPause = 741;

		public const string OnApplicationPauseMessage = "OnApplicationPause";

		public const short PongFailure = 750;

		public const string PongFailureMsg = "Pongにより切断された";

		public const short ReceiveBufferSizeAbnormality = 751;

		public const string ReceiveBufferSizeAbnormalityMsg = "受信したデータbufferが異常でした";

		public const short ThreadSocketException = 752;

		public const string ThreadSOcketExceptionMsg = "Message = {0}\nStackTrace = {1}";

		public const short ThreadException = 753;

		public const string ThreadExceptionMsg = "Message = {0}\nStackTrace = {1}";

		public const short HeaderSizeAbnormality = 754;

		public const string HeaderSizeAbnormalityMsg = "ヘッダーサイズ異常です";

		public const short SocketNoConnected = 760;

		public const string SocketNoConnectedMsg = "ソケットに未接続";

		public const short HttpRequestConnectErr = 761;

		public const string HttpRequestConnectErrMsg = "Httpリクエスト通信エラー";

		public const short RoomIdAbnormality = 770;

		public const string RoomIdAbnormalityMsg = "ルームIDが異常です";

		public const short HashDataError = 780;

		public const string HashDataErrorMsg = "データが存在しませんでした。";

		public const short HashDifferent = 781;

		public const string HashDifferentMsg = "ハッシュの値が違います、改ざんされている可能性があります。";
	}
}
