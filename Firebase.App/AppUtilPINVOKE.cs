using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class AppUtilPINVOKE
	{
		protected static AppUtilPINVOKE.SWIGExceptionHelper swigExceptionHelper = new AppUtilPINVOKE.SWIGExceptionHelper();

		protected static AppUtilPINVOKE.SWIGStringHelper swigStringHelper = new AppUtilPINVOKE.SWIGStringHelper();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_FutureBase__SWIG_0")]
		public static extern IntPtr new_FutureBase__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_FutureBase")]
		public static extern void delete_FutureBase(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_FutureBase__SWIG_1")]
		public static extern IntPtr new_FutureBase__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureBase_Release")]
		public static extern void FutureBase_Release(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureBase_status")]
		public static extern int FutureBase_status(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureBase_error")]
		public static extern int FutureBase_error(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureBase_error_message")]
		public static extern string FutureBase_error_message(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_StringStringMap__SWIG_0")]
		public static extern IntPtr new_StringStringMap__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_StringStringMap__SWIG_1")]
		public static extern IntPtr new_StringStringMap__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_size")]
		public static extern uint StringStringMap_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_empty")]
		public static extern bool StringStringMap_empty(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_Clear")]
		public static extern void StringStringMap_Clear(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_getitem")]
		public static extern string StringStringMap_getitem(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_setitem")]
		public static extern void StringStringMap_setitem(HandleRef jarg1, string jarg2, string jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_ContainsKey")]
		public static extern bool StringStringMap_ContainsKey(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_Add")]
		public static extern void StringStringMap_Add(HandleRef jarg1, string jarg2, string jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_Remove")]
		public static extern bool StringStringMap_Remove(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_create_iterator_begin")]
		public static extern IntPtr StringStringMap_create_iterator_begin(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_get_next_key")]
		public static extern string StringStringMap_get_next_key(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringStringMap_destroy_iterator")]
		public static extern void StringStringMap_destroy_iterator(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_StringStringMap")]
		public static extern void delete_StringStringMap(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Clear")]
		public static extern void StringList_Clear(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Add")]
		public static extern void StringList_Add(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_size")]
		public static extern uint StringList_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_capacity")]
		public static extern uint StringList_capacity(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_reserve")]
		public static extern void StringList_reserve(HandleRef jarg1, uint jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_StringList__SWIG_0")]
		public static extern IntPtr new_StringList__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_StringList__SWIG_1")]
		public static extern IntPtr new_StringList__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_StringList__SWIG_2")]
		public static extern IntPtr new_StringList__SWIG_2(int jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_getitemcopy")]
		public static extern string StringList_getitemcopy(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_getitem")]
		public static extern string StringList_getitem(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_setitem")]
		public static extern void StringList_setitem(HandleRef jarg1, int jarg2, string jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_AddRange")]
		public static extern void StringList_AddRange(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_GetRange")]
		public static extern IntPtr StringList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Insert")]
		public static extern void StringList_Insert(HandleRef jarg1, int jarg2, string jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_InsertRange")]
		public static extern void StringList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_RemoveAt")]
		public static extern void StringList_RemoveAt(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_RemoveRange")]
		public static extern void StringList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Repeat")]
		public static extern IntPtr StringList_Repeat(string jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Reverse__SWIG_0")]
		public static extern void StringList_Reverse__SWIG_0(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Reverse__SWIG_1")]
		public static extern void StringList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_SetRange")]
		public static extern void StringList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Contains")]
		public static extern bool StringList_Contains(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_IndexOf")]
		public static extern int StringList_IndexOf(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_LastIndexOf")]
		public static extern int StringList_LastIndexOf(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_StringList_Remove")]
		public static extern bool StringList_Remove(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_StringList")]
		public static extern void delete_StringList(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Clear")]
		public static extern void CharVector_Clear(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Add")]
		public static extern void CharVector_Add(HandleRef jarg1, byte jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_size")]
		public static extern uint CharVector_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_capacity")]
		public static extern uint CharVector_capacity(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_reserve")]
		public static extern void CharVector_reserve(HandleRef jarg1, uint jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_CharVector__SWIG_0")]
		public static extern IntPtr new_CharVector__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_CharVector__SWIG_1")]
		public static extern IntPtr new_CharVector__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_CharVector__SWIG_2")]
		public static extern IntPtr new_CharVector__SWIG_2(int jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_getitemcopy")]
		public static extern byte CharVector_getitemcopy(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_getitem")]
		public static extern byte CharVector_getitem(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_setitem")]
		public static extern void CharVector_setitem(HandleRef jarg1, int jarg2, byte jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_AddRange")]
		public static extern void CharVector_AddRange(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_GetRange")]
		public static extern IntPtr CharVector_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Insert")]
		public static extern void CharVector_Insert(HandleRef jarg1, int jarg2, byte jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_InsertRange")]
		public static extern void CharVector_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_RemoveAt")]
		public static extern void CharVector_RemoveAt(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_RemoveRange")]
		public static extern void CharVector_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Repeat")]
		public static extern IntPtr CharVector_Repeat(byte jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Reverse__SWIG_0")]
		public static extern void CharVector_Reverse__SWIG_0(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Reverse__SWIG_1")]
		public static extern void CharVector_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_SetRange")]
		public static extern void CharVector_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Contains")]
		public static extern bool CharVector_Contains(HandleRef jarg1, byte jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_IndexOf")]
		public static extern int CharVector_IndexOf(HandleRef jarg1, byte jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_LastIndexOf")]
		public static extern int CharVector_LastIndexOf(HandleRef jarg1, byte jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CharVector_Remove")]
		public static extern bool CharVector_Remove(HandleRef jarg1, byte jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_CharVector")]
		public static extern void delete_CharVector(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_FutureString")]
		public static extern IntPtr new_FutureString();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureString_SWIG_OnCompletion")]
		public static extern IntPtr FutureString_SWIG_OnCompletion(HandleRef jarg1, FutureString.SWIG_CompletionDelegate jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureString_SWIG_FreeCompletionData")]
		public static extern void FutureString_SWIG_FreeCompletionData(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureString_result")]
		public static extern string FutureString_result(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_FutureString")]
		public static extern void delete_FutureString(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_FutureVoid")]
		public static extern IntPtr new_FutureVoid();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureVoid_SWIG_OnCompletion")]
		public static extern IntPtr FutureVoid_SWIG_OnCompletion(HandleRef jarg1, FutureVoid.SWIG_CompletionDelegate jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureVoid_SWIG_FreeCompletionData")]
		public static extern void FutureVoid_SWIG_FreeCompletionData(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_FutureVoid")]
		public static extern void delete_FutureVoid(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_AppOptionsInternal")]
		internal static extern IntPtr new_AppOptionsInternal();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_SetDatabaseUrlInternal")]
		internal static extern void AppOptionsInternal_SetDatabaseUrlInternal(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_GetDatabaseUrlInternal")]
		internal static extern string AppOptionsInternal_GetDatabaseUrlInternal(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_LoadFromJsonConfig__SWIG_0")]
		public static extern IntPtr AppOptionsInternal_LoadFromJsonConfig__SWIG_0(string jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_LoadFromJsonConfig__SWIG_1")]
		public static extern IntPtr AppOptionsInternal_LoadFromJsonConfig__SWIG_1(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_AppId_set")]
		public static extern void AppOptionsInternal_AppId_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_AppId_get")]
		public static extern string AppOptionsInternal_AppId_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_ApiKey_set")]
		public static extern void AppOptionsInternal_ApiKey_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_ApiKey_get")]
		public static extern string AppOptionsInternal_ApiKey_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_MessageSenderId_set")]
		public static extern void AppOptionsInternal_MessageSenderId_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_MessageSenderId_get")]
		public static extern string AppOptionsInternal_MessageSenderId_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_StorageBucket_set")]
		public static extern void AppOptionsInternal_StorageBucket_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_StorageBucket_get")]
		public static extern string AppOptionsInternal_StorageBucket_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_ProjectId_set")]
		public static extern void AppOptionsInternal_ProjectId_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_ProjectId_get")]
		public static extern string AppOptionsInternal_ProjectId_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_PackageName_set")]
		public static extern void AppOptionsInternal_PackageName_set(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsInternal_PackageName_get")]
		public static extern string AppOptionsInternal_PackageName_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_AppOptionsInternal")]
		internal static extern void delete_AppOptionsInternal(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_FirebaseApp")]
		public static extern void delete_FirebaseApp(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_options")]
		public static extern IntPtr FirebaseApp_options(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_SetDataCollectionDefaultEnabledInternal")]
		internal static extern void FirebaseApp_SetDataCollectionDefaultEnabledInternal(HandleRef jarg1, bool jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_IsDataCollectionDefaultEnabledInternal")]
		internal static extern bool FirebaseApp_IsDataCollectionDefaultEnabledInternal(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_NameInternal_get")]
		public static extern string FirebaseApp_NameInternal_get(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_CreateInternal__SWIG_0")]
		public static extern IntPtr FirebaseApp_CreateInternal__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_CreateInternal__SWIG_1")]
		public static extern IntPtr FirebaseApp_CreateInternal__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_CreateInternal__SWIG_2")]
		public static extern IntPtr FirebaseApp_CreateInternal__SWIG_2(HandleRef jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_SetLogLevelInternal")]
		internal static extern void FirebaseApp_SetLogLevelInternal(int jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_GetLogLevelInternal")]
		internal static extern int FirebaseApp_GetLogLevelInternal();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_RegisterLibraryInternal")]
		internal static extern void FirebaseApp_RegisterLibraryInternal(string jarg1, string jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_AppSetDefaultConfigPath")]
		public static extern void FirebaseApp_AppSetDefaultConfigPath(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FirebaseApp_DefaultName_get")]
		public static extern string FirebaseApp_DefaultName_get();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_PollCallbacks")]
		public static extern void PollCallbacks();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppEnableLogCallback")]
		public static extern void AppEnableLogCallback(bool jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppGetLogLevel")]
		public static extern int AppGetLogLevel();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_SetEnabledAllAppCallbacks")]
		public static extern void SetEnabledAllAppCallbacks(bool jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_SetEnabledAppCallbackByName")]
		public static extern void SetEnabledAppCallbackByName(string jarg1, bool jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_SetLogFunction")]
		public static extern void SetLogFunction(FirebaseApp.LogMessageDelegate jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_AppOptionsLoadFromJsonConfig")]
		public static extern IntPtr AppOptionsLoadFromJsonConfig(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CleanupNotifierBridge_RegisterCleanupDelegate")]
		public static extern bool CleanupNotifierBridge_RegisterCleanupDelegate(IntPtr jarg1, IntPtr jarg2, CleanupNotifierBridge.CleanupDelegate jarg3, IntPtr jarg4);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CleanupNotifierBridge_UnregisterCleanupDelegate")]
		public static extern void CleanupNotifierBridge_UnregisterCleanupDelegate(IntPtr jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CleanupNotifierBridge_GetAndDestroyNotifiedFlag")]
		public static extern bool CleanupNotifierBridge_GetAndDestroyNotifiedFlag(IntPtr jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_CheckAndroidDependencies")]
		public static extern int CheckAndroidDependencies();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FixAndroidDependencies")]
		public static extern IntPtr FixAndroidDependencies();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_InitializePlayServicesInternal")]
		internal static extern void InitializePlayServicesInternal();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_TerminatePlayServicesInternal")]
		internal static extern void TerminatePlayServicesInternal();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_VariantVariantMap__SWIG_0")]
		public static extern IntPtr new_VariantVariantMap__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_VariantVariantMap__SWIG_1")]
		public static extern IntPtr new_VariantVariantMap__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_size")]
		public static extern uint VariantVariantMap_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_empty")]
		public static extern bool VariantVariantMap_empty(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_Clear")]
		public static extern void VariantVariantMap_Clear(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_getitem")]
		public static extern IntPtr VariantVariantMap_getitem(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_setitem")]
		public static extern void VariantVariantMap_setitem(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_ContainsKey")]
		public static extern bool VariantVariantMap_ContainsKey(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_Add")]
		public static extern void VariantVariantMap_Add(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_Remove")]
		public static extern bool VariantVariantMap_Remove(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_create_iterator_begin")]
		public static extern IntPtr VariantVariantMap_create_iterator_begin(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_get_next_key")]
		public static extern IntPtr VariantVariantMap_get_next_key(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantVariantMap_destroy_iterator")]
		public static extern void VariantVariantMap_destroy_iterator(HandleRef jarg1, IntPtr jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_VariantVariantMap")]
		public static extern void delete_VariantVariantMap(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Clear")]
		public static extern void VariantList_Clear(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Add")]
		public static extern void VariantList_Add(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_size")]
		public static extern uint VariantList_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_capacity")]
		public static extern uint VariantList_capacity(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_reserve")]
		public static extern void VariantList_reserve(HandleRef jarg1, uint jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_VariantList__SWIG_0")]
		public static extern IntPtr new_VariantList__SWIG_0();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_VariantList__SWIG_1")]
		public static extern IntPtr new_VariantList__SWIG_1(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_new_VariantList__SWIG_2")]
		public static extern IntPtr new_VariantList__SWIG_2(int jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_getitemcopy")]
		public static extern IntPtr VariantList_getitemcopy(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_getitem")]
		public static extern IntPtr VariantList_getitem(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_setitem")]
		public static extern void VariantList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_AddRange")]
		public static extern void VariantList_AddRange(HandleRef jarg1, HandleRef jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_GetRange")]
		public static extern IntPtr VariantList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Insert")]
		public static extern void VariantList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_InsertRange")]
		public static extern void VariantList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_RemoveAt")]
		public static extern void VariantList_RemoveAt(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_RemoveRange")]
		public static extern void VariantList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Repeat")]
		public static extern IntPtr VariantList_Repeat(HandleRef jarg1, int jarg2);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Reverse__SWIG_0")]
		public static extern void VariantList_Reverse__SWIG_0(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_Reverse__SWIG_1")]
		public static extern void VariantList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_VariantList_SetRange")]
		public static extern void VariantList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_VariantList")]
		public static extern void delete_VariantList(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_delete_Variant")]
		public static extern void delete_Variant(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_Null")]
		public static extern IntPtr Variant_Null();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_EmptyVector")]
		public static extern IntPtr Variant_EmptyVector();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_EmptyMap")]
		public static extern IntPtr Variant_EmptyMap();

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_EmptyMutableBlob")]
		public static extern IntPtr Variant_EmptyMutableBlob(uint jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_type")]
		public static extern int Variant_type(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_is_string")]
		public static extern bool Variant_is_string(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_is_fundamental_type")]
		public static extern bool Variant_is_fundamental_type(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_AsString")]
		public static extern IntPtr Variant_AsString(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_blob_size")]
		public static extern uint Variant_blob_size(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_vector__SWIG_0")]
		public static extern IntPtr Variant_vector__SWIG_0(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_map__SWIG_0")]
		public static extern IntPtr Variant_map__SWIG_0(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_int64_value")]
		public static extern long Variant_int64_value(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_double_value")]
		public static extern double Variant_double_value(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_bool_value")]
		public static extern bool Variant_bool_value(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_string_value")]
		public static extern string Variant_string_value(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_FromInt64")]
		public static extern IntPtr Variant_FromInt64(long jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_FromDouble")]
		public static extern IntPtr Variant_FromDouble(double jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_FromBool")]
		public static extern IntPtr Variant_FromBool(bool jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_FromString")]
		public static extern IntPtr Variant_FromString(string jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_Variant_untyped_mutable_blob_data")]
		public static extern IntPtr Variant_untyped_mutable_blob_data(HandleRef jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureString_SWIGUpcast")]
		public static extern IntPtr FutureString_SWIGUpcast(IntPtr jarg1);

		[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "Firebase_App_CSharp_FutureVoid_SWIGUpcast")]
		public static extern IntPtr FutureVoid_SWIGUpcast(IntPtr jarg1);

		protected class SWIGExceptionHelper
		{
			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingApplicationException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingArithmeticException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingDivideByZeroException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingIndexOutOfRangeException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingInvalidCastException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingInvalidOperationException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingIOException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingNullReferenceException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingOutOfMemoryException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingOverflowException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingSystemException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingArgumentException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingArgumentNullException);

			private static AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate = new AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(AppUtilPINVOKE.SWIGExceptionHelper.SetPendingArgumentOutOfRangeException);

			static SWIGExceptionHelper()
			{
				AppUtilPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacks_AppUtil(AppUtilPINVOKE.SWIGExceptionHelper.applicationDelegate, AppUtilPINVOKE.SWIGExceptionHelper.arithmeticDelegate, AppUtilPINVOKE.SWIGExceptionHelper.divideByZeroDelegate, AppUtilPINVOKE.SWIGExceptionHelper.indexOutOfRangeDelegate, AppUtilPINVOKE.SWIGExceptionHelper.invalidCastDelegate, AppUtilPINVOKE.SWIGExceptionHelper.invalidOperationDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ioDelegate, AppUtilPINVOKE.SWIGExceptionHelper.nullReferenceDelegate, AppUtilPINVOKE.SWIGExceptionHelper.outOfMemoryDelegate, AppUtilPINVOKE.SWIGExceptionHelper.overflowDelegate, AppUtilPINVOKE.SWIGExceptionHelper.systemDelegate);
				AppUtilPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacksArgument_AppUtil(AppUtilPINVOKE.SWIGExceptionHelper.argumentDelegate, AppUtilPINVOKE.SWIGExceptionHelper.argumentNullDelegate, AppUtilPINVOKE.SWIGExceptionHelper.argumentOutOfRangeDelegate);
			}

			[DllImport("FirebaseCppApp-5.2.1")]
			public static extern void SWIGRegisterExceptionCallbacks_AppUtil(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemExceptionDelegate);

			[DllImport("FirebaseCppApp-5.2.1", EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_AppUtil")]
			public static extern void SWIGRegisterExceptionCallbacksArgument_AppUtil(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate, AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate);

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingApplicationException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new ApplicationException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingArithmeticException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new ArithmeticException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingDivideByZeroException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new DivideByZeroException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingIndexOutOfRangeException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new IndexOutOfRangeException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingInvalidCastException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new InvalidCastException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingInvalidOperationException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new InvalidOperationException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingIOException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new IOException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingNullReferenceException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new NullReferenceException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingOutOfMemoryException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new OutOfMemoryException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingOverflowException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new OverflowException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionDelegate))]
			private static void SetPendingSystemException(string message)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new SystemException(message, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentException(string message, string paramName)
			{
				AppUtilPINVOKE.SWIGPendingException.Set(new ArgumentException(message, paramName, AppUtilPINVOKE.SWIGPendingException.Retrieve()));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentNullException(string message, string paramName)
			{
				Exception ex = AppUtilPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				AppUtilPINVOKE.SWIGPendingException.Set(new ArgumentNullException(paramName, message));
			}

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate))]
			private static void SetPendingArgumentOutOfRangeException(string message, string paramName)
			{
				Exception ex = AppUtilPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				AppUtilPINVOKE.SWIGPendingException.Set(new ArgumentOutOfRangeException(paramName, message));
			}

			public delegate void ExceptionDelegate(string message);

			public delegate void ExceptionArgumentDelegate(string message, string paramName);
		}

		public class SWIGPendingException
		{
			[ThreadStatic]
			private static Exception pendingException;

			private static int numExceptionsPending;

			public static bool Pending
			{
				get
				{
					bool result = false;
					if (AppUtilPINVOKE.SWIGPendingException.numExceptionsPending > 0 && AppUtilPINVOKE.SWIGPendingException.pendingException != null)
					{
						result = true;
					}
					return result;
				}
			}

			public static void Set(Exception e)
			{
				if (AppUtilPINVOKE.SWIGPendingException.pendingException != null)
				{
					throw new ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + AppUtilPINVOKE.SWIGPendingException.pendingException.ToString() + ")", e);
				}
				AppUtilPINVOKE.SWIGPendingException.pendingException = e;
				object typeFromHandle = typeof(AppUtilPINVOKE);
				lock (typeFromHandle)
				{
					AppUtilPINVOKE.SWIGPendingException.numExceptionsPending++;
				}
			}

			public static Exception Retrieve()
			{
				Exception result = null;
				if (AppUtilPINVOKE.SWIGPendingException.numExceptionsPending > 0 && AppUtilPINVOKE.SWIGPendingException.pendingException != null)
				{
					result = AppUtilPINVOKE.SWIGPendingException.pendingException;
					AppUtilPINVOKE.SWIGPendingException.pendingException = null;
					object typeFromHandle = typeof(AppUtilPINVOKE);
					lock (typeFromHandle)
					{
						AppUtilPINVOKE.SWIGPendingException.numExceptionsPending--;
					}
				}
				return result;
			}
		}

		protected class SWIGStringHelper
		{
			private static AppUtilPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate = new AppUtilPINVOKE.SWIGStringHelper.SWIGStringDelegate(AppUtilPINVOKE.SWIGStringHelper.CreateString);

			static SWIGStringHelper()
			{
				AppUtilPINVOKE.SWIGStringHelper.SWIGRegisterStringCallback_AppUtil(AppUtilPINVOKE.SWIGStringHelper.stringDelegate);
			}

			[DllImport("FirebaseCppApp-5.2.1")]
			public static extern void SWIGRegisterStringCallback_AppUtil(AppUtilPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate);

			[MonoPInvokeCallback(typeof(AppUtilPINVOKE.SWIGStringHelper.SWIGStringDelegate))]
			private static string CreateString(string cString)
			{
				return cString;
			}

			public delegate string SWIGStringDelegate(string message);
		}
	}
}
